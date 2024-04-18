/// <summary>
/// Interface for a state that can be used by a state machine.
/// </summary>
/// <typeparam name="T">The type of the state machine that this state is used by.</typeparam>
/// <remarks>
/// This interface defines the methods that a state must implement.
/// </remarks>
public interface IState
{
    /// <summary>
    /// Called when the state is entered.
    /// </summary>
    void Enter();

    /// <summary>
    /// Called during the fixed update loop.
    /// </summary>
    void StateFixedUpdate();

    /// <summary>
    /// Called during the update loop.
    /// </summary>
    void StateUpdate();

    /// <summary>
    /// Called during the late update loop.
    /// </summary>
    void StateLateUpdate();

    /// <summary>
    /// Called when the state is exited.
    /// </summary>
    void Exit();
}
