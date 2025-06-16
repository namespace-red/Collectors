using System;

public class Transition
{
    private readonly ITransitionCondition _condition;
    
    public Transition(IState nextState, ITransitionCondition condition)
    {
        NextState = nextState ?? throw new ArgumentNullException(nameof(nextState));;
        _condition = condition ?? throw new ArgumentNullException(nameof(condition));;
    }
        
    public IState NextState { get; }
    public bool IsReady => _condition.IsDone();
}
