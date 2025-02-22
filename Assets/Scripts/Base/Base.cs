using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Radar))]
public class Base : MonoBehaviour
{
    [SerializeField] private CollectorSpawner _collectorSpawner;
    [SerializeField] private int _collectorsCount;

    private List<Collector> _collectors = new List<Collector>();
    private List<Collector> _freeCollectors = new List<Collector>();
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
        
        // Подписаться на радар
    }

    private void OnDestroy()
    {
        foreach (var collector in _collectors)
        {
            // Отписаться от освобождение Collector
        }
        
        // Отписаться от радар
    }

    private void CreateCollector()
    {
        ++_collectorsCount;
        
        var collector = _collectorSpawner.Get();
        _collectors.Add(collector);
        _freeCollectors.Add(collector);
        // Подписаться на освобождение Collector
    }

    private void OnRadar(IPickable picked)
    {
        if (_freeCollectors.Count == 0)
            return;

        var collector = _freeCollectors[0];
        _freeCollectors.Remove(collector);
        
        //указать цель picked collector'у
    }

    private void OnCollectorFreedOut(Collector collector)
    {
        _freeCollectors.Add(collector);
    }
}
