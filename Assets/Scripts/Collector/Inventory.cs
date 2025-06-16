using System;

public class Inventory
{
    private IPickable _slot;

    public void Put(IPickable slot)
    {
        if (_slot != null)
            throw new StackOverflowException(nameof(_slot));
        
        _slot = slot ?? throw new NullReferenceException(nameof(slot));
    }
    
    public IPickable Take()
    {
        var slot = _slot;
        _slot = null;
        return slot;
    }
}
