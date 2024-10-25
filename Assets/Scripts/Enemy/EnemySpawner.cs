using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private float _coolDown;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Transform _enemyParent;
    [SerializeField] private Vector3 _moveDirection;

    private void OnEnable()
    {
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        var wait = new WaitForSeconds(_coolDown);
        
        while (enabled)
        {
            Create();
            yield return wait;
        }
    }
    
    private void Create()
    {
        Vector3 position = GetSpawnPoint();
        var enemy = Instantiate(_enemyPrefab, position, Quaternion.identity, _enemyParent);
        enemy.Move(_moveDirection);
    }

    private Vector3 GetSpawnPoint()
    {
        int randomPoint = Random.Range(0, _spawnPoints.Count);
        return _spawnPoints[randomPoint].position;
    }
}
