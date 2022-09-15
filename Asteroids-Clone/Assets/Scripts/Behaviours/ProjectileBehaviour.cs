using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [Header("Shoot Controls"), SerializeField]
    private ShootType _shootType;

    [SerializeField] private float _lifetime;
    [SerializeField] private float _currentLifeTime;
    [SerializeField] private PoolController _poolController;

    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private bool _shootEnabled;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _playerShootSprite;
    [SerializeField] private Sprite _enemyShootSprite;

    private void Awake()
    {
        _currentLifeTime = _lifetime;
    }

    private void Update()
    {
        if (!_shootEnabled)
        {
            return;
        }

        _currentLifeTime -= 1 * Time.deltaTime;
        if (_currentLifeTime < 0f) DestroyShoot();
    }

    public void CreateShoot(PoolController poolController, float lifetime)
    {
        _poolController = poolController;
        _lifetime = lifetime;
    }

    public void ConfigureShoot(ShootType shootType)
    {
        _shootType = shootType;
        _rigidbody2D.velocity = Vector3.zero;
        if (shootType == ShootType.Player)
        {
            this.gameObject.layer = 8;
            _spriteRenderer.sprite = _playerShootSprite;
            this.tag = "Projectile";
        }

        if (shootType == ShootType.Enemy)
        {
            this.gameObject.layer = 9;
            _spriteRenderer.sprite = _enemyShootSprite;
            this.tag = "Enemy";
        }
    }

    public void DefaultTime()
    {
        _currentLifeTime = _lifetime;
        _shootEnabled = true;
    }

    public void DestroyShoot()
    {
        _shootEnabled = false;
        if (_poolController == null) _poolController = GameManager.Instance.ProjectilePool;
        _poolController.ReturnToPool(this.gameObject);
    }

    public void AddShootForce(float speed, Vector3 direction)
    {
        _rigidbody2D.velocity = Vector3.zero;
        _rigidbody2D.AddForce(direction * speed, ForceMode2D.Impulse);
    }
}

public enum ShootType
{
    None = 0,
    Player = 1,
    Enemy = 2
}