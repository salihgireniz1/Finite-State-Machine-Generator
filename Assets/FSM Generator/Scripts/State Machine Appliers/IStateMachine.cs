/// <summary>
/// Interface for a state machine that manages IState instances.
/// </summary>
public interface IStateMachine
{
    public IState CurrentState { get; }

    /// <summary>
    /// Exits current state, replaces it with the new state and enters back.
    /// </summary>
    /// <param name="newState">New state of object.</param>
    void ChangeState(IState newState);
}
