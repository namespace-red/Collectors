using System.Collections;
using UnityEngine;

public class ResourceFactory : Factory<Resource>
{
    [SerializeField] private Collider _spawnArea;
    [SerializeField, Min(0.01f)] private float _secCoolDown;
    [SerializeField, Min(0)] private int _startCount;

    private PointInBox _pointInBox;

    protected override void Awake()
    {
        base.Awake();
        
        _pointInBox = new PointInBox(_spawnArea.bounds);
    }

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
        resource.transform.position = _pointInBox.GetRandom();
        
        resource.PutPickable += DestroyResource;
        return resource;
    }

    private void DestroyResource(IPickable pickable)
    {
        pickable.PutPickable -= DestroyResource;
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
