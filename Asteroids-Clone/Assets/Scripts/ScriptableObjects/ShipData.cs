using UnityEngine;

[CreateAssetMenu(fileName = "ShipData", menuName = "ScriptableObjects/Ship", order = 1)]
public class ShipData : ScriptableObject
{
    [Header("Ship Sprite"), SerializeField]
    private Sprite _sprite;

    [Header("Ship Parameters"), Range(1f, 200f), SerializeField]
    private float _speed = 100f;

    [Range(1f, 10f), SerializeField] private float _shootSpeed = 3f;
    [Range(1f, 300f), SerializeField] private float _rotationSpeed = 5f;
    [Range(0f, 2f), SerializeField] private float _deaccelerationRate = 5f;
    [Range(1, 5), SerializeField] private int _maxHP = 3;

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

    public float DeaccelerationRate
    {
        get => _deaccelerationRate;
        set => _deaccelerationRate = value;
    }

    public float RotationSpeed
    {
        get => _rotationSpeed;
        set => _rotationSpeed = value;
    }

    public int MaxHp
    {
        get => _maxHP;
        set => _maxHP = value;
    }

    public float ShootSpeed
    {
        get => _shootSpeed;
        set => _shootSpeed = value;
    }
}