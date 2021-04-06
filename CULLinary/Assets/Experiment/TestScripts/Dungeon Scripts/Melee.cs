using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        int damage = PlayerManager.playerData == null ? 20 : PlayerManager.playerData.GetMeleeDamage();
        if (Random.Range(0, 100) < PlayerManager.playerData.critRate)
        {
            damage *= 2;
        }
        Enemy target = collider.GetComponent<Enemy>();
        if (target != null)
        {
            target.HandleHit(damage);
        }
    }
}
