using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private Transform _target;
    [SerializeField] private float _coolDown;
    [SerializeField] private EnemyFactory _enemyFactory;

    private void OnEnable()
    {
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        var wait = new WaitForSeconds(_coolDown);
        
        while (enabled)
        {
            _enemyFactory.Create(_enemyType, transform.position, _target);
            yield return wait;
        }
    }
}
