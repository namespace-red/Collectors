using UnityEngine;

public class PlaceChecker
{
    private LayerMask _groundLayer;
    private float _colonyRadius;
    private Camera _camera;
    
    public PlaceChecker(LayerMask groundLayer, float colonyRadius)
    {
        _groundLayer = groundLayer;
        _colonyRadius = colonyRadius;
        _camera = Camera.main;
    }

    public bool TryGetColony(out Colony colony)
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out var hit) == false)
        {
            colony = default;
            return false;
        }

        colony = hit.collider.GetComponentInParent<Colony>();
        return colony != null;
    }

    public bool TryGetPointForColony(out Vector3 point)
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _groundLayer) == false)
        {
            point = default;
            return false;
        }

        point = hit.point;
        return true;
    }

    public bool CanPlaceColony(Vector3 center)
    {
        return Physics.CheckSphere(center, _colonyRadius, ~_groundLayer, QueryTriggerInteraction.Ignore) == false;
    }
}
