using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Radar))]
public class MainHome : MonoBehaviour
{
    [SerializeField] private CollectorSpawner _collectorSpawner;
    [SerializeField] private int _startCollectorsCount;

    private List<Collector> _collectors = new List<Collector>();
    private List<IPickable> _pickablesInWork = new List<IPickable>();
    private Radar _radar;
    private int _appleCount;

    public event Action<int> ChangedAppleCount;
    
    public int AppleCount
    {
        get => _appleCount;
        private set
        {
            _appleCount = value;
            ChangedAppleCount?.Invoke(_appleCount);
        }
    }

    private void OnValidate()
    {
        if (_collectorSpawner == null)
            throw new NullReferenceException(nameof(_collectorSpawner));
    }

    private void Awake()
    {
        _radar = GetComponent<Radar>();
    }

    private void Start()
    {
        _collectorSpawner.Create(_startCollectorsCount);
    }

    private void OnEnable()
    {
        _radar.DetectedPickable += OnDetectedPickables;
        _collectorSpawner.Created += OnCreatedCollector;
    }

    private void OnDisable()
    {
        _radar.DetectedPickable -= OnDetectedPickables;
        _collectorSpawner.Created -= OnCreatedCollector;
    }

    private void OnCreatedCollector(Collector collector)
    {
        _collectors.Add(collector);
        collector.BroughtPickable += OnCollectorBroughtPickable;
    }

    private void OnDetectedPickables(List<IPickable> pickables)
    {
        foreach (var pickable in pickables)
        {
            if (_pickablesInWork.Contains(pickable))
                break;
            
            var collector = GetFreeCollector();
        
            if (collector == null)
                return;

            _pickablesInWork.Add(pickable);
            pickable.Destroying += OnDestroyingPickable;
            collector.SetPickableTarget(pickable);
        }
    }

    private Collector GetFreeCollector()
    {
        return _collectors.FirstOrDefault(collector => collector.IsBusy == false);
    }

    private void OnDestroyingPickable(IPickable pickable)
    {
        pickable.Destroying -= OnDestroyingPickable;
        _pickablesInWork.Remove(pickable);
    }

    private void OnCollectorBroughtPickable()
    {
        AppleCount++;
    }
}
