using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Ship Local Components"), SerializeField]
    private SpriteRenderer _shipSpriteRenderer;

    [SerializeField] private SpriteRenderer _thruster;
    [SerializeField] private Transform _shootSpawnPosition;
    [SerializeField] private GameObject _explosionParticle;

    [Header("Ship Parameters"), SerializeField]
    private float _speed = 5f;

    [SerializeField] private float _shootSpeed = 1f;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private int _maxHP;
    [SerializeField] private int _currentHP;
    [SerializeField] private int _invulnerableTime;
    [Header("Physics"), SerializeField] private Rigidbody2D _rb;
    private PoolController _projectilePool;
    [Header("States"), SerializeField] private bool _canTakeDamage;
    [SerializeField] private bool _isDead;

    public bool IsDead
    {
        get => _isDead;
        set => _isDead = value;
    }

    private void Start()
    {
        _projectilePool = GameManager.Instance.ProjectilePool;
        GameManager.Instance.UpdatePlayerHP(_currentHP);
    }

    public void ShipConfiguration(Sprite sprite, float speed, float rotationSpeed, float deaccelerationRate, int maxHP,
        float shootSpeed)
    {
        _shipSpriteRenderer.sprite = sprite;
        _speed = speed;
        _rotationSpeed = rotationSpeed;
        _maxHP = maxHP;
        _currentHP = maxHP;
        _rb.drag = deaccelerationRate;
        _shootSpeed = shootSpeed;
        _canTakeDamage = true;
    }

    public void Accelerate()
    {
        _rb.AddForce(transform.right * _speed * Time.fixedDeltaTime);
    }

    public void ThrusterState(bool state)
    {
        _thruster.enabled = state;
    }

    public void Turn(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                transform.Rotate(-Vector3.back * _rotationSpeed * Time.deltaTime);
                break;
            case Direction.Right:
                transform.Rotate(Vector3.back * _rotationSpeed * Time.deltaTime);
                break;
        }
    }

    public void Shoot()
    {
        var projectile = _projectilePool.GetFromPool();
        var projectileBehaviour = projectile.GetComponent<ProjectileBehaviour>();
        projectileBehaviour.DefaultTime();
        projectileBehaviour.ConfigureShoot(ShootType.Player);
        projectile.SetActive(true);
        projectileBehaviour.AddShootForce(_shootSpeed, transform.right);
        projectile.transform.position = _shootSpawnPosition.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            if (_canTakeDamage && _currentHP > 0)
            {
                _currentHP--;
                GameManager.Instance.UpdatePlayerHP(_currentHP);
                if (_currentHP > 0) UpdateInvulnerability();
                else
                {
                    _isDead = true;
                    _explosionParticle.SetActive(true);
                    _shipSpriteRenderer.gameObject.SetActive(false);
                    _thruster.enabled = false;
                }
            }
        }
    }

    private async void UpdateInvulnerability()
    {
        SpriteRendererState(false);
        await Task.Delay(_invulnerableTime * 100);
        SpriteRendererState(true);
        await Task.Delay(_invulnerableTime * 100);
        SpriteRendererState(false);
        await Task.Delay(_invulnerableTime * 100);
        SpriteRendererState(true);
        await Task.Delay(_invulnerableTime * 100);
        SpriteRendererState(false);
        await Task.Delay(_invulnerableTime * 100);
        SpriteRendererState(true);
        await Task.Delay(_invulnerableTime * 100);
        _canTakeDamage = true;
    }

    private void SpriteRendererState(bool state)
    {
        _shipSpriteRenderer.enabled = state;
    }
}

public enum Direction
{
    None = 0,
    Left = 1,
    Right = 2
}