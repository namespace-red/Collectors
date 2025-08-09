using System;
using UnityEngine;

public class ColonyFactory : Factory<Colony>
{
    [SerializeField] private Colony _firstPrefab;
    [SerializeField] private TeamPickableHandler _teamPickableHandler;
    [SerializeField] private Transform _collectorParent;

    public event Action<Colony> Created;
    
    protected override void OnValidate()
    {
        base.OnValidate();
        
        if (_firstPrefab == null)
            throw new NullReferenceException(nameof(_firstPrefab));
        
        if (_teamPickableHandler == null)
            throw new NullReferenceException(nameof(_teamPickableHandler));
        
        if (_collectorParent == null)
            throw new NullReferenceException(nameof(_collectorParent));
    }

    public Colony Create(Vector3 position)
    {
        var colony = base.Create();
        colony.transform.position = position;
        colony.CollectorFactory.Parent = _collectorParent;
        colony.Init(this, _teamPickableHandler);
        
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
