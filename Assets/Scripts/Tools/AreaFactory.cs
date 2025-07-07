using UnityEngine;

public class AreaFactory<T> : Factory<T>
    where T : MonoBehaviour
{
    [SerializeField] private Collider _collider;

    private Bounds _bounds;

    protected void Awake()
    {
        _bounds = _collider.bounds;
    }

    public override T Create()
    {
        float x = Random.Range(_bounds.min.x, _bounds.max.x);
        float z = Random.Range(_bounds.min.z, _bounds.max.z);
        float y = _bounds.center.y;
        
        T obj = base.Create();
        obj.transform.position = new Vector3(x, y, z);
        
        return obj;
    }
}
