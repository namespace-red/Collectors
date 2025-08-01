using System;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public event Action Placed;
    public event Action Removed;
    
    public void Place(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        Placed?.Invoke();
    }

    public void Remove()
    {
        gameObject.SetActive(false);
        Removed?.Invoke();
    }
}
