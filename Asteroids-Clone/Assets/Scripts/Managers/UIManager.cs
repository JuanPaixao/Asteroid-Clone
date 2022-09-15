using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("HUD"), SerializeField] private TextMeshProUGUI _playerHP;
    [SerializeField] private TextMeshProUGUI _score;
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateScore(int score)
    {
        _score.text = $"Score: {score}";
    }

    public void UpdateHP(int hp)
    {
        _playerHP.text = $"x {hp}";
    }
}