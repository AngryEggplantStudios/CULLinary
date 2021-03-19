using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoAttack : EnemyAttack
{
    private SphereCollider attackCollider;
    private DungeonPlayerHealth healthScript;
    private bool canDealDamage;
    public GameObject selectionCirclePrefab;
    public GameObject selectionCircleActual;


    private void Awake()
    {

        attackCollider = gameObject.GetComponent<SphereCollider>();
        canDealDamage = false;
    }

    public override void attackPlayerStart()
    {
        attackCollider.enabled = true;
    }

    public override void attackPlayerDealDamage()
    {
        canDealDamage = true;
    }


    public override void attackPlayerEnd()
    {
        attackCollider.enabled = false;
        canDealDamage = false;
    }

    private void OnTriggerStay(Collider other)
    {
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
        DungeonPlayerHealth target = other.GetComponent<DungeonPlayerHealth>();
        if (target != null)
        {
            healthScript = null;
        }
    }

}
