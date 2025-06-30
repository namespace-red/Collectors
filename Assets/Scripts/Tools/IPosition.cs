using UnityEngine;

public interface IPosition
{
    Transform Transform { get; set; }
    Vector3 Offset { get; set; }

    Vector3 Get();
}
