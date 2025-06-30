using UnityEngine;

public class StateMachine
{
    private readonly TransitionsController _transitions = new TransitionsController();
    private IState _currentState;
    
    public void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }

    public void Update()
    {
        var transition = _transitions.TryGetReadyTransition(_currentState);
        
        if (transition != null)
            SetState(transition.NextState);
        
        _currentState.Update();
    }

    public void SetFirstState(IState state)
    {
        SetState(state);
    }

    public void AddTransition(IState fromState, IState toState, ITransitionCondition transitionCondition)
    {
        var transition = TransitionsController.CreateTransition(toState, transitionCondition);
        _transitions.AddTransitionByState(fromState, transition);
    }

    public void AddTransitionFromAnyStates(IState toState, ITransitionCondition transitionCondition)
    {
        var transition = TransitionsController.CreateTransition(toState, transitionCondition);
        _transitions.AddTransitionFromAnyStates(transition);
    }

    private void SetState(IState newState)
    {
        _currentState?.Exit();

        Debug.Log("State " + newState);
        _currentState = newState;
        _transitions.SetCurrentState(_currentState);
        _currentState.Enter();
    }
}
