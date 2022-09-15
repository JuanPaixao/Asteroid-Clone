using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosionBehaviour : MonoBehaviour
{
    [Header("Local Reference"), SerializeField]
    private EnemyBehaviour _enemyBehaviour;

    public void ExplosionCallback()
    {
        _enemyBehaviour.ReturnToPool();
    }
    
}