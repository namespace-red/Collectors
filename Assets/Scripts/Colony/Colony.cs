using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Colony : MonoBehaviour
{
    [SerializeField] private PickableInWorldController pickableInWorldController;
    [SerializeField] private CollectorSpawner _collectorSpawner;
    [SerializeField, Min(1)] private int _startCollectorsCount;

    private List<Collector> _collectors = new List<Collector>();
    private int _pickableCount;

    public event Action<int> ChangedPickableCount;

    public int PickableCount
    {
        get => _pickableCount;
        private set
        {
            _pickableCount = value;
            ChangedPickableCount?.Invoke(_pickableCount);
        }
    }

    private void OnValidate()
    {
        if (_collectorSpawner == null)
            throw new NullReferenceException(nameof(_collectorSpawner));
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
        collector.BroughtPickable += OnCollectorBroughtPickable;
        collector.BroughtPickable += RunPickableDetector;
        RunPickableDetector();
    }

    private void OnCollectorBroughtPickable()
    {
        PickableCount++;
    }

    private void RunPickableDetector()
    {
        pickableInWorldController.RunDetector();
    }

    void OnDetectedPickable(IEnumerable<IPickable> detectedPickable)
    {
        Collector collector;

        while ((collector = GetFreeCollector()) != null && detectedPickable.Any())
        {
            var pickable = detectedPickable.OrderBy(p => 
                Vector3.Distance(p.Transform.position, collector.transform.position)).First();

            collector.SetPickableTarget(pickable);
            pickableInWorldController.Take(pickable);
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
