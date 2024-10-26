using UnityEngine;

public class MoverByPoints : MoverByTarget
{
    private const float PositionMagnitudeInaccuracy = 0.1f;
        
    [SerializeField] private Transform _pointParent;
    
    private Transform[] _points;
    private int _currentIndex;
    
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        InitPoints();
        Target = _points[_currentIndex];
    }
    
    private void FixedUpdate()
    {
        if ((Target.position - transform.position).magnitude < PositionMagnitudeInaccuracy)
        {
            SetNextPoint();
            Target = _points[_currentIndex];
        }

        Rotate();
        Move();
    }

    private void InitPoints()
    {
        _points = new Transform[_pointParent.childCount];

        for (int i = 0; i < _pointParent.childCount; i++)
            _points[i] = _pointParent.GetChild(i).transform;
    }

    private void SetNextPoint()
    {
        _currentIndex++;

        if (_currentIndex == _points.Length)
            _currentIndex = 0;
    }
}
