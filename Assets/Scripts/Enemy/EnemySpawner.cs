using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
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
            Enemy enemy = _enemyFactory.Create(_enemyPrefab, transform.position);
            enemy.SetTarget(_target);
            yield return wait;
        }
    }
}
