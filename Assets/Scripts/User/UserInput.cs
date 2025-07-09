using System;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    private const int LeftMouse = 0;
    
    public event Action PressedLeftMouse;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(LeftMouse)) 
            PressedLeftMouse?.Invoke();
    }
}
