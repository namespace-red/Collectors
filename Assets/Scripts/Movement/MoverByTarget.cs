using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoverByTarget : MonoBehaviour
{
    [SerializeField] protected float Speed;
    [SerializeField] private Transform _target;

    protected Rigidbody Rigidbody;

    public Transform Target
    {
        get => _target;
        set => _target = value;
    }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_target == null) 
            return;
        
        Rotate();
        Move();
    }

    protected void Rotate()
    {
        transform.forward = (_target.position - transform.position).normalized;
    }

    protected void Move()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        Vector3 step = direction * (Speed * Time.fixedDeltaTime);
        Vector3 position = transform.position + step;

        Rigidbody.MovePosition(position);
    }
}
