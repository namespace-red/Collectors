using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class BasePhysicsMover : MonoBehaviour
{
    [SerializeField] protected float Speed;
    
    public bool IsRotate { get; set; }
    
    protected Rigidbody Rigidbody;

    protected abstract Vector3 Direction
    {
        get;
    }

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate()
    {
        if (IsRotate)
            Rotate();
        
        Move();
    }

    private void Rotate()
    {
        transform.forward = Direction;
    }

    private void Move()
    {
        Vector3 step = Direction * (Speed * Time.fixedDeltaTime);
        Rigidbody.MovePosition(transform.position + step);
    }
}
