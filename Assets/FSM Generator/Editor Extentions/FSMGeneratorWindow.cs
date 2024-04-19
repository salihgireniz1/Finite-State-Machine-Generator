using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

/// <summary>
/// A window for creating and managing Finite State Machine Generators.
/// </summary>
public class FSMGeneratorWindow : EditorWindow
{
    private string stateMachineName = "NewStateMachine";
    private string path = "FSM";
    private List<string> states = new List<string>();
    public int defaultStateIndex;

    /// <summary>
    /// Opens the FSM Generator window.
    /// </summary>
    [MenuItem("Tools/FSM Generator")]
    public static void ShowWindow()
    {
        GetWindow<FSMGeneratorWindow>("FSM Generator");
    }

    /// <summary>
    /// Handles the GUI for the window.
    /// </summary>
    private void OnGUI()
    {
        GUILayout.Label("Create a New Finite State Machine", EditorStyles.boldLabel);

        stateMachineName = EditorGUILayout.TextField("State Machine Name", stateMachineName);
        path = EditorGUILayout.TextField("Path (relative to Assets/)", path);

        GUILayout.Label("States:");
        if (states.Count == 0 || !string.IsNullOrEmpty(states[states.Count - 1]))
        {
            states.Add("");
        }

        int removeIndex = -1;
        for (int i = 0; i < states.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            states[i] = EditorGUILayout.TextField(states[i]);
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                removeIndex = i;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex != -1)
        {
            states.RemoveAt(removeIndex);
        }

        // Ensure there's no null or empty strings in the list for the dropdown to work correctly
        states.RemoveAll(string.IsNullOrEmpty);

        if (states.Count > 0)
        {
            GUILayout.Label("Default State:");
            defaultStateIndex = EditorGUILayout.Popup(defaultStateIndex, states.ToArray());
        }

        // Button to create the FSM
        if (GUILayout.Button("Generate FSM", GUILayout.Height(25)))
        {
            CreateOrUpdateFSMGenerator();
        }
    }


    /// <summary>
    /// Creates or updates an FSM Generator.
    /// </summary>
    private void CreateOrUpdateFSMGenerator()
    {
        if (string.IsNullOrEmpty(stateMachineName))
        {
            EditorUtility.DisplayDialog("Error", "State Machine name cannot be empty.", "OK");
            return;
        }

        states.RemoveAll(string.IsNullOrEmpty);

        string assetPath = AssetDatabase.FindAssets($"{stateMachineName} t:FSMGenerator")
            .Select(AssetDatabase.GUIDToAssetPath)
            .FirstOrDefault(p => p.EndsWith($"{stateMachineName} Generator.asset"));

        if (!string.IsNullOrEmpty(assetPath))
        {
            FSMGenerator existingGenerator = AssetDatabase.LoadAssetAtPath<FSMGenerator>(assetPath);
            if (existingGenerator != null)
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = existingGenerator;
                if (EditorUtility.DisplayDialog("Existing Generator Found", $"A generator for '{stateMachineName}' already exists. Do you want to update it?", "Yes", "No"))
                {
                    UpdateExistingGenerator(existingGenerator);
                    return;
                }
            }
        }

        CreateNewFSMGenerator();
    }

    /// <summary>
    /// Creates a new FSMGenerator asset.
    /// </summary>
    private void CreateNewFSMGenerator()
    {
        FSMGenerator generator = CreateInstance<FSMGenerator>();
        generator.stateMachineName = stateMachineName;
        generator.states = states;
        generator.defaultStateIndex = defaultStateIndex;
        generator.path = NormalizePath(path);

        string fullPath = Path.Combine("Assets", generator.path, $"{stateMachineName} Generator.asset");
        fullPath = AssetDatabase.GenerateUniqueAssetPath(fullPath);

        if (!Directory.Exists(Path.Combine(Application.dataPath, generator.path)))
        {
            Directory.CreateDirectory(Path.Combine(Application.dataPath, generator.path));
        }

        AssetDatabase.CreateAsset(generator, fullPath);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = generator;
    }

    /// <summary>
    /// Updates an existing FSMGenerator asset.
    /// </summary>
    /// <param name="existingGenerator">The existing FSMGenerator to update.</param>
    private void UpdateExistingGenerator(FSMGenerator existingGenerator)
    {
        existingGenerator.states = states;
        existingGenerator.defaultStateIndex = defaultStateIndex;
        existingGenerator.path = NormalizePath(path);

        EditorUtility.SetDirty(existingGenerator);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = existingGenerator;
    }

    /// <summary>
    /// Normalizes the path to ensure it does not start with 'Assets/'.
    /// </summary>
    /// <param name="inputPath">The input path to normalize.</param>
    /// <returns>The normalized path.</returns>
    private string NormalizePath(string inputPath)
    {
        if (inputPath.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase))
        {
            return inputPath.Substring("Assets/".Length);
        }
        return inputPath;
    }
}
