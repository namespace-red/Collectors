using System;
using UnityEngine;

public class MoverByPoints : MonoBehaviour
{
    private const float PositionMagnitudeInaccuracy = 0.1f;
        
    [SerializeField] private Transform _wayPointParent;
    [SerializeField] private float _speed;
    [SerializeField] private Transform[] _wayPoints;
    
    private Rigidbody _rigidbody;
    private int _currentIndex;
    
    public Transform Target => _wayPoints[_currentIndex];
    private Vector3 Direction => (Target.position - transform.position).normalized;
    
    private void Awake()
    {
        if (_wayPoints == null)
            throw new NullReferenceException(name +  " WayPoints is empty. Run InitWayPoints in ContextMenu");
        
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        if ((Target.position - transform.position).magnitude < PositionMagnitudeInaccuracy)
        {
            SetNextPoint();
        }

        Rotate();
        Move();
    }

    [ContextMenu("InitWayPoints")]
    private void InitWayPoints()
    {
        _wayPoints = new Transform[_wayPointParent.childCount];

        for (int i = 0; i < _wayPointParent.childCount; i++)
            _wayPoints[i] = _wayPointParent.GetChild(i);
    }

    private void SetNextPoint()
    {
        _currentIndex = ++_currentIndex % _wayPoints.Length;
    }
    
    private void Rotate()
    {
        transform.forward = Direction;
    }

    private void Move()
    {
        Vector3 step = Direction * (_speed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(transform.position + step);
    }
}
