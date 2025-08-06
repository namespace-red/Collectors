using System;
using UnityEngine;
using CollectorStateMachine;

[RequireComponent(typeof(CollectorAnimations))]
[RequireComponent(typeof(MoverToTarget))]
public class Collector : MonoBehaviour
{
    private const int AnimationLayer = 0;
    private readonly int _pickUpAnimationHash = Animator.StringToHash("Base Layer.PickingUp2");
    private readonly int _putAnimationHash = Animator.StringToHash("Base Layer.Put2");

    [SerializeField] private Transform _pickUpPoint;
    [SerializeField, Min(0)] private float _inaccuracyPickable = 1.1f;
    [SerializeField, Min(0)] private float _inaccuracyFlag = 3f;
    [SerializeField, Min(0)] private float _inaccuracyWaitPosition = 0.1f;
    [SerializeField, Min(0)] private float _inaccuracyWarehouse = 0.1f;
    
    private StateMachine _stateMachine;
    private FlagTransitionConditions _havePickableTc;
    private FlagTransitionConditions _needGoToFlagTc;

    private PositionInBox _waitPosition = new PositionInBox();
    private PositionPoint _warehousePosition = new PositionPoint();
    private PositionPoint _flagPosition = new PositionPoint();
    private PositionPoint _pickablePosition = new PositionPoint();

    public event Action<Collector> GotToFlag;
    public event Action<IPickable> PutPickable;
    
    public bool IsBusy { get; private set; }
    public MoverToTarget MoverToTarget { get; private set; }
    public CollectorAnimations Animations { get; private set; }
    public Inventory Inventory { get; } = new Inventory();
    public IPosition PickablePosition => _pickablePosition;

    private void OnValidate()
    {
        if (_pickUpPoint == null)
            throw new NullReferenceException(nameof(_pickUpPoint));
    }

    private void Awake()
    {
        MoverToTarget = GetComponent<MoverToTarget>();
        Animations = GetComponent<CollectorAnimations>();
        
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

    public void SetColony(Colony colony)
    {
        _waitPosition.Transform = colony.WaitArea.transform;
        _waitPosition.SetRandomOffset(colony.WaitArea.bounds);
        _warehousePosition.Transform = colony.WarehouseTransform;
        _flagPosition.Transform = colony.Flag.transform;
        
        if (_stateMachine == null)
            InitStateMachine();
    }
    
    public void GoToPickable(IPickable target)
    {
        IsBusy = true;
        _pickablePosition.Transform = target.Transform;
        _havePickableTc.SetTrueFlag();
    }

    public void CompletePutPickable(IPickable pickable)
    {
        IsBusy = false;
        _pickablePosition.Transform = null;
        PutPickable?.Invoke(pickable);
    }

    public void GoToFlag()
    {
        IsBusy = true;
        _needGoToFlagTc.SetTrueFlag();
    }
    
    public void CompleteGoToFlag()
    {
        IsBusy = false;
        GotToFlag?.Invoke(this);
    }

    private void InitStateMachine()
    {
        var moverToWaitPositionState = new MoverToTargetState(this, _waitPosition);
        var moverToWarehouseState = new MoverToTargetState(this, _warehousePosition);
        var moverToTargetState = new MoverToTargetState(this, _pickablePosition);
        var moverToFlag = new MoverToFlagState(this, _flagPosition);
        var idleState = new IdleState(Animations);
        var pickUpState = new PickUpState(this, _pickUpPoint);
        var putState = new PutState(this);

        var waitPointNearbyTc = new NearbyTransitionConditions(transform, _waitPosition, _inaccuracyWaitPosition);
        var warehouseNearbyTc = new NearbyTransitionConditions(transform, _warehousePosition, _inaccuracyWarehouse);
        var targetNearbyTc = new NearbyTransitionConditions(transform, _pickablePosition, _inaccuracyPickable);
        var flagNearbyTc = new NearbyTransitionConditions(transform, _flagPosition, _inaccuracyFlag);
        _havePickableTc = new FlagTransitionConditions();
        _needGoToFlagTc = new FlagTransitionConditions();
        var animatedPickUpTc = new AnimatedTransitionConditions(Animations.Animator, AnimationLayer, _pickUpAnimationHash);
        var animatedPutTc = new AnimatedTransitionConditions(Animations.Animator, AnimationLayer, _putAnimationHash);
        
        _stateMachine = new StateMachine();
        _stateMachine.AddTransition(moverToWaitPositionState, idleState, waitPointNearbyTc);
        _stateMachine.AddTransition(moverToWaitPositionState, moverToTargetState, _havePickableTc);
        _stateMachine.AddTransition(moverToWaitPositionState, moverToFlag, _needGoToFlagTc);
        _stateMachine.AddTransition(moverToFlag, moverToWaitPositionState, flagNearbyTc);
        _stateMachine.AddTransition(idleState, moverToTargetState, _havePickableTc);
        _stateMachine.AddTransition(idleState, moverToFlag, _needGoToFlagTc);
        _stateMachine.AddTransition(moverToTargetState, pickUpState, targetNearbyTc);
        _stateMachine.AddTransition(pickUpState, moverToWarehouseState, animatedPickUpTc);
        _stateMachine.AddTransition(moverToWarehouseState, putState, warehouseNearbyTc);
        _stateMachine.AddTransition(putState, moverToWaitPositionState, animatedPutTc);
        _stateMachine.SetFirstState(moverToWaitPositionState);
    }
}
