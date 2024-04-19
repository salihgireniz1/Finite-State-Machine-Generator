using System;
using UnityEngine;

public class Idle : BaseState<CharacterStateMachine>
{
    public Idle(CharacterStateMachine stateMachine) : base(stateMachine)
    {
        if (stateMachine == null) throw new ArgumentNullException(nameof(stateMachine));
    }

    public override void Enter() { }
    public override void StateUpdate() { }
    public override void StateLateUpdate() { }
    public override void StateFixedUpdate() { }
}
