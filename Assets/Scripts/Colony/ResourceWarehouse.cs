using System;
using UnityEngine;

public class ResourceWarehouse : MonoBehaviour
{
    [SerializeField] private int _count;
    
    public event Action<int> ChangedCount;

    public int Count
    {
        get => _count;
        private set
        {
            _count = value;
            ChangedCount?.Invoke(_count);
        }
    }

    public void Add(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        Count += value;
    }

    public bool IsEnough(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        return Count >= value;
    }

    public void Spend(int value)
    {
        if (value < 0 || IsEnough(value) == false)
            throw new ArgumentOutOfRangeException(nameof(value));
        
        Count -= value;
    }
}
