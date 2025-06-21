using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickableInWorldController : MonoBehaviour
{
    [SerializeField] private PickableDetector _pickableDetector;
    [SerializeField, Min(0.1f)] private float _secCoolDown;
    
    private List<IPickable> _pickableInWork = new List<IPickable>();
    private HashSet<IPickable> _freePickable = new HashSet<IPickable>();
    
    public event Action<IEnumerable<IPickable>> DetectedPickables;
    
    private void OnEnable()
    {
        StartCoroutine(RunDetecting());
    }

    public void RunDetector() => enabled = true;

    public void StopDetector() => enabled = false;

    public void Take(IPickable pickable)
    {
        _freePickable.Remove(pickable);
        _pickableInWork.Add(pickable);
        pickable.PutPickable += OnPutPickable;
    }
    
    private IEnumerator RunDetecting()
    {
        var coolDown = new WaitForSeconds(_secCoolDown);
        
        while (enabled)
        {
            var detectedPickable = _pickableDetector.DetectAll().ToHashSet();

            if (detectedPickable.Count > 0)
            {
                detectedPickable.ExceptWith(_pickableInWork);
                _freePickable.UnionWith(detectedPickable);
                DetectedPickables?.Invoke(_freePickable);
            }

            yield return coolDown;
        }
    }
    
    private void OnPutPickable(IPickable pickable)
    {
        pickable.PutPickable -= OnPutPickable;
        _pickableInWork.Remove(pickable);
    }
}
