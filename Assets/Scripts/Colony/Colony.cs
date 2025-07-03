using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ResourceWarehouse))]
public class Colony : MonoBehaviour
{
    [SerializeField] private PickableInWorldController pickableInWorldController;
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
        pickableInWorldController.DetectedPickables += OnDetectedPickable;
    }

    private void OnDisable()
    {
        _collectorSpawner.Created -= OnCreatedCollector;
        pickableInWorldController.DetectedPickables -= OnDetectedPickable;
    }

    private void Start()
    {
        _collectorSpawner.Create(_startCollectorsCount);
    }

    private void OnCreatedCollector(Collector collector)
    {
        _collectors.Add(collector);
        collector.PutPickable += OnCollectorBroughtPickable;
        RunPickableDetector();
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

        RunPickableDetector();
    }

    private void RunPickableDetector()
    {
        pickableInWorldController.RunDetector();
    }

    private void OnDetectedPickable(IEnumerable<IPickable> detectedPickable)
    {
        Collector collector = GetFreeCollector();

        while (collector != null && detectedPickable.Any())
        {
            var pickable = detectedPickable.
                OrderBy(p => p.Transform.position.SqrDistance(collector.transform.position)).
                First();

            collector.SetPickableTarget(pickable);
            pickableInWorldController.Take(pickable);
            
            collector = GetFreeCollector();
        }

        if (collector == null)
        {
            pickableInWorldController.StopDetector();
        }
    }

    private Collector GetFreeCollector()
    {
        return _collectors.FirstOrDefault(collector => collector.IsBusy == false);
    }
}
