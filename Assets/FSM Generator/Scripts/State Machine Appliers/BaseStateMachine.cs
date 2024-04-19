using R3;
using System;
using UnityEngine;
using System.Threading;

/// <summary>
/// Base class for all state machines.
/// </summary>
public class BaseStateMachine : MonoBehaviour
{
    public IState CurrentState => currentState;
    protected IState currentState;
    protected IState defaultState;

    // A single CancellationTokenSource for all updates
    protected CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    // CompositeDisposable to manage all subscriptions
    protected CompositeDisposable disposables = new CompositeDisposable();

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

    protected virtual void OnDestroy()
    {
        StopUpdates();
    }

    #endregion

    #region Overrideables

    /// <summary>
    /// Changes the current state to the new state provided.
    /// </summary>
    /// <param name="newState">The new state to transition to.</param>
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
    /// Generates the defined states for the state machine. Should be overridden to initialize states.
    /// </summary>
    protected virtual void GenerateStates()
    {
        // Intended to be overridden by derived classes to set up states.
    }

    #endregion

    #region Update Methods

    /// <summary>
    /// Subscribes to the update loop.
    /// </summary>
    protected void StartUpdate()
    {
        var updateSubscription = Observable.EveryUpdate(UnityFrameProvider.Update, cancellationTokenSource.Token)
            .Subscribe(_ => CurrentState?.StateUpdate());
        disposables.Add(updateSubscription);
    }

    /// <summary>
    /// Subscribes to the fixed update loop.
    /// </summary>
    protected void StartFixedUpdate()
    {
        var fixedUpdateSubscription = Observable.EveryUpdate(UnityFrameProvider.FixedUpdate, cancellationTokenSource.Token)
            .Subscribe(_ => CurrentState?.StateFixedUpdate());
        disposables.Add(fixedUpdateSubscription);
    }

    /// <summary>
    /// Subscribes to the late update loop.
    /// </summary>
    protected void StartLateUpdate()
    {
        var lateUpdateSubscription = Observable.EveryUpdate(UnityFrameProvider.PreLateUpdate, cancellationTokenSource.Token)
            .Subscribe(_ => CurrentState?.StateLateUpdate());
        disposables.Add(lateUpdateSubscription);
    }

    /// <summary>
    /// Cancels all subscriptions to update loops and disposes of the disposables.
    /// </summary>
    protected void StopUpdates()
    {
        cancellationTokenSource.Cancel();
        disposables.Dispose();
    }

    #endregion
}