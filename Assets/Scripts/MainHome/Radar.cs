using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PickableDetector))]
public class Radar : MonoBehaviour
{
    [SerializeField] private float _coolDown;

    private PickableDetector _pickableDetector;
    private List<IPickable> _detectedPickables = new List<IPickable>();

    public event Action<List<IPickable>> DetectedPickable;

    private void Awake()
    {
        _pickableDetector = GetComponent<PickableDetector>();
    }

    private void OnEnable()
    {
        StartCoroutine(RunDetector());
    }

    private IEnumerator RunDetector()
    {
        var coolDown = new WaitForSeconds(_coolDown);
        
        while (enabled)
        {
            _detectedPickables.Clear();

            _detectedPickables = _pickableDetector.DetectAll().ToList();

            if (_detectedPickables.Count > 0)
            {
                DetectedPickable?.Invoke(_detectedPickables);
            }

            yield return coolDown;
        }
    }
}
