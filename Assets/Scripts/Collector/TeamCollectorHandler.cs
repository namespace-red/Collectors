using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TeamRadar))]
public class TeamCollectorHandler : MonoBehaviour
{
    [SerializeField] private ColonyFactory _colonyFactory;
    [SerializeField] private List<Colony> _colonies;

    private TeamRadar _teamRadar;

    private void Awake()
    {
        _teamRadar = GetComponent<TeamRadar>();
    }

    private void OnEnable()
    {
        _teamRadar.DetectedPickables += OnDetectedPickable;
        _colonyFactory.Created += _colonies.Add;
    }

    private void OnDisable()
    {
        _teamRadar.DetectedPickables -= OnDetectedPickable;
        _colonyFactory.Created -= _colonies.Add;
    }

    public void RunRadar()
    {
        _teamRadar.Run();
    }

    private void OnDetectedPickable()
    {
        while (HaveFreeCollector() && _teamRadar.HaveFreePickable)
        {
            Collector collector = GetFreeCollector();
            var pickable = _teamRadar.TakeNearestPickable(collector.transform.position);
            collector.SetPickableTarget(pickable);
        }

        if (HaveFreeCollector() == false)
        {
            _teamRadar.Stop();
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
