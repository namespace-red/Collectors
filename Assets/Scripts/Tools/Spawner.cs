using System;
using UnityEngine;

[RequireComponent(typeof(IPosition))]
public class Spawner<T> : MonoBehaviour
    where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private Transform _parent;
    
    private IPosition _position;

    private void Awake()
    {
        _position = GetComponent<IPosition>();
    }

    protected virtual void OnValidate()
    {
        if (_parent == null)
            _parent = transform;
        
        if (_prefab == null)
            throw new NullReferenceException(nameof(_prefab));
    }

    public virtual T Get()
    {
        return Instantiate(_prefab, _position.Get(), Quaternion.identity, _parent);
    }
}
