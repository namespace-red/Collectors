using UnityEngine;

public class FirstColonyPlacer : MonoBehaviour
{
    [SerializeField] private UserInput _userInput;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField, Min(0)] private float _colonyRadius;
    [SerializeField] private ColonyFactory _colonyFactory;

    private PlaceChecker _placeChecker;
    
    private void Awake()
    {
        _placeChecker = new PlaceChecker(_groundLayer, _colonyRadius);
    }

    private void OnEnable()
    {
        _userInput.PressedLeftMouse += OnPressedLeftMouse;
    }

    private void OnDisable()
    {
        _userInput.PressedLeftMouse -= OnPressedLeftMouse;
    }

    private void OnPressedLeftMouse()
    {
        if (_placeChecker.TryGetPointForColony(out var point) == false)
            return;
                
        if (_placeChecker.CanPlaceColony(point) == false)
            return;

        _colonyFactory.CreateFirstColony(point);
        enabled = false;
    }
}
