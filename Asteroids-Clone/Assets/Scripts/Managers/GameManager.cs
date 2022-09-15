using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Parameters"), SerializeField]
    private float _xScreenLimit;

    [SerializeField] private float _yScreenLimit;
    [SerializeField] private int _maxEnemies;
    [SerializeField] private int _maxLasers;
    [SerializeField] private float _laserLifetime;
    [SerializeField] private float _enemySpawnTime;
    [SerializeField] private int _timeToResetGame;
    [Header("Pools Reference"), SerializeField]
    private PoolController _enemyPool;

    [SerializeField] private PoolController _projectilePool;

    public static GameManager Instance;

    [Header("Scene References"), SerializeField]
    private UIManager _uiManager;

    [Header("Game"), SerializeField] private int _score;

    private void Awake()
    {
        Instance = this;
    }

    public float XScreenLimit
    {
        get => _xScreenLimit;
        set => _xScreenLimit = value;
    }

    public float YScreenLimit
    {
        get => _yScreenLimit;
        set => _yScreenLimit = value;
    }

    public int MaxEnemies
    {
        get => _maxEnemies;
        set => _maxEnemies = value;
    }

    public int MaxLasers
    {
        get => _maxLasers;
        set => _maxLasers = value;
    }

    public float LaserLifetime
    {
        get => _laserLifetime;
        set => _laserLifetime = value;
    }

    public PoolController ProjectilePool
    {
        get => _projectilePool;
        set => _projectilePool = value;
    }

    public PoolController EnemyWPool
    {
        get => _enemyPool;
        set => _enemyPool = value;
    }

    public float EnemySpawnTime
    {
        get => _enemySpawnTime;
        set => _enemySpawnTime = value;
    }

    public async void RestartGame()
    {
        await Task.Delay(_timeToResetGame * 1000);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Scored()
    {
        _score++;
        _uiManager.UpdateScore(_score);
    }

    public void UpdatePlayerHP(int hp)
    {
        _uiManager.UpdateHP(hp);
    }
}