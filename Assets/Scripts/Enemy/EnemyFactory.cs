using System;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private Transform _enemyPool;

    private void Awake()
    {
        if (_enemyPool == null)
            throw new NullReferenceException(nameof(_enemyPool));
    }

    public Enemy Create(Enemy prefab, Vector3 position)
    {
        return Instantiate(prefab, position, Quaternion.identity, _enemyPool);
    }
}
