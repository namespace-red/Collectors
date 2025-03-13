using System;
using UnityEngine;

public class CollectorSpawner : Spawner<Collector>
{
    [SerializeField] private PositionInArea _waitArea;
    [SerializeField] private PositionInPoint _warehousePoint;

    private void OnValidate()
    {
        if (_waitArea == null)
            throw new NullReferenceException(nameof(_waitArea));
        
        if (_warehousePoint == null)
            throw new NullReferenceException(nameof(_warehousePoint));
    }
    
    public override Collector Get()
    {
        var collector = base.Get();
        collector.Init(_waitArea, _warehousePoint);
        return collector;
    }
}
