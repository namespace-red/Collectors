using UnityEngine;

public class PointInBox
{
    private Bounds _bounds;
    
    public PointInBox(Bounds bounds)
    {
        _bounds = bounds;
    }

    public Vector3 GetRandom(bool isYInCenter = true)
    {
        float x = Random.Range(_bounds.min.x, _bounds.max.x);
        float y = isYInCenter ? _bounds.center.y : Random.Range(_bounds.min.y, _bounds.max.y);
        float z = Random.Range(_bounds.min.z, _bounds.max.z);
        
        return new Vector3(x, y, z);
    }
}
