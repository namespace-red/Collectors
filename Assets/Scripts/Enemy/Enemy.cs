using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody _rigidbody;
    private Vector3 _direction;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        if (_direction == Vector3.zero)
            return;
        
        Vector3 position = transform.position + _direction * _speed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(position);
    }
    
    public void Move(Vector3 direction)
    {
        _direction = direction;
    }
}
