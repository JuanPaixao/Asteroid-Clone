using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PoolController : MonoBehaviour
{
    [SerializeField] private bool _enemyPool;

    [SerializeField] private List<PoolObject> _objectData;
    private Stack<GameObject> _createdPool;
    private GameObject _poolObject;

    [SerializeField] private int _objectsQuantity;

    [Header("Scene References"), SerializeField]
    private SpawnManager _spawnManager;

    [SerializeField] private GameManager _gameManager;

    public SpawnManager SpawnManager
    {
        get => _spawnManager;
        set => _spawnManager = value;
    }

    public GameManager GameManager
    {
        get => _gameManager;
        set => _gameManager = value;
    }

    public void CreatePool(int quantity, GameObject poolObject)
    {
        _createdPool = new Stack<GameObject>();
        _poolObject = poolObject;
        if (_enemyPool)
        {
            for (int i = 0; i < quantity; i++)
            {
                CreateRandomEnemy(poolObject);
            }
        }
        else
        {
            for (int i = 0; i < quantity; i++)
            {
                CreateProjectile(poolObject);
            }
        }
    }

    public void CreateRandomEnemy(GameObject poolObject)
    {
        var createdObject = Instantiate(poolObject, Vector2.zero, Quaternion.identity, this.transform);
        var selectedRandomEnemy = (EnemyData)GetRandomEnemy();
        createdObject.SetActive(false);
        _poolObject.GetComponent<EnemyBehaviour>().ConfigureEnemy(selectedRandomEnemy.Sprite,
            selectedRandomEnemy.Speed,
            selectedRandomEnemy.EnemyType, selectedRandomEnemy.Size, selectedRandomEnemy.HasChildAsteroids,
            selectedRandomEnemy.ChildAsteroids,
            selectedRandomEnemy.CollisionRadius, selectedRandomEnemy.ShootCooldown, selectedRandomEnemy.ShootSpeed,
            this);
        _createdPool.Push(createdObject);
        _objectsQuantity = _createdPool.Count;
    }

    public void CreateProjectile(GameObject poolObject)
    {
        var createdObject = Instantiate(poolObject, Vector2.zero, Quaternion.identity, this.transform);
        createdObject.SetActive(false);
        var behaviour = _poolObject.GetComponent<ProjectileBehaviour>();
        behaviour.CreateShoot(this, GameManager.Instance.LaserLifetime);
        behaviour.DefaultTime();
        _createdPool.Push(createdObject);
        _objectsQuantity = _createdPool.Count;
    }

    public GameObject GetFromPool()
    {
        if (_createdPool.Count > 0)
        {
            return PopFromPool();
        }

        if (_enemyPool)
        {
            CreateRandomEnemy(_poolObject);
        }
        else
        {
            CreateProjectile(_poolObject);
        }

        return PopFromPool();
    }


    private GameObject PopFromPool()
    {
        var receivedObject = _createdPool.Pop();
        receivedObject.SetActive(true);
        _objectsQuantity = _createdPool.Count;
        return receivedObject;
    }

    public void ReturnToPool(GameObject poolObject)
    {
        poolObject.SetActive(false);
        _createdPool.Push(poolObject);
        _objectsQuantity = _createdPool.Count;
    }

    public PoolObject GetRandomEnemy()
    {
        int randomEnemy = Random.Range(0, _objectData.Count);
        return _objectData[randomEnemy];
    }

    public GameObject GetSpecificEnemy(EnemyData enemyData, Transform position)
    {
        var createdObject = GetFromPool();
        var selectedEnemy = createdObject.GetComponent<EnemyBehaviour>();
        selectedEnemy.ConfigureEnemy(enemyData.Sprite, enemyData.Speed, enemyData.EnemyType, enemyData.Size,
            enemyData.HasChildAsteroids, enemyData.ChildAsteroids,
            enemyData.CollisionRadius, enemyData.ShootCooldown, enemyData.ShootSpeed, this);
        _objectsQuantity = _createdPool.Count;
        return createdObject;
    }
}

[Serializable]
class EnemyPool
{
}