using System;
using UnityEngine;

public class Apple : MonoBehaviour, IPickable
{
    public Transform Transform => transform;
    
    public event Action Picked;
    
    public void PickUp()
    {
        throw new NotImplementedException();
    }
}
