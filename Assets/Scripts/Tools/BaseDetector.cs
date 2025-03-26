using System.Collections.Generic;
using UnityEngine;

public class BaseDetector<T> : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _targetLayer;

    public Vector3 Center => transform.position + transform.rotation * _offset;
    
    public float Radius
    {
        get => _radius;
        private set => _radius = value;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Center, Radius);
    }

    public bool TryDetect(out T target)
    {
        foreach (var collider in FindColliders())
        {
            if (collider.TryGetComponent(out target))
            {
                return true;
            }
        }

        target = default;
        return false;
    }
    
    public IEnumerable<T> DetectAll()
    {
        var targets = new List<T>();

        foreach (var collider in FindColliders())
        {
            if (collider.TryGetComponent(out T target))
            {
                targets.Add(target);
            }
        }

        return targets;
    }

    private IEnumerable<Collider> FindColliders()
    {
        return Physics.OverlapSphere(Center, _radius, _targetLayer);
    }
}
