using System;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    private const int LeftMouse = 0;
    private const int RightMouse = 1;
    
    public event Action PressedLeftMouse;
    public event Action PressedRightMouse;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(LeftMouse)) 
            PressedLeftMouse?.Invoke();
        
        if (Input.GetMouseButtonDown(RightMouse)) 
            PressedRightMouse?.Invoke();
    }
}
