using UnityEngine;

[RequireComponent(typeof(MoverToTarget))]
[RequireComponent(typeof(MoverToPoint))]
public class Collector : MonoBehaviour
{
    private StateMachine _stateMachine = new StateMachine();

    private NearbyPointTransitionConditions _waitPointNearbyPointTc;
    private FlagTransitionConditions _haveTargetTc;
    // var animatedTransitionConditions = new AnimatedTransitionConditions();
    
    private IPosition _waitArea;
    private IPosition _warehousePoint;

    public bool IsBusy
    {
        get;
        private set;
    }
    
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

    private void Update()
    {
        _stateMachine.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    public void Init(IPosition waitArea, IPosition warehousePoint)
    {
        _waitArea = waitArea;
        _warehousePoint = warehousePoint;

        SetRandomWaitPoint();

        InitStateMachine();
    }

    private void InitStateMachine()
    {
        var moverToWaitPointState = new MoverToPointState(MoverToWaitPoint);
        // var moverToTargetState = new MoverToTargetState();
        var idleState = new IdleState();
        // var pickUp = new PickUpState();
        
        _waitPointNearbyPointTc = new NearbyPointTransitionConditions(transform, MoverToWaitPoint.TargetPoint);
        _haveTargetTc = new FlagTransitionConditions();
        // animatedTransitionConditions = new AnimatedTransitionConditions();
        
        _stateMachine = new StateMachine();
        _stateMachine.AddTransition(moverToWaitPointState, idleState, _waitPointNearbyPointTc);
        // _stateMachine.AddTransition(idleState, patrolState, emptyTransitionConditions);
        // _stateMachine.AddTransition(patrolState, targetPursuerState, playerDetectorTransitionConditions);
        // _stateMachine.AddTransitionFromAnyStates(deathState, deathTransitionConditions);
        _stateMachine.SetFirstState(moverToWaitPointState);
    }
    
    private void SetRandomWaitPoint()
    {
        MoverToWaitPoint.TargetPoint = _waitArea.Get();
    }
}
