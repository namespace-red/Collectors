using System;
using UnityEngine;

public class ColonyFactory : Factory<Colony>
{
    [SerializeField] private Colony _firstPrefab;
    [SerializeField] private TeamCollectorHandler _teamCollectorHandler;
    [SerializeField] private Transform _collectorParent;

    public event Action<Colony> Created;
    
    protected override void OnValidate()
    {
        base.OnValidate();
        
        if (_firstPrefab == null)
            throw new NullReferenceException(nameof(_firstPrefab));
        
        if (_teamCollectorHandler == null)
            throw new NullReferenceException(nameof(_teamCollectorHandler));
        
        if (_collectorParent == null)
            throw new NullReferenceException(nameof(_collectorParent));
    }

    public Colony Create(Vector3 position)
    {
        var colony = base.Create();
        colony.transform.position = position;
        colony.Init(this, _teamCollectorHandler);
        
        var collectorFactory = colony.GetComponentInChildren<CollectorFactory>();

        if (collectorFactory == null)
            throw new NullReferenceException("Don't have CollectorFactory");

        collectorFactory.Parent = _collectorParent;
        
        Created?.Invoke(colony);
        return colony;
    }

    public Colony CreateFirstColony(Vector3 position)
    {
        CurrentPrefab = _firstPrefab;
        var colony = Create(position);
        CurrentPrefab = Prefab;
        return colony;
    }
}
