using UnityEngine;

public interface IPosition
{
    Transform Transform
    {
        get;
    }

    Vector3 Offset
    {
        get;
    }

    Vector3 Get();
}
