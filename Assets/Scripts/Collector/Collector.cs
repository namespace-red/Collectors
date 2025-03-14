using UnityEngine;

[RequireComponent(typeof(MoverToTarget))]
[RequireComponent(typeof(MoverToPoint))]
public class Collector : MonoBehaviour
{
    private StateMachine _stateMachine = new StateMachine();

    private NearbyPointTransitionConditions _waitPointNearbyPointTc;
    private FlagTransitionConditions _haveTargetTc;
    private NearbyTransitionConditions _targetNearbyTc;
    
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

    public void SetPickableTarget(IPickable pickable)
    {
        var target = ((MonoBehaviour) pickable).transform;
        MoverToTarget.Target = target;
        _targetNearbyTc.Target = target;

        _haveTargetTc.Flag = true;

        IsBusy = true;
    }
    
    private void InitStateMachine()
    {
        var moverToWaitPointState = new MoverToPointState(MoverToWaitPoint);
        var idleState = new IdleState();
        var idleState2 = new IdleState();
        var moverToTargetState = new MoverToTargetState(MoverToTarget);
        
        _waitPointNearbyPointTc = new NearbyPointTransitionConditions(transform, MoverToWaitPoint.TargetPoint);
        _haveTargetTc = new FlagTransitionConditions();
        _targetNearbyTc = new NearbyTransitionConditions(transform);
        
        _stateMachine = new StateMachine();
        _stateMachine.AddTransition(moverToWaitPointState, idleState, _waitPointNearbyPointTc);
        _stateMachine.AddTransition(moverToWaitPointState, moverToTargetState, _haveTargetTc);
        _stateMachine.AddTransition(idleState, moverToTargetState, _haveTargetTc);
        _stateMachine.AddTransition(moverToTargetState, idleState2, _targetNearbyTc);
        _stateMachine.SetFirstState(moverToWaitPointState);
    }
    
    private void SetRandomWaitPoint()
    {
        MoverToWaitPoint.TargetPoint = _waitArea.Get();
    }
}
