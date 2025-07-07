using UnityEngine;

public class AreaFactory<T> : Factory<T>
    where T : MonoBehaviour
{
    [SerializeField] private Collider _spawnArea;

    private PointInBox _pointInBox;

    private void Awake()
    {
        _pointInBox = new PointInBox(_spawnArea.bounds);
    }

    public override T Create()
    {
        T obj = base.Create();
        obj.transform.position = _pointInBox.GetRandom();
        
        return obj;
    }
}
