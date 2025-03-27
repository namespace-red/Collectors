using System;
using System.Collections;
using UnityEngine;

public class CollectorSpawner : Spawner<Collector>
{
    [SerializeField] private float _secCoolDown;
    [SerializeField] private PositionInArea _waitArea;
    [SerializeField] private PositionPoint _warehousePoint;

    private Coroutine _coroutine;
    private int _sizeOfQueue;

    public event Action<Collector> Created;
    
    protected override void OnValidate()
    {
        base.OnValidate();
        
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

    public void Create(int count)
    {
        _sizeOfQueue += count;
        
        if (_coroutine == null)
        {
            _coroutine = StartCoroutine(StartCreating());
        }
    }

    private IEnumerator StartCreating()
    {
        var wait = new WaitForSeconds(_secCoolDown);

        while (_sizeOfQueue > 0)
        {
            Created?.Invoke(Get());
            yield return wait;
            _sizeOfQueue--;
        }

        _coroutine = null;
    }
}
