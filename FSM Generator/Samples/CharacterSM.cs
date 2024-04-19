
public class CharacterSM : BaseStateMachine
{
    public IState Idle;
    public IState Walking;

    public override void ChangeState(IState newState)
    {
        base.ChangeState(newState);
    }

    protected override void GenerateStates()
    {
        this.Idle = new Idle(this);
        this.Walking = new Walking(this);
        this.defaultState = Idle;
    }
}
