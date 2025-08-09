using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TeamPickableHandler))]
public class TeamCollectorHandler : MonoBehaviour
{
    [SerializeField] private ColonyFactory _colonyFactory;
    
    private List<Colony> _colonies = new List<Colony>();
    private TeamPickableHandler _teamPickableHandler;

    private void Awake()
    {
        _teamPickableHandler = GetComponent<TeamPickableHandler>();
    }

    private void OnEnable()
    {
        _teamPickableHandler.AddedPickables += SendCollectorForPickable;
        _colonyFactory.Created += _colonies.Add;
    }

    private void OnDisable()
    {
        _teamPickableHandler.AddedPickables -= SendCollectorForPickable;
        _colonyFactory.Created -= _colonies.Add;
    }

    private void SendCollectorForPickable()
    {
        while (HaveFreeCollector() && _teamPickableHandler.HaveFreePickable)
        {
            Collector collector = GetFreeCollector();
            var pickable = _teamPickableHandler.TakeNearestPickable(collector.transform.position);
            collector.GoToPickable(pickable);
        }

        if (HaveFreeCollector() == false)
        {
            foreach (var colony in _colonies)
                colony.StopPickableDetector();
        }
    }

    private bool HaveFreeCollector()
        => _colonies.Any(colony => colony.HaveFreeCollector());

    private Collector GetFreeCollector()
    {
        foreach (var colony in _colonies)
        {
            if (colony.HaveFreeCollector())
            {
                return colony.GetFreeCollector();
            }
        }
        
        return null;
    }
}
