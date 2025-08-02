using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private TeamCollectorHandler _teamCollectorHandler;
    private ResourceWarehouse _resourceWarehouse;
    private List<Collector> _collectors = new List<Collector>();
    
    private StateMachine _stateMachine;
    private CollectorCreaterState _collectorCreaterState;
    private ColonyCreaterState _colonyCreaterState;
    private FlagTransitionConditions _collectorModTc;
    private FlagAndCountMoreTransitionConditions _colonyModTc;

    public Collider WaitArea => _waitArea;
    public Transform WarehouseTransform => _warehouseTransform;
    public Flag Flag => _flag;
    public PickableDetector PickableDetector => _pickableDetector;
    public CollectorFactory CollectorFactory => _collectorFactory;
    public ResourceWarehouse ResourceWarehouse => _resourceWarehouse;
    public bool NeedSendCollectorForPickable { get; set; } = true;

    private void OnValidate()
    {
        if (_flag == null)
            throw new NullReferenceException(nameof(_flag));
        
        if (_pickableDetector == null)
            throw new NullReferenceException(nameof(_pickableDetector));
        
        if (_collectorFactory == null)
            throw new NullReferenceException(nameof(_collectorFactory));
    }
    
    private void Awake()
    {
        _resourceWarehouse = GetComponent<ResourceWarehouse>();
    }

    private void OnEnable()
    {
        _collectorFactory.Created += AddCollector;
        _flag.Placed += OnPlacedFlag;
        _flag.Removed += OnRemovedFlag;
    }

    private void OnDisable()
    {
        _collectorFactory.Created -= AddCollector;
        _flag.Placed -= OnPlacedFlag;
        _flag.Removed -= OnRemovedFlag;
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    public void Init(ColonyFactory colonyFactory, TeamCollectorHandler teamCollectorHandler)
    {
        _teamCollectorHandler = teamCollectorHandler;
        InitStateMachine(colonyFactory);
        _flag.Remove();
    }
    
    public bool HaveFreeCollector()
        => _collectors.Any(collector => collector.IsBusy == false);

    public Collector GetFreeCollector()
        => _collectors.FirstOrDefault(collector => collector.IsBusy == false);

    public void AddCollector(Collector collector)
    {
        _collectors.Add(collector);
        collector.PutPickable += OnCollectorPutPickable;
        collector.SetColony(this);
        
        _teamCollectorHandler.RunRadar();
    }

    public void RemoveCollector(Collector collector)
    {
        _collectors.Remove(collector);
        collector.PutPickable -= OnCollectorPutPickable;
    }

    private void InitStateMachine(ColonyFactory colonyFactory)
    {
        _collectorCreaterState = new CollectorCreaterState(_collectorFactory, _startCollectorCount, _resourceWarehouse, CollectorPrice);
        _colonyCreaterState = new ColonyCreaterState(colonyFactory, this, ColonyPrice);

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

    private void OnCollectorPutPickable(IPickable pickable)
    {
        switch (pickable)
        {
            case Resource resource:
                _resourceWarehouse.Add(resource.Value);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pickable));
        }

        if (NeedSendCollectorForPickable)
        {
            _teamCollectorHandler.RunRadar();
        }
    }
}
