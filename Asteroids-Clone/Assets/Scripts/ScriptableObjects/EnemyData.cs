using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyData : PoolObject
{
    [Header("Enemy Sprite"), SerializeField]
    private Sprite _sprite;

    [Header("Enemy Parameters"), Range(0f, 5f), SerializeField]
    private float _speed = 1f;

    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private EnemySize _enemySize;
    [SerializeField] private bool _hasChildAsteroids;
    [SerializeField] private List<EnemyData> _childAsteroids;
    [SerializeField] private float _collisionRadius;
    [Range(0f, 10f), SerializeField] private float _shootCooldown;
    [Range(0f, 10f), SerializeField] private float _shootSpeed;
    public Sprite Sprite
    {
        get => _sprite;
        set => _sprite = value;
    }

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public EnemyType EnemyType
    {
        get => _enemyType;
        set => _enemyType = value;
    }

    public EnemySize Size
    {
        get => _enemySize;
        set => _enemySize = value;
    }

    public bool HasChildAsteroids
    {
        get => _hasChildAsteroids;
        set => _hasChildAsteroids = value;
    }

    public List<EnemyData> ChildAsteroids
    {
        get => _childAsteroids;
        set => _childAsteroids = value;
    }

    public float CollisionRadius
    {
        get => _collisionRadius;
        set => _collisionRadius = value;
    }

    public float ShootCooldown
    {
        get => _shootCooldown;
        set => _shootCooldown = value;
    }

    public float ShootSpeed
    {
        get => _shootSpeed;
        set => _shootSpeed = value;
    }
}

[Serializable]
public enum EnemySize
{
    Small = 0,
    Medium = 1,
    Big = 2
}

[Serializable]
public enum EnemyType
{
    Asteroid = 0,
    EnemyShip = 1,
}