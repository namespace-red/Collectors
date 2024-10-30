using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoverByTarget : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _target;

    private Rigidbody _rigidbody;

    public Transform Target
    {
        get => _target;
        set
        {
            if (value == null)
                throw new NullReferenceException(nameof(value));
            
            _target = value;
        }
    }

    private Vector3 Direction => (_target.position - transform.position).normalized;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Rotate();
        Move();
    }

    private void Rotate()
    {
        transform.forward = Direction;
    }

    private void Move()
    {
        Vector3 step = Direction * (_speed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(transform.position + step);
    }
}
