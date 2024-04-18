

/// <summary>
/// A state machine for a character.
/// </summary>
public class CharacterStateMachine : BaseStateMachine
{
    #region Defined States
    /// <summary>
    /// The default state for the character.
    /// </summary>
    public IState NormalState;
    #endregion

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        defaultState = NormalState;
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void GenerateStates()
    {
        NormalState = new NormalState(this);
    }
}