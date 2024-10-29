using System;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private Transform _enemyParent;

    private void Awake()
    {
        if (_enemyParent == null)
            throw new NullReferenceException(nameof(_enemyParent));
    }

    public Enemy Create(Enemy prefab, Vector3 position)
    {
        return Instantiate(prefab, position, Quaternion.identity, _enemyParent);
    }
}
