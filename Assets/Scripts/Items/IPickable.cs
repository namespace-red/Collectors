using System;
using UnityEngine;

public interface IPickable
{
    public event Action<IPickable> PutPickable;
    
    public Transform Transform
    {
        get;
    }
    
    public void PickUp(Transform parent);
    public void Put();
}