using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MoverByTarget))]
public class Enemy : MonoBehaviour
{
    private MoverByTarget _moverByTarget;

    public Transform Target
    { 
        get => _moverByTarget.Target;
        set => _moverByTarget.Target = value; 
    }

    private void Awake()
    {
        _moverByTarget = GetComponent<MoverByTarget>();
    }
}
