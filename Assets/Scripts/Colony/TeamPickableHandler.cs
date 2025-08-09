using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamPickableHandler : MonoBehaviour
{
    private List<IPickable> _pickableInWork = new List<IPickable>();
    private HashSet<IPickable> _freePickable = new HashSet<IPickable>();

    public event Action AddedPickables;
    
    public bool HaveFreePickable => _freePickable.Count > 0;

    public void AddPickables(ICollection<IPickable> pickables)
    {
        var pickablesHashSet = pickables.ToHashSet();
        pickablesHashSet.ExceptWith(_pickableInWork);
        pickablesHashSet.ExceptWith(_freePickable);

        if (pickablesHashSet.Count > 0)
        {
            _freePickable.UnionWith(pickablesHashSet);
            AddedPickables?.Invoke();
        }
    }

    public IPickable TakeNearestPickable(Vector3 position)
    {
        var pickable = _freePickable.OrderBy(pickable => pickable.Transform.position.SqrDistance(position)).First();
        _freePickable.Remove(pickable);
        _pickableInWork.Add(pickable);
        pickable.PutPickable += OnPutPickable;
        return pickable;
    }

    private void OnPutPickable(IPickable pickable)
    {
        pickable.PutPickable -= OnPutPickable;
        _pickableInWork.Remove(pickable);
    }
}
