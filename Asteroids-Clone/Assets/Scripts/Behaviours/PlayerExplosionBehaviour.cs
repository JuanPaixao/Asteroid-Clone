using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExplosionBehaviour : MonoBehaviour
{
   public void PlayerExplosionCallback()
   {
      GameManager.Instance.RestartGame();
   }
}
