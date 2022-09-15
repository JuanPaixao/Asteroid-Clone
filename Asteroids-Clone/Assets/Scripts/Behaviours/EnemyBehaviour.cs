using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemy Sprite Renderer"), SerializeField]
    private SpriteRenderer _spriteRenderer;

    [Header("Enemy Parameters"), SerializeField]
    private float _speed = 100f;

    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private EnemySize _enemySize;
    [SerializeField] private bool _hasChildAsteroids;
    [SerializeField] private List<EnemyData> _childAsteroids;
    [SerializeField] private float _shootCooldown;
    [SerializeField] private float _shootSpeed;
    private float _shootCurrentCooldown;
    [Header("Controls"), SerializeField] private PoolController _poolReference;

    [Header("Local References"), SerializeField]
    private CircleCollider2D _circleCollider2D;

    [SerializeField] private Transform _shootSpawnPosition;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [Header("VFX"), SerializeField] private GameObject _explosion;

    [Header("Scene References"), SerializeField]
    private PoolController _projectilePool;


    public PoolController PoolReference
    {
        get => _poolReference;
        set => _poolReference = value;
    }

    public void ConfigureEnemy(Sprite sprite, float speed, EnemyType enemyType, EnemySize enemySize,
        bool hasChildAsteroids, List<EnemyData> childAsteroids, float collisionRadius, float shootCooldown,
        float shootSpeed,
        PoolController poolController)

    {
        PoolReference = poolController;
        _spriteRenderer.sprite = sprite;
        _speed = speed;
        _enemyType = enemyType;
        if (_enemyType == EnemyType.EnemyShip) _shootCooldown = shootCooldown;
        _enemySize = enemySize;
        _hasChildAsteroids = hasChildAsteroids;
        if (hasChildAsteroids)
        {
            _childAsteroids = new List<EnemyData>();
            _childAsteroids = childAsteroids;
        }

        _shootCurrentCooldown = _shootCooldown;
        _shootSpeed = shootSpeed;
        _circleCollider2D.radius = collisionRadius;
        if (_projectilePool == null) _projectilePool = GameManager.Instance.ProjectilePool;
        _projectilePool = _poolReference.GameManager.ProjectilePool;
    }

    private void Update()
    {
        if (_enemyType == EnemyType.EnemyShip)
        {
            _shootCurrentCooldown -= Time.deltaTime;
            if (_shootCurrentCooldown <= 0)
            {
                Shoot();
                bool rotate = Random.Range(0f, 100f) < 50f;
                if (rotate) RotateRandomly();
            }
        }
    }

    public void Shoot()
    {
        var projectile = _projectilePool.GetFromPool();
        var projectileBehaviour = projectile.GetComponent<ProjectileBehaviour>();
        projectileBehaviour.DefaultTime();
        projectileBehaviour.ConfigureShoot(ShootType.Enemy);
        projectile.SetActive(true);
        projectileBehaviour.AddShootForce(_shootSpeed, transform.right);
        projectile.transform.position = _shootSpawnPosition.transform.position;
        _shootCurrentCooldown = _shootCooldown;
    }

    public void RotateRandomly()
    {
        _rigidbody2D.velocity = Vector2.zero;
        AddRandomSpeed();
    }

    public void AddRandomSpeed()
    {
        var direction = Random.insideUnitCircle.normalized;
        transform.eulerAngles = new Vector3(0, 0, direction.x * Random.Range(0f, 360f));
        _rigidbody2D.AddForce(direction * _speed, ForceMode2D.Impulse);
        Debug.Log(direction);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Projectile"))
        {
            col.GetComponent<ProjectileBehaviour>().DestroyShoot();
            DamageEvents();
            _circleCollider2D.enabled = false;
        }
    }

    private void DamageEvents()
    {
        _rigidbody2D.velocity = Vector2.zero;
        if (_hasChildAsteroids)
        {
            for (int i = 0; i < _childAsteroids.Count; i++)
            {
                PoolReference.SpawnManager.SpawnChildEnemy(_childAsteroids[i], this.transform);
            }
        }

        _explosion.SetActive(true);
    }

    public void ReturnToPool()
    {
        _explosion.SetActive(false);
        _poolReference.GameManager.Scored();
        if (PoolReference == null) PoolReference = GameManager.Instance.ProjectilePool;
        _circleCollider2D.enabled = true;
        PoolReference.ReturnToPool(this.gameObject);
    }
}