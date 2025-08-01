using System;
using System.Collections;

public class CountMoreTransitionConditions : ITransitionCondition
{
    private readonly ICollection _collection;
    private readonly int _count;

    public CountMoreTransitionConditions(ICollection collection, int count)
    {
        _collection = collection ?? throw new ArgumentNullException(nameof(collection));;
        _count = count;
    }
    
    public bool IsDone()
    {
        return _collection.Count > _count;
    }
}
