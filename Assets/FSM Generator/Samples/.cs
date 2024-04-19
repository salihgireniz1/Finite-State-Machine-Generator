    public IState ;
using System;
using UnityEngine;

public class  : BaseState<CharacterStateMachineStateMachine>
{
    public (CharacterStateMachineStateMachine stateMachine) : base(stateMachine)
    {
        if (stateMachine == null) throw new ArgumentNullException(nameof(stateMachine));
    }

    public override void Enter() { }
    public override void StateUpdate() { }
    public override void StateLateUpdate() { }
    public override void StateFixedUpdate() { }
    protected override void GenerateStates()
    {
        this. = new (this);
        this.defaultState = ;
    }

}
