/// <summary>
/// A base class for character states.
/// </summary>
public class BaseState<T> : IState where T : BaseStateMachine
{
    /// <summary>
    /// The state machine that this state is used by.
    /// </summary>
    public readonly T stateMachine;

    /// <summary>
    /// Initializes a new instance of the <see cref="CharacterBaseState"/> class.
    /// </summary>
    /// <param name="stateMachine">The state machine that this state is used by.</param>
    public BaseState(T stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    /// <inheritdoc />
    public virtual void Enter() { }

    /// <inheritdoc />
    public virtual void Exit() { }

    /// <inheritdoc />
    public virtual void StateUpdate() { }

    /// <inheritdoc />
    public virtual void StateLateUpdate() { }

    /// <inheritdoc />
    public virtual void StateFixedUpdate() { }
}