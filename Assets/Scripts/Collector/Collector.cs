using System;
using UnityEngine;

[RequireComponent(typeof(CollectorAnimations))]
[RequireComponent(typeof(MoverToTarget))]
public class Collector : MonoBehaviour
{
    [SerializeField] private Transform _pickUpPoint;
    [SerializeField] private float _inaccuracyTarget = 1.1f;
    [SerializeField] private float _inaccuracyWaitPosition = 0.1f;
    [SerializeField] private float _inaccuracyWarehouse = 0.1f;
    
    private StateMachine _stateMachine;
    private MoverToTargetState _moverToWaitPositionState;
    private MoverToTargetState _moverToWarehouseState;
    private MoverToTargetState _moverToTargetState;
    private IdleState _idleState;
    private PickUpState _pickUpState;
    
    private NearbyTransitionConditions _waitPointNearbyTc;
    private NearbyTransitionConditions _warehouseNearbyTc;
    private NearbyTransitionConditions _targetNearbyTc;
    private FlagTransitionConditions _haveTargetTc;
    private FlagTransitionConditions _pickUpTc;
    private AnimatedTransitionConditions _animatedTc;
    
    private IPosition _waitPosition;
    private IPosition _warehousePosition;
    private CollectorAnimations _animations;
    private Inventory _inventory = new Inventory();

    public Action BroughtPickable;

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

    private void OnValidate()
    {
        if (_pickUpPoint == null)
            throw new NullReferenceException(nameof(_pickUpPoint));
    }

    private void Awake()
    {
        MoverToTarget = GetComponent<MoverToTarget>();
        _animations = GetComponent<CollectorAnimations>();
        
        MoverToTarget.IsRotate = true;
        MoverToTarget.enabled = false;
    }

    private void OnEnable()
    {
        _animations.PickUpComplete += OnPickUpAnimationComplete;
    }

    private void OnDisable()
    {
        _animations.PickUpComplete -= OnPickUpAnimationComplete;
    }

    private void OnDestroy()
    {
        _moverToWarehouseState.Finished -= OnCameToWarehouse;
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    public void Init(IPosition waitPosition, IPosition warehousePosition)
    {
        _waitPosition = waitPosition ?? throw new NullReferenceException(nameof(waitPosition));
        _warehousePosition = warehousePosition ?? throw new NullReferenceException(nameof(warehousePosition));

        InitStateMachine();
    }

    public void SetPickableTarget(IPickable target)
    {
        _moverToTargetState.Target = target.Transform;
        _targetNearbyTc.Target = target.Transform;

        _haveTargetTc.Flag = true;
        IsBusy = true;
    }
    
    private void InitStateMachine()
    {
        _moverToWaitPositionState = new MoverToTargetState(_animations, MoverToTarget, _waitPosition);
        _moverToWarehouseState = new MoverToTargetState(_animations, MoverToTarget, _warehousePosition);
        _moverToTargetState = new MoverToTargetState(_animations, MoverToTarget);
        _idleState = new IdleState();
        _pickUpState = new PickUpState(_animations, _pickUpPoint, MoverToTarget, _inventory);

        _moverToWarehouseState.Finished += OnCameToWarehouse;
        
        _waitPointNearbyTc = new NearbyTransitionConditions(transform, _waitPosition, _inaccuracyWaitPosition);
        _warehouseNearbyTc = new NearbyTransitionConditions(transform, _warehousePosition, _inaccuracyWarehouse);
        _targetNearbyTc = new NearbyTransitionConditions(transform, _inaccuracyTarget);
        _haveTargetTc = new FlagTransitionConditions();
        _pickUpTc = new FlagTransitionConditions();
        // _animatedTc = new AnimatedTransitionConditions(_animation);
        
        _stateMachine = new StateMachine();
        _stateMachine.AddTransition(_moverToWaitPositionState, _idleState, _waitPointNearbyTc);
        _stateMachine.AddTransition(_moverToWaitPositionState, _moverToTargetState, _haveTargetTc);
        _stateMachine.AddTransition(_idleState, _moverToTargetState, _haveTargetTc);
        _stateMachine.AddTransition(_moverToTargetState, _pickUpState, _targetNearbyTc);
        _stateMachine.AddTransition(_pickUpState, _moverToWarehouseState, _pickUpTc);
        // _stateMachine.AddTransition(_pickUpState, _moverToWarehouseState, _animatedTc);
        _stateMachine.AddTransition(_moverToWarehouseState, _moverToWaitPositionState, _warehouseNearbyTc);
        _stateMachine.SetFirstState(_moverToWaitPositionState);
    }

    private void OnPickUpAnimationComplete()
    {
        _pickUpTc.Flag = true;
    }

    private void OnCameToWarehouse()
    {
        var pickable = _inventory.Take();
        Destroy(pickable.Transform.gameObject);
        IsBusy = false;
        BroughtPickable?.Invoke();
    }
}
