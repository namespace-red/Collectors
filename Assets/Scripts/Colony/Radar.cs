using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField] private PickableDetector _pickableDetector;
    [SerializeField, Min(0.1f)] private float _secCoolDown;
    
    private List<IPickable> _pickableInWork = new List<IPickable>();
    private HashSet<IPickable> _freePickable = new HashSet<IPickable>();

    public event Action DetectedPickables;

    public bool HaveFreePickable => _freePickable.Count > 0;
    
    private void OnEnable()
    {
        StartCoroutine(RunDetecting());
    }
    
    public void Run() => enabled = true;

    public void Stop() => enabled = false;

    public IPickable TakeNearestPickable(Vector3 position)
    {
        var pickable = _freePickable.OrderBy(pickable => pickable.Transform.position.SqrDistance(position)).First();
        _freePickable.Remove(pickable);
        _pickableInWork.Add(pickable);
        pickable.PutPickable += OnPutPickable;
        return pickable;
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
                DetectedPickables?.Invoke();
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
