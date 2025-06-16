using System;
using UnityEngine;

[RequireComponent(typeof(CollectorAnimations))]
[RequireComponent(typeof(MoverToTarget))]
public class Collector : MonoBehaviour
{
    private const int AnimationLayer = 0;
    private const string PickUpAnimationName = "PickingUp2";
    private const string PutAnimationName = "Put2";
    
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
    private PutState _putState;
    
    private NearbyTransitionConditions _waitPointNearbyTc;
    private NearbyTransitionConditions _warehouseNearbyTc;
    private NearbyTransitionConditions _targetNearbyTc;
    private FlagTransitionConditions _haveTargetTc;
    private AnimatedTransitionConditions _animatedPickUpTc;
    private AnimatedTransitionConditions _animatedPutTc;

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

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    private void OnDestroy()
    {
        _putState.Finished -= OnPutInWarehouse;
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
        _idleState = new IdleState(_animations);
        _pickUpState = new PickUpState(_animations, _pickUpPoint, MoverToTarget, _inventory);
        _putState = new PutState(_animations, _inventory);

        _putState.Finished += OnPutInWarehouse;
        
        _waitPointNearbyTc = new NearbyTransitionConditions(transform, _waitPosition, _inaccuracyWaitPosition);
        _warehouseNearbyTc = new NearbyTransitionConditions(transform, _warehousePosition, _inaccuracyWarehouse);
        _targetNearbyTc = new NearbyTransitionConditions(transform, _inaccuracyTarget);
        _haveTargetTc = new FlagTransitionConditions();
        _animatedPickUpTc = new AnimatedTransitionConditions(_animations.Animator, AnimationLayer, PickUpAnimationName);
        _animatedPutTc = new AnimatedTransitionConditions(_animations.Animator, AnimationLayer, PutAnimationName);
        
        _stateMachine = new StateMachine();
        _stateMachine.AddTransition(_moverToWaitPositionState, _idleState, _waitPointNearbyTc);
        _stateMachine.AddTransition(_moverToWaitPositionState, _moverToTargetState, _haveTargetTc);
        _stateMachine.AddTransition(_idleState, _moverToTargetState, _haveTargetTc);
        _stateMachine.AddTransition(_moverToTargetState, _pickUpState, _targetNearbyTc);
        _stateMachine.AddTransition(_pickUpState, _moverToWarehouseState, _animatedPickUpTc);
        _stateMachine.AddTransition(_moverToWarehouseState, _putState, _warehouseNearbyTc);
        _stateMachine.AddTransition(_putState, _moverToWaitPositionState, _animatedPutTc);
        _stateMachine.SetFirstState(_moverToWaitPositionState);
    }

    private void OnPutInWarehouse()
    {
        IsBusy = false;
        BroughtPickable?.Invoke();
    }
}
