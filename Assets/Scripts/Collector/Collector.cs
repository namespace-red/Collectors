using System;
using UnityEngine;

[RequireComponent(typeof(CollectorAnimations))]
[RequireComponent(typeof(MoverToTarget))]
public class Collector : MonoBehaviour
{
    private const int AnimationLayer = 0;
    private readonly int _pickUpAnimationHash = Animator.StringToHash("Base Layer.PickingUp2");
    private readonly int _putAnimationHash = Animator.StringToHash("Base Layer.Put2");

    [SerializeField] private Transform _pickUpPoint;
    [SerializeField] private float _inaccuracyTarget = 1.1f;
    [SerializeField] private float _inaccuracyFlag = 3f;
    [SerializeField] private float _inaccuracyWaitPosition = 0.1f;
    [SerializeField] private float _inaccuracyWarehouse = 0.1f;
    
    private StateMachine _stateMachine;
    private MoverToTargetState _moverToFlag;
    private PutState _putState;
    private FlagTransitionConditions _haveTargetTc;
    private FlagTransitionConditions _needGoToFlagTc;

    private PositionInBox _waitPosition = new PositionInBox();
    private PositionPoint _warehousePosition = new PositionPoint();
    private PositionPoint _flagPosition = new PositionPoint();
    private PositionPoint _targetPosition = new PositionPoint();
    private CollectorAnimations _animations;
    private MoverToTarget _moverToTarget;
    private Inventory _inventory = new Inventory();

    public event Action<Collector> GotToFlag;
    public event Action<IPickable> PutPickable;
    
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
        _moverToFlag.Finished -= OnGotToFlag;
        _putState.PutPickable -= OnPutPickableInWarehouse;
    }

    public void SetColony(Colony colony)
    {
        _waitPosition.Transform = colony.WaitArea.transform;
        _waitPosition.SetRandomOffset(colony.WaitArea.bounds);
        _warehousePosition.Transform = colony.WarehouseTransform;
        _flagPosition.Transform = colony.Flag.transform;
        
        if (_stateMachine == null)
            InitStateMachine();
    }
    
    public void SetPickableTarget(IPickable target)
    {
        IsBusy = true;
        _targetPosition.Transform = target.Transform;
        _haveTargetTc.SetTrueFlag();
    }

    public bool TryGoToFlag()
    {
        if (IsBusy)
            return false;
        
        IsBusy = true;
        _needGoToFlagTc.SetTrueFlag();
        return true;
    }
    
    private void InitStateMachine()
    {
        var moverToWaitPositionState = new MoverToTargetState(_animations, _moverToTarget, _waitPosition);
        var moverToWarehouseState = new MoverToTargetState(_animations, _moverToTarget, _warehousePosition);
        var moverToTargetState = new MoverToTargetState(_animations, _moverToTarget, _targetPosition);
        _moverToFlag = new MoverToTargetState(_animations, _moverToTarget, _flagPosition);
        var idleState = new IdleState(_animations);
        var pickUpState = new PickUpState(_animations, _pickUpPoint, _moverToTarget, _inventory);
        _putState = new PutState(_animations, _inventory);

        _moverToFlag.Finished += OnGotToFlag;
        _putState.PutPickable += OnPutPickableInWarehouse;

        var waitPointNearbyTc = new NearbyTransitionConditions(transform, _waitPosition, _inaccuracyWaitPosition);
        var warehouseNearbyTc = new NearbyTransitionConditions(transform, _warehousePosition, _inaccuracyWarehouse);
        var targetNearbyTc = new NearbyTransitionConditions(transform, _targetPosition, _inaccuracyTarget);
        var flagNearbyTc = new NearbyTransitionConditions(transform, _flagPosition, _inaccuracyFlag);
        _haveTargetTc = new FlagTransitionConditions();
        _needGoToFlagTc = new FlagTransitionConditions();
        var animatedPickUpTc = new AnimatedTransitionConditions(_animations.Animator, AnimationLayer, _pickUpAnimationHash);
        var animatedPutTc = new AnimatedTransitionConditions(_animations.Animator, AnimationLayer, _putAnimationHash);
        
        _stateMachine = new StateMachine();
        _stateMachine.AddTransition(moverToWaitPositionState, idleState, waitPointNearbyTc);
        _stateMachine.AddTransition(moverToWaitPositionState, moverToTargetState, _haveTargetTc);
        _stateMachine.AddTransition(moverToWaitPositionState, _moverToFlag, _needGoToFlagTc);
        _stateMachine.AddTransition(_moverToFlag, moverToWaitPositionState, flagNearbyTc);
        _stateMachine.AddTransition(idleState, moverToTargetState, _haveTargetTc);
        _stateMachine.AddTransition(idleState, _moverToFlag, _needGoToFlagTc);
        _stateMachine.AddTransition(moverToTargetState, pickUpState, targetNearbyTc);
        _stateMachine.AddTransition(pickUpState, moverToWarehouseState, animatedPickUpTc);
        _stateMachine.AddTransition(moverToWarehouseState, _putState, warehouseNearbyTc);
        _stateMachine.AddTransition(_putState, moverToWaitPositionState, animatedPutTc);
        _stateMachine.SetFirstState(moverToWaitPositionState);
    }

    private void OnGotToFlag()
    {
        IsBusy = false;
        GotToFlag?.Invoke(this);
    }

    private void OnPutPickableInWarehouse(IPickable pickable)
    {
        IsBusy = false;
        PutPickable?.Invoke(pickable);
    }
}
