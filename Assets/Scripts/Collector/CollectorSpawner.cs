using System;
using System.Collections;
using UnityEngine;

public class CollectorSpawner : Spawner<Collector>
{
    [SerializeField] private float _secCoolDown = 1f;
    [SerializeField] private Collider _waitArea;
    [SerializeField] private Transform _warehouse;
    
    private IPosition _warehousePosition;
    private Coroutine _coroutine;
    private int _sizeOfQueue;

    public event Action<Collector> Created;
    
    protected override void OnValidate()
    {
        base.OnValidate();
        
        if (_waitArea == null)
            throw new NullReferenceException(nameof(_waitArea));
        
        if (_warehouse == null)
            throw new NullReferenceException(nameof(_warehouse));
    }

    private void Awake()
    {
        _warehousePosition = new PositionPoint(_warehouse);
    }

    public override Collector Get()
    {
        var collector = base.Get();
        
        var waitPosition = new PositionInArea(_waitArea.transform, _waitArea.bounds);
        collector.Init(waitPosition, _warehousePosition);
        
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
