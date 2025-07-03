using System;
using UnityEngine;

public class Resource : MonoBehaviour, IPickable
{
    public event Action<IPickable> PutPickable;
    
    public Transform Transform => transform;
    public int Value => 1; 

    public void PickUp(Transform parent)
    {
        Transform.SetParent(parent);
        Transform.localPosition = Vector3.zero;
    }

    public void Put()
    {
        gameObject.SetActive(false);
        PutPickable?.Invoke(this);
    }
}
