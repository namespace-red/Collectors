using System;

public interface IPickable
{
    public event Action Picked;
    
    public void PickUp();
}