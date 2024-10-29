using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MoverByTarget))]
public class Enemy : MonoBehaviour
{
    private MoverByTarget _moverByTarget;

    private void Awake()
    {
        _moverByTarget = GetComponent<MoverByTarget>();
    }

    public void SetTarget(Transform target)
    {
        _moverByTarget.Target = target;
    }
}
