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
    private PutState _putState;
    private FlagTransitionConditions _haveTargetTc;

    private IPosition _waitPosition;
    private IPosition _warehousePosition;
    private IPosition _targetPosition = new PositionPoint();
    private CollectorAnimations _animations;
    private MoverToTarget _moverToTarget;
    private Inventory _inventory = new Inventory();

    public event Action PutPickable;
    
    public bool IsBusy { get; private set; }

    private void OnValidate()
    {
        if (_pickUpPoint == null)
            throw new NullReferenceException(nameof(_pickUpPoint));
    }

    private void Awake()
    {
        _moverToTarget = GetComponent<MoverToTarget>();
        _animations = GetComponent<CollectorAnimations>();
        
        _moverToTarget.IsRotate = true;
        _moverToTarget.enabled = false;
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
        _putState.PutPickable -= OnPutPickableInWarehouse;
    }

    public void Init(IPosition waitPosition, IPosition warehousePosition)
    {
        _waitPosition = waitPosition ?? throw new ArgumentNullException(nameof(waitPosition));
        _warehousePosition = warehousePosition ?? throw new ArgumentNullException(nameof(warehousePosition));

        InitStateMachine();
    }

    public void SetPickableTarget(IPickable target)
    {
        IsBusy = true;
        _targetPosition.Transform = target.Transform;
        _haveTargetTc.SetTrueFlag();
    }
    
    private void InitStateMachine()
    {
        var moverToWaitPositionState = new MoverToTargetState(_animations, _moverToTarget, _waitPosition);
        var moverToWarehouseState = new MoverToTargetState(_animations, _moverToTarget, _warehousePosition);
        var moverToTargetState = new MoverToTargetState(_animations, _moverToTarget, _targetPosition);
        var idleState = new IdleState(_animations);
        var pickUpState = new PickUpState(_animations, _pickUpPoint, _moverToTarget, _inventory);
        _putState = new PutState(_animations, _inventory);

        _putState.PutPickable += OnPutPickableInWarehouse;

        var waitPointNearbyTc = new NearbyTransitionConditions(transform, _waitPosition, _inaccuracyWaitPosition);
        var warehouseNearbyTc = new NearbyTransitionConditions(transform, _warehousePosition, _inaccuracyWarehouse);
        var targetNearbyTc = new NearbyTransitionConditions(transform, _targetPosition, _inaccuracyTarget);
        _haveTargetTc = new FlagTransitionConditions();
        var animatedPickUpTc = new AnimatedTransitionConditions(_animations.Animator, AnimationLayer, PickUpAnimationName);
        var animatedPutTc = new AnimatedTransitionConditions(_animations.Animator, AnimationLayer, PutAnimationName);
        
        _stateMachine = new StateMachine();
        _stateMachine.AddTransition(moverToWaitPositionState, idleState, waitPointNearbyTc);
        _stateMachine.AddTransition(moverToWaitPositionState, moverToTargetState, _haveTargetTc);
        _stateMachine.AddTransition(idleState, moverToTargetState, _haveTargetTc);
        _stateMachine.AddTransition(moverToTargetState, pickUpState, targetNearbyTc);
        _stateMachine.AddTransition(pickUpState, moverToWarehouseState, animatedPickUpTc);
        _stateMachine.AddTransition(moverToWarehouseState, _putState, warehouseNearbyTc);
        _stateMachine.AddTransition(_putState, moverToWaitPositionState, animatedPutTc);
        _stateMachine.SetFirstState(moverToWaitPositionState);
    }

    private void OnPutPickableInWarehouse()
    {
        IsBusy = false;
        PutPickable?.Invoke();
    }
}
