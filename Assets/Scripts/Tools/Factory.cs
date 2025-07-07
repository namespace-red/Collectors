using System;
using UnityEngine;

public class Factory<T> : MonoBehaviour
    where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private Transform _parent;
    
    protected virtual void OnValidate()
    {
        if (_prefab == null)
            throw new NullReferenceException(nameof(_prefab));
        
        if (_parent == null)
            _parent = transform;
    }

    public virtual T Create()
    {
        return Instantiate(_prefab, transform.position, Quaternion.identity, _parent);
    }
}
