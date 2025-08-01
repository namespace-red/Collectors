using System;
using System.Collections;
using UnityEngine;

public class CollectorFactory : Factory<Collector>
{
    [SerializeField, Min(0.1f)] private float _secCoolDown;
    
    private Coroutine _coroutine;
    private int _sizeOfQueue;

    public event Action<Collector> Created;
    
    public override Collector Create()
    {
        var collector = base.Create();
        
        Created?.Invoke(collector);
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
            Create();
            yield return wait;
            _sizeOfQueue--;
        }

        _coroutine = null;
    }
}
