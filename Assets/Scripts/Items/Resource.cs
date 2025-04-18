using System;
using UnityEngine;

public class Resource : MonoBehaviour, IPickable
{
    public event Action<IPickable> Destroying;
    
    public Transform Transform => transform;

    public void PickUp(Transform parent)
    {
        Transform.SetParent(parent);
        Transform.localPosition = Vector3.zero;
    }

    private void OnDestroy()
    {
        Destroying?.Invoke(this);
    }
}
