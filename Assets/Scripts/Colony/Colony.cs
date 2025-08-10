using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ColonyStateMachine;

[RequireComponent(typeof(ResourceWarehouse))]
public class Colony : MonoBehaviour
{
    private const int MinCollectors = 1;
    private const int CollectorPrice = 3;
    private const int ColonyPrice = 5;
    
    [SerializeField] private Collider _waitArea;
    [SerializeField] private Transform _warehouseTransform;
    [SerializeField] private Flag _flag;
    [SerializeField] private PickableDetector _pickableDetector;
    [SerializeField] private CollectorFactory _collectorFactory;
    [SerializeField, Min(0)] private int _startCollectorCount;

    private TeamPickableHandler _teamPickableHandler;
    private List<Collector> _collectors = new List<Collector>();
    
    private StateMachine _stateMachine;
    private CollectorCreaterState _collectorCreaterState;
    private ColonyCreaterState _colonyCreaterState;
    private FlagTransitionConditions _collectorModTc;
    private FlagAndCountMoreTransitionConditions _colonyModTc;

    public Collider WaitArea => _waitArea;
    public Transform WarehouseTransform => _warehouseTransform;
    public Flag Flag => _flag;
    public CollectorFactory CollectorFactory => _collectorFactory;
    public ResourceWarehouse ResourceWarehouse { get; private set; }
    public bool NeedSendCollectorForPickable { get; set; } = true;

    private void OnValidate()
    {
        if (_waitArea == null)
            throw new NullReferenceException(nameof(_waitArea));
        
        if (_warehouseTransform == null)
            throw new NullReferenceException(nameof(_warehouseTransform));
        
        if (_flag == null)
            throw new NullReferenceException(nameof(_flag));
        
        if (_pickableDetector == null)
            throw new NullReferenceException(nameof(_pickableDetector));
        
        if (_collectorFactory == null)
            throw new NullReferenceException(nameof(_collectorFactory));
    }
    
    private void Awake()
    {
        ResourceWarehouse = GetComponent<ResourceWarehouse>();
    }

    private void OnEnable()
    {
        _collectorFactory.Created += AddCollector;
        _flag.Placed += OnPlacedFlag;
        _flag.Removed += OnRemovedFlag;
        _pickableDetector.DetectedPickables += AddPickablesToHandler;
        
        if (_teamPickableHandler != null)
            _teamPickableHandler.AddedPickables += TrySendCollectorsForPickable;
    }

    private void OnDisable()
    {
        _collectorFactory.Created -= AddCollector;
        _flag.Placed -= OnPlacedFlag;
        _flag.Removed -= OnRemovedFlag;
        _pickableDetector.DetectedPickables -= AddPickablesToHandler;
        _teamPickableHandler.AddedPickables -= TrySendCollectorsForPickable;
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    public void Init(ColonyFactory colonyFactory, TeamPickableHandler teamPickableHandler)
    {
        _teamPickableHandler = teamPickableHandler;
        _teamPickableHandler.AddedPickables += TrySendCollectorsForPickable;
        InitStateMachine(colonyFactory);
        _flag.Remove();
    }

    public bool HaveFreeCollector()
        => _collectors.Any(collector => collector.IsFree);

    public bool HaveFreeCollectorForPickable()
        => NeedSendCollectorForPickable && HaveFreeCollector();

    public Collector GetFreeCollector()
        => _collectors.FirstOrDefault(collector => collector.IsFree);

    public void AddCollector(Collector collector)
    {
        _collectors.Add(collector);
        collector.WasFreed += OnCollectorWasFreed;
        collector.PutPickable += OnCollectorPutPickable;
        collector.SetColony(this);
        
        _pickableDetector.Run();
    }

    public void RemoveCollector(Collector collector)
    {
        _collectors.Remove(collector);
        collector.WasFreed -= OnCollectorWasFreed;
        collector.PutPickable -= OnCollectorPutPickable;
    }

    private void InitStateMachine(ColonyFactory colonyFactory)
    {
        _collectorCreaterState = new CollectorCreaterState(this, _startCollectorCount,  CollectorPrice);
        _colonyCreaterState = new ColonyCreaterState(this, colonyFactory, ColonyPrice);

        _collectorModTc = new FlagTransitionConditions();
        _colonyModTc = new FlagAndCountMoreTransitionConditions(_collectors, MinCollectors);
        
        _stateMachine = new StateMachine();
        _stateMachine.AddTransition(_collectorCreaterState, _colonyCreaterState, _colonyModTc);
        _stateMachine.AddTransition(_colonyCreaterState, _collectorCreaterState, _collectorModTc);
        _stateMachine.SetFirstState(_collectorCreaterState);
    }

    private void OnPlacedFlag()
    {
        if (_stateMachine.IsCurrentState(_collectorCreaterState))
            _colonyModTc.SetTrueFlag();
    }

    private void OnRemovedFlag()
    {
        if (_stateMachine.IsCurrentState(_colonyCreaterState))
            _collectorModTc.SetTrueFlag();
    }

    private void AddPickablesToHandler(ICollection<IPickable> pickables)
    {
        _teamPickableHandler.AddPickables(pickables);
    }

    private void TrySendCollectorsForPickable()
    {
        while (_teamPickableHandler.HaveFreePickable && HaveFreeCollectorForPickable())
        {
            Collector collector = GetFreeCollector();
            var pickable = _teamPickableHandler.TakeNearestPickable(collector.transform.position);
            collector.GoToPickable(pickable);
        }
        
        if (HaveFreeCollectorForPickable() == false)
        {
            _pickableDetector.Stop();
        }
    }

    private void OnCollectorWasFreed(Collector _)
    {
        if (NeedSendCollectorForPickable)
            TrySendCollectorsForPickable();
    }

    private void OnCollectorPutPickable(IPickable pickable)
    {
        switch (pickable)
        {
            case Resource resource:
                ResourceWarehouse.Add(resource.Value);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pickable));
        }

        _pickableDetector.Run();
    }
}
