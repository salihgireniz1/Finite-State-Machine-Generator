using System.Collections.Generic;
using UnityEngine;

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

    [Tooltip("Index of the default state in the states array.")]
    public int defaultStateIndex;

    [Tooltip("The file path where the state machine scripts will be generated.")]
    [Space(10)]
    [Header("File Settings")]
    public string path;





}

