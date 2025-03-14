using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Radar))]
public class MainHome : MonoBehaviour
{
    [SerializeField] private CollectorSpawner _collectorSpawner;
    [SerializeField] private int _collectorsCount;

    private List<Collector> _collectors = new List<Collector>();
    private Radar _radar;

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
        for (int i = 0; i < _collectorsCount; i++)
        {
            --_collectorsCount;
            CreateCollector();
        }
    }

    private void OnEnable()
    {
        _radar.DetectedPickable += OnDetectedPickables;
    }

    private void OnDisable()
    {
        _radar.DetectedPickable -= OnDetectedPickables;
    }

    private void CreateCollector()
    {
        ++_collectorsCount;
        
        var collector = _collectorSpawner.Get();
        _collectors.Add(collector);
    }

    private void OnDetectedPickables(List<IPickable> pickables)
    {
        foreach (var pickable in pickables)
        {
            var collector = GetFreeCollector();
        
            if (collector == null)
                return;

            collector.SetPickableTarget(pickable);
        }
    }

    private Collector GetFreeCollector()
    {
        return _collectors.FirstOrDefault(collector => collector.IsBusy == false);
    }
}
