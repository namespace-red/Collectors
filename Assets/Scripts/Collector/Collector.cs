using UnityEngine;

[RequireComponent(typeof(MoverToTarget))]
[RequireComponent(typeof(MoverToPoint))]
public class Collector : MonoBehaviour
{
    private StateMachine _stateMachine = new StateMachine();

    public MoverToTarget MoverToTarget
    {
        get;
        private set;
    }

    public MoverToPoint MoverToWaitPoint
    {
        get;
        private set;
    }

    private void Awake()
    {
        MoverToTarget = GetComponent<MoverToTarget>();
        MoverToWaitPoint = GetComponent<MoverToPoint>();
    }

    private void Start()
    {
        InitStateMachine();
    }

    private void InitStateMachine()
    {
        var moverToWaitPointState = new MoverToPointState(MoverToWaitPoint);
        // var moverToTargetState = new MoverToTargetState();
        var idleState = new IdleState();
        // var pickUp = new PickUpState();
        
        var waitPointNearbyTransitionConditions = new NearbyTransitionConditions(transform, MoverToWaitPoint.TargetPoint);
        // var flagTransitionConditions = new FlagTransitionConditions();
        // var animatedTransitionConditions = new AnimatedTransitionConditions();
        
        _stateMachine = new StateMachine();
        _stateMachine.SetFirstState(moverToWaitPointState);
        _stateMachine.AddTransition(moverToWaitPointState, idleState, waitPointNearbyTransitionConditions);
        // _stateMachine.AddTransition(idleState, patrolState, emptyTransitionConditions);
        // _stateMachine.AddTransition(patrolState, targetPursuerState, playerDetectorTransitionConditions);
        // _stateMachine.AddTransitionFromAnyStates(deathState, deathTransitionConditions);
    }
}
