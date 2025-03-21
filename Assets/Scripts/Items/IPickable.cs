using System;
using UnityEngine;

public interface IPickable
{
    public event Action<IPickable> Destroying;
    
    public Transform Transform
    {
        get;
    }
    
    public void PickUp(Transform parent);
}