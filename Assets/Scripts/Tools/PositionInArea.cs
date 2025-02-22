using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PositionInArea : MonoBehaviour, IPosition
{
    private Collider _spawnZone;

    private void Awake()
    {
        _spawnZone = GetComponent<Collider>();
    }

    public Vector3 Get()
    {
        var bounds = _spawnZone.bounds;
        float x = Random.Range(bounds.min.x + 1, bounds.max.x - 1);
        float z = Random.Range(bounds.min.z + 1, bounds.max.z - 1);
        float y = bounds.center.y;

        return new Vector3(x, y, z);
    }
}
