using System;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private Enemy _redPrefab;
    [SerializeField] private Enemy _orangePrefab;
    [SerializeField] private Enemy _yellowPrefab;
    [SerializeField] private Transform _enemyParent;

    private void Awake()
    {
        CheckPrefabs();
    }

    public Enemy Create(EnemyType enemyType, Vector3 position, Transform target)
    {
        Enemy newEnemy;
        
        switch (enemyType)
        {
            case EnemyType.Red:
                newEnemy = Create(_redPrefab, position);
                break;

            case EnemyType.Orange:
                newEnemy = Create(_orangePrefab, position);
                break;
            
            case EnemyType.Yellow:
                newEnemy = Create(_yellowPrefab, position);
                break;
            
            default:
                throw new ArgumentException($"Not correct type {enemyType}");
        }

        newEnemy.Target = target;
        return newEnemy;
    }
    
    private Enemy Create(Enemy prefab, Vector3 position)
    {
        return Instantiate(prefab, position, Quaternion.identity, _enemyParent);
    }

    private void CheckPrefabs()
    {
        CheckForNull(_redPrefab);
        CheckForNull(_orangePrefab);
        CheckForNull(_yellowPrefab);
    }

    private void CheckForNull(Enemy enemy)
    {
        if (enemy == null)
            throw new NullReferenceException(nameof(enemy));
    }
}
