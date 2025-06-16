public class Transition
{
    private readonly ITransitionCondition _condition;
    
    public Transition(IState nextState, ITransitionCondition condition)
    {
        NextState = nextState;
        _condition = condition;
    }
        
    public IState NextState { get; }
    public bool IsReady => _condition.IsDone();
}
