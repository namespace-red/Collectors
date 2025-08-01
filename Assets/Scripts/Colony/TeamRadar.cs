using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamRadar : MonoBehaviour
{
    [SerializeField] private ColonyFactory _colonyFactory;
    [SerializeField] private List<PickableDetector> _pickableDetectors;
    [SerializeField, Min(0.1f)] private float _secCoolDown;
    
    private List<IPickable> _pickableInWork = new List<IPickable>();
    private HashSet<IPickable> _freePickable = new HashSet<IPickable>();

    public event Action DetectedPickables;

    public bool HaveFreePickable => _freePickable.Count > 0;
    
    private void OnEnable()
    {
        StartCoroutine(RunDetecting());
    }

    private void Start()
    {
        _colonyFactory.Created += OnCreatedColony;
    }

    private void OnDestroy()
    {
        _colonyFactory.Created -= OnCreatedColony;
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

    private void OnCreatedColony(Colony colony)
    {
        _pickableDetectors.Add(colony.PickableDetector);
    }

    private IEnumerator RunDetecting()
    {
        var coolDown = new WaitForSeconds(_secCoolDown);
        
        while (enabled)
        {
            foreach (var detectedPickables in _pickableDetectors.
                Select(pickableDetector => pickableDetector.DetectAll().ToHashSet()).
                Where(detectedPickables => detectedPickables.Count > 0))
            {
                detectedPickables.ExceptWith(_pickableInWork);
                _freePickable.UnionWith(detectedPickables);
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
