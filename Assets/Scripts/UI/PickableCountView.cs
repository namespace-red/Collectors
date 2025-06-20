using System;
using TMPro;
using UnityEngine;

public class PickableCountView : MonoBehaviour
{
    [SerializeField] private Colony colony;
    [SerializeField] private Canvas _canvas;
    
    private TMP_Text _text;

    private void OnValidate()
    {
        if (colony == null)
            throw new NullReferenceException(nameof(colony));
    }

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        transform.SetParent(_canvas.transform);
    }

    private void OnEnable()
    {
        colony.ChangedPickableCount += SetCount;
    }

    private void OnDisable()
    {
        colony.ChangedPickableCount -= SetCount;
    }

    private void SetCount(int count)
    {
        _text.text = count.ToString();
    }
}
