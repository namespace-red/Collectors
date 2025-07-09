using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ResourceWarehouse))]
public class Colony : MonoBehaviour
{
    [SerializeField] private Radar _radar;
    [SerializeField] private CollectorFactory _collectorFactory;
    [SerializeField, Min(0)] private int _startCollectorCount;
    [SerializeField] private Flag _flag;
    
    private ResourceWarehouse _resourceWarehouse;
    private List<Collector> _collectors = new List<Collector>();
    
    private StateMachine _stateMachine;
    private FlagTransitionConditions _collectorModTc;

    public Flag Flag => _flag;

    private void OnValidate()
    {
        if (_collectorFactory == null)
            throw new NullReferenceException(nameof(_collectorFactory));
        
        if (_flag == null)
            throw new NullReferenceException(nameof(_flag));
    }

    private void Awake()
    {
        _resourceWarehouse = GetComponent<ResourceWarehouse>();
    }

    private void OnEnable()
    {
        _collectorFactory.Created += OnCreatedCollector;
        _radar.DetectedPickables += OnDetectedPickable;
    }

    private void OnDisable()
    {
        _collectorFactory.Created -= OnCreatedCollector;
        _radar.DetectedPickables -= OnDetectedPickable;
    }

    private void Start()
    {
        _flag.Remove();
        InitStateMachine();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    private void InitStateMachine()
    {
        var collectorCreaterState = new CollectorCreaterState(_resourceWarehouse, _collectorFactory, _startCollectorCount);
        
        _collectorModTc = new FlagTransitionConditions();
        
        _stateMachine = new StateMachine();
        _stateMachine.SetFirstState(collectorCreaterState);
    }
    
    private void OnCreatedCollector(Collector collector)
    {
        _collectors.Add(collector);
        collector.PutPickable += OnCollectorBroughtPickable;
        RunRadar();
    }

    private void OnCollectorBroughtPickable(IPickable pickable)
    {
        switch (pickable)
        {
            case Resource resource:
                _resourceWarehouse.Add(resource.Value);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pickable));
        }

        RunRadar();
    }

    private void RunRadar()
    {
        _radar.Run();
    }

    private void OnDetectedPickable()
    {
        while (HaveFreeCollector() && _radar.HaveFreePickable)
        {
            Collector collector = GetFreeCollector();
            var pickable = _radar.TakeNearestPickable(collector.transform.position);
            collector.SetPickableTarget(pickable);
        }

        if (HaveFreeCollector() == false)
        {
            _radar.Stop();
        }
    }

    private bool HaveFreeCollector()
        => _collectors.Any(collector => collector.IsBusy == false);
    
    private Collector GetFreeCollector() 
        => _collectors.FirstOrDefault(collector => collector.IsBusy == false);
}
