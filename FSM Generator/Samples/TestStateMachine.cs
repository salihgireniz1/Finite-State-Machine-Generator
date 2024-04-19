using R3;
using System;
using UnityEngine;

public class TestStateMachine : BaseStateMachine
{
    public IState One;
    public IState Two;

    protected override void GenerateStates()
    {
        One = new One(this);
        Two = new Two(this);
    }
}
