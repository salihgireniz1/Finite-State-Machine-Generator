/// <summary>
/// A state representing the normal state of the character.
/// </summary>
/// <param name="stateMachine">The state machine that this state belongs to.</param>
public class NormalState : BaseState<CharacterStateMachine>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NormalState"/> class.
    /// </summary>
    /// <param name="stateMachine">The state machine that this state belongs to.</param>
    /// <exception cref="ArgumentNullException">Thrown if the provided state machine is null.</exception>
    public NormalState(CharacterStateMachine stateMachine) : base(stateMachine)
    {
        if (stateMachine == null)
        {
            throw new System.ArgumentNullException(nameof(stateMachine));
        }
    }

    public override void Enter()
    {
    }

    public override void StateUpdate()
    {
    }

    public override void StateLateUpdate()
    {
    }

    public override void StateFixedUpdate()
    {
    }
}