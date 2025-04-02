using System;
using UnityEngine;

[RequireComponent(typeof(CollectorAnimations))]
[RequireComponent(typeof(MoverToTarget))]
public class Collector : MonoBehaviour
{
    [SerializeField] private Transform _pickUpPoint;
    
    private StateMachine _stateMachine;
    private MoverToTargetState _moverToWaitPointState;
    private MoverToTargetState _moverToWarehouseState;
    private MoverToTargetState _moverToTargetState;
    private IdleState _idleState;
    private PickUpState _pickUpState;
    
    private NearbyTransitionConditions _waitPointNearbyTc;
    private NearbyTransitionConditions _warehouseNearbyTc;
    private NearbyTransitionConditions _targetNearbyTc;
    private FlagTransitionConditions _haveTargetTc;
    private FlagTransitionConditions _pickUpTc;
    
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
        _moverToWaitPointState = new MoverToTargetState(MoverToTarget, _waitPosition);
        _moverToWarehouseState = new MoverToTargetState(MoverToTarget, _warehousePosition);
        _moverToTargetState = new MoverToTargetState(MoverToTarget);
        _idleState = new IdleState();
        _pickUpState = new PickUpState(_animations, _pickUpPoint, MoverToTarget, _inventory);

        _moverToWarehouseState.Finished += OnCameToWarehouse;
        
        _waitPointNearbyTc = new NearbyTransitionConditions(transform, _waitPosition, 0.1f);
        _warehouseNearbyTc = new NearbyTransitionConditions(transform, _warehousePosition, 0.5f);
        _targetNearbyTc = new NearbyTransitionConditions(transform, 1.3f);
        _haveTargetTc = new FlagTransitionConditions();
        _pickUpTc = new FlagTransitionConditions();
        
        _stateMachine = new StateMachine();
        _stateMachine.AddTransition(_moverToWaitPointState, _idleState, _waitPointNearbyTc);
        _stateMachine.AddTransition(_moverToWaitPointState, _moverToTargetState, _haveTargetTc);
        _stateMachine.AddTransition(_idleState, _moverToTargetState, _haveTargetTc);
        _stateMachine.AddTransition(_moverToTargetState, _pickUpState, _targetNearbyTc);
        _stateMachine.AddTransition(_pickUpState, _moverToWarehouseState, _pickUpTc);
        _stateMachine.AddTransition(_moverToWarehouseState, _moverToWaitPointState, _warehouseNearbyTc);
        _stateMachine.SetFirstState(_moverToWaitPointState);
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
