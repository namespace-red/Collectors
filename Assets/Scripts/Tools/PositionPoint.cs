using UnityEngine;

public class PositionPoint : MonoBehaviour, IPosition
{
    [SerializeField] private Vector3 _offset;
    
    public Transform Transform => transform;
    public Vector3 Offset => _offset;

    public Vector3 Get()
    {
        return transform.position + transform.rotation * _offset;
    }
}
