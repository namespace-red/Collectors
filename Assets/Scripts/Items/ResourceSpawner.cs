using System.Collections;
using UnityEngine;

public class ResourceSpawner : AreaSpawner<Resource>
{
    [SerializeField, Min(0.01f)] private float _secCoolDown;
    [SerializeField, Min(1)] private int _startCount;

    private void OnEnable()
    {
        StartCoroutine(StartCreating());
    }

    private void Start()
    {
        for (int i = 0; i < _startCount - 1; i++) //_startCount - 1 тк в корутине уже 1 создался
        {
            Create();
        }
    }

    public override Resource Create()
    {
        var resource = base.Create();
        resource.PutPickable += DestroyResource;
        return resource;
    }

    private void DestroyResource(IPickable pickable)
    {
        pickable.PutPickable += DestroyResource;
        Destroy(pickable.Transform.gameObject);
    }

    private IEnumerator StartCreating()
    {
        var wait = new WaitForSeconds(_secCoolDown);

        while (enabled)
        {
            Create();
            yield return wait;
        }
    }
}
