using System;
using UnityEngine;

[RequireComponent(typeof(CollectorAnimations))]
[RequireComponent(typeof(MoverToTarget))]
[RequireComponent(typeof(MoverToPoint))]
public class Collector : MonoBehaviour
{
    [SerializeField] private Transform _pickUpPoint;
    
    private StateMachine _stateMachine = new StateMachine();

    private MoverToPointState _moverToWaitPointState;
    private MoverToPointState _moverToWarehousePointState;
    private IdleState _idleState;
    private MoverToTargetState _moverToTargetState;
    private PickUpState _pickUpState;
    
    private NearbyPointTransitionConditions _waitPointNearbyPointTc;
    private FlagTransitionConditions _haveTargetTc;
    private FlagTransitionConditions _pickUpTc;
    private NearbyTransitionConditions _targetNearbyTc;
    
    private IPosition _waitArea;
    private IPosition _warehousePoint;
    private CollectorAnimations _animations;

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

    private void OnValidate()
    {
        if (_pickUpPoint == null)
            throw new NullReferenceException(nameof(_pickUpPoint));
    }

    private void Awake()
    {
        MoverToTarget = GetComponent<MoverToTarget>();
        MoverToWaitPoint = GetComponent<MoverToPoint>();
        _animations = GetComponent<CollectorAnimations>();
    }

    private void OnEnable()
    {
        _animations.PickUpComplete += OnPickUpAnimationComplete;
    }

    private void OnDisable()
    {
        _animations.PickUpComplete -= OnPickUpAnimationComplete;
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
        _moverToWaitPointState = new MoverToPointState(MoverToWaitPoint, _waitArea);
        _moverToWarehousePointState = new MoverToPointState(MoverToWaitPoint, _warehousePoint);
        _idleState = new IdleState();
        _moverToTargetState = new MoverToTargetState(MoverToTarget);
        _pickUpState = new PickUpState(_animations, _pickUpPoint, MoverToTarget);
        
        _waitPointNearbyPointTc = new NearbyPointTransitionConditions(transform, MoverToWaitPoint.TargetPoint);
        _haveTargetTc = new FlagTransitionConditions();
        _pickUpTc = new FlagTransitionConditions();
        _targetNearbyTc = new NearbyTransitionConditions(transform);
        
        _stateMachine = new StateMachine();
        _stateMachine.AddTransition(_moverToWaitPointState, _idleState, _waitPointNearbyPointTc);
        _stateMachine.AddTransition(_moverToWaitPointState, _moverToTargetState, _haveTargetTc);
        _stateMachine.AddTransition(_idleState, _moverToTargetState, _haveTargetTc);
        _stateMachine.AddTransition(_moverToTargetState, _pickUpState, _targetNearbyTc);
        _stateMachine.AddTransition(_pickUpState, _moverToWarehousePointState, _pickUpTc);
        _stateMachine.SetFirstState(_moverToWaitPointState);
    }

    private void OnPickUpAnimationComplete()
    {
        _pickUpTc.Flag = true;
    }
}
