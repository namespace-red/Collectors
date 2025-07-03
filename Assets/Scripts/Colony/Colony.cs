using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ResourceWarehouse))]
public class Colony : MonoBehaviour
{
    [SerializeField] private Radar _radar;
    [SerializeField] private CollectorSpawner _collectorSpawner;
    [SerializeField, Min(1)] private int _startCollectorsCount;

    private List<Collector> _collectors = new List<Collector>();

    public ResourceWarehouse ResourceWarehouse { get; private set; }

    private void OnValidate()
    {
        if (_collectorSpawner == null)
            throw new NullReferenceException(nameof(_collectorSpawner));
    }

    private void Awake()
    {
        ResourceWarehouse = GetComponent<ResourceWarehouse>();
    }

    private void OnEnable()
    {
        _collectorSpawner.Created += OnCreatedCollector;
        _radar.DetectedPickables += OnDetectedPickable;
    }

    private void OnDisable()
    {
        _collectorSpawner.Created -= OnCreatedCollector;
        _radar.DetectedPickables -= OnDetectedPickable;
    }

    private void Start()
    {
        _collectorSpawner.Create(_startCollectorsCount);
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
                ResourceWarehouse.Add(resource.Value);
                break;
            default:
                throw new NotSupportedException(nameof(pickable));
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
