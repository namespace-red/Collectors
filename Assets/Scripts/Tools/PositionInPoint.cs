using UnityEngine;

public class PositionInPoint : MonoBehaviour, IPosition
{
    public Vector3 Get()
    {
        return transform.position;
    }
}
