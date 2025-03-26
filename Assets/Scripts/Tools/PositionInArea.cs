using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PositionInArea : MonoBehaviour, IPosition
{
    private Collider _collider;

    public Transform Transform => transform;

    public Vector3 Offset
    {
        get
        {
            var bounds = _collider.bounds;
            float x = Random.Range(bounds.min.x, bounds.max.x + 1);
            float z = Random.Range(bounds.min.z, bounds.max.z + 1);
            float y = bounds.center.y;

            return new Vector3(x, y, z) - transform.position;
        }
    }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public Vector3 Get()
    {
        return transform.position + transform.rotation * Offset;
    }
}
