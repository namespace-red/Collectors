using System;
using UnityEngine;

public class FlagInputHandler : MonoBehaviour
{
    [SerializeField] private UserInput _userInput;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField, Min(0)] private float _colonyRadius;
    
    private Mod _currentMod = Mod.GetFlag;
    private Flag _currentFlag;
    private PlaceChecker _placeChecker;

    private void Awake()
    {
        _placeChecker = new PlaceChecker(_groundLayer, _colonyRadius);
    }

    private void OnEnable()
    {
        _userInput.PressedLeftMouse += OnPressedLeftMouse;
        _userInput.PressedRightMouse += OnPressedRightMouse;
    }

    private void OnDisable()
    {
        _userInput.PressedLeftMouse -= OnPressedLeftMouse;
        _userInput.PressedRightMouse -= OnPressedRightMouse;
    }

    private void OnPressedLeftMouse()
    {
        switch (_currentMod)
        {
            case Mod.GetFlag:
                if (_placeChecker.TryGetColony(out var colony) == false)
                    return;

                _currentFlag = colony.Flag;
                _currentMod = Mod.PlaceFlag;
                break;
            
            case Mod.PlaceFlag:
                if (_placeChecker.TryGetPointForColony(out var point) == false)
                    return;
                
                if (_placeChecker.CanPlaceColony(point) == false)
                    return;
                
                _currentFlag.Place(point);
                _currentFlag = null;
                _currentMod = Mod.GetFlag;
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(_currentMod));
        }
    }

    private void OnPressedRightMouse()
    {
        switch (_currentMod)
        {
            case Mod.GetFlag:
                break;
            
            case Mod.PlaceFlag:
                _currentFlag = null;
                _currentMod = Mod.GetFlag;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(_currentMod));
        }
    }

    private enum Mod
    {
        GetFlag,
        PlaceFlag
    }
}
