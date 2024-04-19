using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

/// <summary>
/// Defines a scriptable object for generating a finite state machine.
/// </summary>
[CreateAssetMenu(fileName = "FSMGenerator", menuName = "FSM/Generator", order = 1)]
public class FSMGenerator : ScriptableObject
{
    [Tooltip("The name of the state machine class to be generated.")]
    [Header("State Machine Settings")]
    public string stateMachineName;

    [Tooltip("List of state names to be included in the state machine.")]
    [Space(10)]
    public List<string> states = new List<string>();

    [Tooltip("The file path where the state machine scripts will be generated.")]
    [Space(10)]
    [Header("File Settings")]
    public string path;

    [HideInInspector]
    public int defaultStateIndex;
    public Task<bool> CanGenerate()
    {
        if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
        {
            Debug.LogError("Please provide a path.");
            return Task.FromResult(false);
        }
        if (string.IsNullOrEmpty(stateMachineName) || string.IsNullOrWhiteSpace(stateMachineName))
        {
            Debug.LogError("Please provide a name for state machine.");
            return Task.FromResult(false);
        }
        else
        {
            stateMachineName = stateMachineName.NormalizeName();
        }
        // Remove all whitespaces from the states.
        for (int i = 0; i < states.Count; i++)
        {
            states[i] = states[i].NormalizeName();
        }
        // Removes all null or empty strings from the states list
        states.RemoveAll(string.IsNullOrEmpty);
        // Marks the generator as dirty to ensure changes are saved
        EditorUtility.SetDirty(this);

        if (states == null || states.Count == 0)
        {
            Debug.LogError("Please at least one state to insert into machine.");
            return Task.FromResult(false);
        }
        return Task.FromResult(true);
    }
    public async void GenerateFSM()
    {
        bool canGenerate = await CanGenerate();
        if (!canGenerate) return;

        Debug.Log($"Generating FSM at path: {path}");

        // Ensure the directory exists
        if (!Directory.Exists(path))
        {
            Debug.Log("Directory does not exist, creating new directory.");
            Directory.CreateDirectory(path);
        }
        for (int i = 0; i < states.Count; i++)
        {
            string filePath = Path.Combine(path, states[i] + ".cs");
            if (!File.Exists(filePath))
            {
                string stateClassContent = GenerateStateClass(states[i]);
                await File.WriteAllTextAsync(filePath, stateClassContent);
                Debug.Log($"Generated state class at: {filePath}");
            }
            else
            {
                Debug.Log($"File {filePath} already exists. Skipping to preserve existing content.");
            }
        }
        // // Generate each state class
        // foreach (string state in states)
        // {
        //     string filePath = Path.Combine(path, state + ".cs");
        //     if (!File.Exists(filePath))
        //     {
        //         string stateClassContent = GenerateStateClass(state);
        //         await File.WriteAllTextAsync(filePath, stateClassContent);
        //         Debug.Log($"Generated state class at: {filePath}");
        //     }
        //     else
        //     {
        //         Debug.Log($"File {filePath} already exists. Skipping to preserve existing content.");
        //     }
        // }


        // Update the state machine class
        string stateMachineFilePath = Path.Combine(path, stateMachineName + ".cs");
        await UpdateStateMachineClass(stateMachineFilePath);

        AssetDatabase.Refresh();
    }

    private async Task UpdateStateMachineClass(string filePath)
    {
        if (!File.Exists(filePath))
        {
            // Generate from scratch if file does not exist
            string stateMachineClassContent = GenerateStateMachineClass();
            await File.WriteAllTextAsync(filePath, stateMachineClassContent);
        }
        else
        {
            // Read the existing file lines
            List<string> lines = new List<string>(File.ReadAllLines(filePath));
            HashSet<string> existingStates = new HashSet<string>();
            int generateStatesMethodStartIndex = -1;
            int generateStatesMethodEndIndex = -1;
            int lastStateDeclarationIndex = -1;

            // Find missing state declarations
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i].Trim();
                if (line.StartsWith("public IState"))
                {
                    lastStateDeclarationIndex = i;
                    string stateName = line.Split(new[] { "public IState", ";" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.Trim();
                    if (!string.IsNullOrEmpty(stateName))
                    {
                        existingStates.Add(stateName);
                    }
                }
            }

            // Add missing state declarations
            foreach (string state in states)
            {
                if (!existingStates.Contains(state))
                {
                    lastStateDeclarationIndex++;
                    lines.Insert(lastStateDeclarationIndex, $"    public IState {state};");
                }
            }

            // Locate GenerateStates method's scope and state declarations
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i].Trim();
                if (line.StartsWith("protected override void GenerateStates()"))
                {
                    if (line.Trim().EndsWith("{")) generateStatesMethodStartIndex = i; // Incase if user opens method at the end of the method definition instead of under it.
                    else generateStatesMethodStartIndex = i + 1; // +1 to skip the line with method start signature
                    i++;

                    int depth = 1;
                    while (i < lines.Count && depth != 0)
                    {
                        i++;
                        if (lines[i].Trim() == "{") depth++;
                        if (lines[i].Trim() == "}") depth--;
                    }
                    generateStatesMethodEndIndex = i;
                }

            }

