
public class CharacterStateMachine : BaseStateMachine
{
    public IState Walking;
    public IState Idle;

    public override void ChangeState(IState newState)
    {
        base.ChangeState(newState);
    }

    protected override void GenerateStates()
    {
        this.Walking = new Walking(this);
        this.Idle = new Idle(this);
        this.defaultState = Idle;
    }
}
