using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggplantAttack : EnemyAttack
{
    private GameObject attackRadius;
    private SpriteRenderer attackSprite;
    private SphereCollider attackCollider;
    private DungeonPlayerHealth healthScript;
    private bool canDealDamage;
    private void Awake()
    {
        attackSprite = gameObject.GetComponent<SpriteRenderer>();
        attackCollider = gameObject.GetComponent<SphereCollider>();
        canDealDamage = false;
    }

    public override void attackPlayerStart()
    {
        attackSprite.enabled = true;
        attackCollider.enabled = true;
    }

    public override void attackPlayerDealDamage()
    {
        canDealDamage = true;
    }


    public override void attackPlayerEnd()
    {
        attackSprite.enabled = false;
        attackCollider.enabled = false;
        canDealDamage = false;
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(canDealDamage);
        if (canDealDamage)
        {

            if (healthScript != null)
            {
                bool hitSuccess = healthScript.HandleHit(attackDamage);
                if (hitSuccess)
                {
                    healthScript.KnockbackPlayer(transform.position);
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        DungeonPlayerHealth target = other.GetComponent<DungeonPlayerHealth>();
        if (target != null)
        {
            healthScript = target;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited");

        DungeonPlayerHealth target = other.GetComponent<DungeonPlayerHealth>();
        if (target != null)
        {
            healthScript = null;
        }
    }

}
