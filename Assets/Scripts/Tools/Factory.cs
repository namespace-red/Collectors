using System;
using UnityEngine;

public class Factory<T> : MonoBehaviour
    where T : MonoBehaviour
{
    [SerializeField] protected T Prefab;
    [SerializeField] private Transform _parent;

    private T _currentPrefab;

    public Transform Parent
    {
        get => _parent;
        set
        {
            if (value == null)
                throw new NullReferenceException(nameof(value));
            
            _parent = value;
        }
    }

    protected T CurrentPrefab
    {
        get => _currentPrefab;
        set
        {
            if (value == null)
                throw new NullReferenceException(nameof(value));

            _currentPrefab = value;
        }
    }
    
    protected virtual void OnValidate()
    {
        if (Prefab == null)
            throw new NullReferenceException(nameof(Prefab));
        
        if (Parent == null)
            throw new NullReferenceException(nameof(Parent));
    }

    protected virtual void Awake()
    {
        if (CurrentPrefab == null)
            CurrentPrefab = Prefab;
    }

    public virtual T Create()
    {
        return Instantiate(CurrentPrefab, transform.position, Quaternion.identity, Parent);
    }
}
