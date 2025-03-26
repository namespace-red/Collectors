using System;
using TMPro;
using UnityEngine;

public class PickableCountView : MonoBehaviour
{
    [SerializeField] private MainHome _mainHome;
    [SerializeField] private Canvas _canvas;
    
    private TMP_Text _text;

    private void OnValidate()
    {
        if (_mainHome == null)
            throw new NullReferenceException(nameof(_mainHome));
    }

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        transform.SetParent(_canvas.transform);
    }

    private void OnEnable()
    {
        _mainHome.ChangedAppleCount += SetCount;
    }

    private void OnDisable()
    {
        _mainHome.ChangedAppleCount -= SetCount;
    }

    private void SetCount(int count)
    {
        _text.text = count.ToString();
    }
}
