using System.Collections.Generic;
using System.Linq;

public class TransitionsController
{
    private readonly Dictionary<IState, List<Transition>> _transitionsMap = new Dictionary<IState, List<Transition>>();
    private readonly List<Transition> _transitionsFromAnyStates = new List<Transition>();
    private List<Transition> _currentTransitions;

    public static Transition CreateTransition(IState nextState, ITransitionCondition transitionCondition)
        => new Transition(nextState, transitionCondition);

    public void AddTransitionByState(IState state, Transition transition)
    {
        if (TryGetTransitionsByState(state, out var transitions) == false)
        {
            CreateConnection(state, out transitions);
        }
        
        transitions.Add(transition);
    }

    public void AddTransitionFromAnyStates(Transition transition)
    {
        _transitionsFromAnyStates.Add(transition);
    }

    public Transition TryGetReadyTransition(IState currentState)
    {
        return _transitionsFromAnyStates.FirstOrDefault(transition => transition.IsReady && transition.NextState != currentState) ?? 
               _currentTransitions?.FirstOrDefault(transition => transition.IsReady);
    }

    public void SetCurrentState(IState currentState)
    {
        TryGetTransitionsByState(currentState, out _currentTransitions);
    }

    private bool TryGetTransitionsByState(IState state, out List<Transition> transitions)
    {
        return _transitionsMap.TryGetValue(state, out transitions);
    }
    
    private void CreateConnection(IState state, out List<Transition> transitions)
    {
        transitions = new List<Transition>();
        _transitionsMap.Add(state, transitions);
    }
}
