using System.Collections;
using UnityEngine;

public class ResourceSpawner : AreaSpawner<Resource>
{
    [SerializeField] private float _secCoolDown = .5f;

    protected override void OnValidate()
    {
        base.OnValidate();
        
        if (_secCoolDown < 0)
            _secCoolDown = 0;
    }
    
    private void OnEnable()
    {
        StartCoroutine(StartCreating());
    }

    private IEnumerator StartCreating()
    {
        var wait = new WaitForSeconds(_secCoolDown);

        while (enabled)
        {
            yield return wait;
            Get();
        }
    }
}
