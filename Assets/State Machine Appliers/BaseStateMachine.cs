using R3;
using System;
using UnityEngine;
using System.Threading;
public class BaseStateMachine : MonoBehaviour
{
    public IState CurrentState { get => currentState; }
    protected IState currentState;
    protected IState defaultState;
    protected IDisposable updateDisposable;
    protected IDisposable fixedUpdateDisposable;
    protected IDisposable lateUpdateDisposable;
    protected CancellationTokenSource cancellationTokenSource;

    #region MonoBehaviour Callbacks

    protected virtual void Awake()
    {
        GenerateStates();
    }
    protected virtual void Start()
    {
        if (defaultState != null) ChangeState(defaultState);
        StartUpdate();
        StartFixedUpdate();
        StartLateUpdate();
    }
    private void OnDestroy()
    {
        StopUpdates();
    }
    #endregion

    #region Overrideables

    ///<inheritdoc/>
    public virtual void ChangeState(IState newState)
    {
        if (newState == null)
        {
            Debug.LogError("New state is null.");
            return;
        }

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
    /// <summary>
    /// Generates the defined states for the state machine.
    /// </summary>s
    protected virtual void GenerateStates() { }
    #endregion

    #region Persistant Methods
    protected void StartUpdate()
    {
        cancellationTokenSource = new();
        updateDisposable = Observable.EveryUpdate(UnityFrameProvider.Update, cancellationTokenSource.Token)
            .Subscribe(_ => CurrentState?.StateUpdate());
    }
    protected void StartFixedUpdate()
    {
        cancellationTokenSource = cancellationTokenSource == null ? new() : cancellationTokenSource;
        fixedUpdateDisposable = Observable.EveryUpdate(UnityFrameProvider.FixedUpdate, cancellationTokenSource.Token)
            .Subscribe(_ => CurrentState?.StateFixedUpdate());
    }
    protected void StartLateUpdate()
    {
        cancellationTokenSource = cancellationTokenSource == null ? new() : cancellationTokenSource;
        lateUpdateDisposable = Observable.EveryUpdate(UnityFrameProvider.PreLateUpdate, cancellationTokenSource.Token)
            .Subscribe(_ => CurrentState?.StateLateUpdate());
    }
    protected void StopUpdates()
    {
        cancellationTokenSource?.Cancel();
        fixedUpdateDisposable?.Dispose();
        lateUpdateDisposable?.Dispose();
        updateDisposable?.Dispose();
    }
    #endregion
}