using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [Header("Pools"), SerializeField] private PoolController _enemyPool;
    [SerializeField] private PoolController _projectilesPool;

    [Header("Prefabs"), SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _laserPrefab;

    [Header("Position Reference"), SerializeField]
    private float _maxXLimit;

    [SerializeField] private float _minXLimit;
    [SerializeField] private float _maxYLimit;
    [SerializeField] private float _minYLimit;

    private void Start()
    {
        _enemyPool.CreatePool(_gameManager.MaxEnemies, _enemyPrefab);
        _projectilesPool.CreatePool(_gameManager.MaxLasers, _laserPrefab);

        InvokeRepeating(nameof(SpawnEnemy), 1f, _gameManager.EnemySpawnTime);
    }

    public void SpawnEnemy()
    {
        var createdEnemy = _enemyPool.GetFromPool();
        var createdBehaviour = createdEnemy.GetComponent<EnemyBehaviour>();
        var randomEnemy = (EnemyData)_enemyPool.GetRandomEnemy();
        createdBehaviour.ConfigureEnemy(randomEnemy.Sprite, randomEnemy.Speed, randomEnemy.EnemyType, randomEnemy.Size,
            randomEnemy.HasChildAsteroids,
            randomEnemy.ChildAsteroids, randomEnemy.CollisionRadius, randomEnemy.ShootCooldown, randomEnemy.ShootSpeed,
            createdBehaviour.PoolReference);
        createdEnemy.transform.position = Position();
        createdEnemy.SetActive(true);
        createdBehaviour.AddRandomSpeed();
    }

    public void SpawnChildEnemy(EnemyData enemyData, Transform position)
    {
        var createdEnemy = _enemyPool.GetSpecificEnemy(enemyData, position);
        var createdBehaviour = createdEnemy.GetComponent<EnemyBehaviour>();
        createdEnemy.transform.position = position.position;
        createdEnemy.SetActive(true);
        createdBehaviour.AddRandomSpeed();
    }

    public Vector3 Position()
    {
        float min = 0.1f;
        float max = 0.9f;
        Vector3 final = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(min, max), 1, 10));

        return final;
    }
}