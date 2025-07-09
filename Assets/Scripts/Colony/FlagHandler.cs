using System;
using UnityEngine;

public class FlagHandler : MonoBehaviour
{
    [SerializeField] private UserInput _userInput;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField, Min(0)] private float _colonyRadius;
    
    private Camera _camera;
    private Mod _currentMod = Mod.GetFlag;
    private Flag _currentFlag;

    private void OnEnable()
    {
        _userInput.PressedLeftMouse += OnPressedLeftMouse;
    }

    private void OnDisable()
    {
        _userInput.PressedLeftMouse -= OnPressedLeftMouse;
    }

    private void Start()
    {
        _camera = Camera.main;
    }
    
    private void OnPressedLeftMouse()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        switch (_currentMod)
        {
            case Mod.GetFlag:
                if (Physics.Raycast(ray, out hit) == false)
                {
                    return;
                }

                var colony = hit.collider.GetComponentInParent<Colony>();

                if (colony == null)
                {
                    return;
                }

                _currentFlag = colony.Flag;
                _currentMod = Mod.PlaceFlag;
                break;
            
            case Mod.PlaceFlag:
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer) && IsFreePlace(hit.point))
                {
                    _currentFlag.Place(hit.point);
                }
                
                _currentFlag = null;
                _currentMod = Mod.GetFlag;
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(_currentMod));
        }
    }

    private bool IsFreePlace(Vector3 center)
    {
        return Physics.CheckSphere(center, _colonyRadius, ~_groundLayer, QueryTriggerInteraction.Ignore) == false;
    }
    
    private enum Mod
    {
        GetFlag,
        PlaceFlag
    }
}
