using System;
using UnityEngine;

public class Idle : BaseState<CharacterFSM>
{
    public Idle(CharacterFSM stateMachine) : base(stateMachine)
    {
        if (stateMachine == null) throw new ArgumentNullException(nameof(stateMachine));
    }

    public override void Enter() { }
    public override void StateUpdate() { }
    public override void StateLateUpdate() { }
    public override void StateFixedUpdate() { }
}
