using System;
using UnityEngine;

public interface IPickable
{
    public Transform Transform
    {
        get;
    }
    public event Action Picked;

    public void PickUp();
}