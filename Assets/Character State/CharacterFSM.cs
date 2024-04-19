
public class CharacterFSM : BaseStateMachine
{
    public IState Idle;
    public IState Walking;
    public IState TTT;
    public IState asd;

    public override void ChangeState(IState newState)
    {
        base.ChangeState(newState);
    }

    protected override void GenerateStates()
    {
        this.Idle = new Idle(this);
        this.Walking = new Walking(this);
        this.TTT = new TTT(this);
        this.asd = new asd(this);
        this.defaultState = Idle;
    }
}
