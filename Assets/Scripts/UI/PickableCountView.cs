using System;
using TMPro;
using UnityEngine;

public class PickableCountView : MonoBehaviour
{
    [SerializeField] private ResourceWarehouse _resourceWarehouse;
    
    private TMP_Text _text;

    private void OnValidate()
    {
        if (_resourceWarehouse == null)
            throw new NullReferenceException(nameof(_resourceWarehouse));
    }

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        _resourceWarehouse.ChangedCount += SetCount;
    }

    private void OnDisable()
    {
        _resourceWarehouse.ChangedCount -= SetCount;
    }

    private void SetCount(int count)
    {
        _text.text = count.ToString();
    }
}
