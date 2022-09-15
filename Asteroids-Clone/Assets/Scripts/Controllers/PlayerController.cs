using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [Header("Data Reference"), SerializeField]
    private ShipData _shipData;

    [SerializeField] private ShipData[] _shipDataPool;

    [Header("Player Inputs"), SerializeField]
    private KeyCode _thottle;

    [SerializeField] private KeyCode _left;
    [SerializeField] private KeyCode _right;
    [SerializeField] private KeyCode _shoot;

    [Header("Local Reference"), SerializeField]
    private PlayerBehaviour _playerBehaviour;

    private void Awake()
    {
        _shipData = _shipDataPool[Random.Range(0, _shipDataPool.Length)];
        _playerBehaviour.ShipConfiguration(_shipData.Sprite, _shipData.Speed, _shipData.RotationSpeed,
            _shipData.DeaccelerationRate, _shipData.MaxHp, _shipData.ShootSpeed);
    }

    private void Update()
    {
        if (_playerBehaviour.IsDead) return;
        if (Input.GetKeyDown(_thottle)) _playerBehaviour.ThrusterState(true);
        if (Input.GetKeyUp(_thottle)) _playerBehaviour.ThrusterState(false);

        if (Input.GetKey(_thottle)) _playerBehaviour.Accelerate();
        if (Input.GetKey(_left)) _playerBehaviour.Turn(Direction.Left);
        if (Input.GetKey(_right)) _playerBehaviour.Turn(Direction.Right);
        if (Input.GetKeyDown(_shoot)) _playerBehaviour.Shoot();
    }
}