using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FSMGenerator))]
public class FSMGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FSMGenerator generator = (FSMGenerator)target;

        DrawDefaultInspector(); // Draws the default inspector

        // Managing dynamic list of states
        if (generator.states == null || generator.states.Count == 0)
        {
            // Only add a new entry if the last one isn't empty
            generator.states.Add("");
        }

        // Dropdown for selecting the default state
        if (generator.states.Count > 0)
        {
            generator.defaultStateIndex = EditorGUILayout.Popup("Default State", generator.defaultStateIndex, generator.states.ToArray());
        }

        // Generates or updates the FSM when the "Generate/Update FSM" button is clicked.
        if (GUILayout.Button("Generate/Update FSM"))
        {
            generator.GenerateFSM();
        }
    }
}