            if (generateStatesMethodStartIndex != -1 && generateStatesMethodEndIndex != -1)
            {
                // Clear existing state initializations
                lines.RemoveRange(generateStatesMethodStartIndex + 1, generateStatesMethodEndIndex - 1 - generateStatesMethodStartIndex);

                // Reconstruct GenerateStates with all states
                List<string> stateInitializations = states.Select(state => $"        this.{state} = new {state}(this);").ToList();
                lines.InsertRange(generateStatesMethodStartIndex + 1, stateInitializations);

                // Update the end index to reflect added lines
                generateStatesMethodEndIndex = generateStatesMethodStartIndex + stateInitializations.Count;

                // Finally, add the default state definition line.
                string defaultStateDefinition = $"        this.defaultState = {states[defaultStateIndex]};";
                List<string> defaultStateDefinitionAsList = new List<string>() { defaultStateDefinition };
                lines.InsertRange(generateStatesMethodEndIndex + 1, defaultStateDefinitionAsList);
            }
            else
            {
                // If GenerateStates method does not exist, we need to create it and append it at the end of the class.
                // Find the index of the class's closing brace.
                int classEndIndex = -1;
                for (int i = lines.Count - 1; i >= 0; i--)
                {
                    if (lines[i].Trim() == "}")
                    {
                        classEndIndex = i;
                        break;
                    }
                }

                // If we have found the class's closing brace, we insert the GenerateStates method above it.
                if (classEndIndex != -1)
                {
                    List<string> generateStatesMethod = new List<string>
            {
                "    protected override void GenerateStates()",
                "    {"
            };

                    // Add state initializations
                    generateStatesMethod.AddRange(states.Select(state => $"        this.{state} = new {state}(this);"));
                    generateStatesMethod.Add($"        this.defaultState = {states[defaultStateIndex]};");
                    generateStatesMethod.Add("    }");
                    generateStatesMethod.Add(""); // For a blank line before the class's closing brace.

                    // Insert the new GenerateStates method before the class's closing brace.
                    lines.InsertRange(classEndIndex, generateStatesMethod);
                }
                else
                {
                    // If the class end brace was not found, log an error.
                    Debug.LogError($"Could not find the class end for {stateMachineName}StateMachine to insert GenerateStates.");
                }
            }

            await File.WriteAllLinesAsync(filePath, lines);
        }
    }

    private string GenerateStateClass(string stateName)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine();
        sb.AppendLine($"public class {stateName} : BaseState<{stateMachineName}>");
        sb.AppendLine("{");
        sb.AppendLine($"    public {stateName}({stateMachineName} stateMachine) : base(stateMachine)");
        sb.AppendLine("    {");
        sb.AppendLine("        if (stateMachine == null) throw new ArgumentNullException(nameof(stateMachine));");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    public override void Enter() { }");
        sb.AppendLine("    public override void StateUpdate() { }");
        sb.AppendLine("    public override void StateLateUpdate() { }");
        sb.AppendLine("    public override void StateFixedUpdate() { }");
        sb.AppendLine("}");
        return sb.ToString();
    }

    private string GenerateStateMachineClass()
    {
        StringBuilder sb = new StringBuilder();
        // sb.AppendLine("using R3;");
        // sb.AppendLine("using System;");
        // sb.AppendLine("using UnityEngine;");
        sb.AppendLine();
        sb.AppendLine($"public class {stateMachineName} : BaseStateMachine");
        sb.AppendLine("{");
        foreach (string state in states)
        {
            sb.AppendLine($"    public IState {state};");
        }

        sb.AppendLine();

        sb.AppendLine("    public override void ChangeState(IState newState)");
        sb.AppendLine("    {");
        sb.AppendLine("        base.ChangeState(newState);");
        sb.AppendLine("    }");

        sb.AppendLine();

        sb.AppendLine("    protected override void GenerateStates()");
        sb.AppendLine("    {");
        foreach (string state in states)
        {
            sb.AppendLine($"        this.{state} = new {state}(this);");
        }
        sb.AppendLine($"        this.defaultState = {states[defaultStateIndex]};");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        return sb.ToString();
    }
}

