using System;

public class Inventory
{
    private IPickable _slot;

    public void Put(IPickable slot)
    {
        _slot = slot ?? throw new StackOverflowException(nameof(slot));
    }
    
    public IPickable Take()
    {
        var slot = _slot;
        _slot = null;
        return slot;
    }
}
