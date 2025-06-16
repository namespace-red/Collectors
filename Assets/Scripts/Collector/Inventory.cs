using System;

public class Inventory
{
    private IPickable _slot;

    public void Put(IPickable slot)
    {
        if (_slot != null)
            throw new InvalidOperationException(nameof(_slot));
        
        _slot = slot ?? throw new ArgumentNullException(nameof(slot));
    }
    
    public IPickable Take()
    {
        var slot = _slot;
        _slot = null;
        return slot;
    }
}
