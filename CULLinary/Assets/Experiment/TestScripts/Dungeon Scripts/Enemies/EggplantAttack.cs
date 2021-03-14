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
    public GameObject selectionCirclePrefab;
    public GameObject selectionCircleActual;


    private void Awake()
    {

        attackSprite = gameObject.GetComponent<SpriteRenderer>();
        attackCollider = gameObject.GetComponent<SphereCollider>();
        canDealDamage = false;
    }

    public override void attackPlayerStart()
    {
        //attackSprite.enabled = true;
        this.selectionCircleActual = Instantiate(this.selectionCirclePrefab);
        this.selectionCircleActual.transform.SetParent(this.transform, false);
        this.selectionCircleActual.transform.eulerAngles = new Vector3(90, 0, 0);
        attackCollider.enabled = true;
    }

    public override void attackPlayerDealDamage()
    {
        canDealDamage = true;
    }


    public override void attackPlayerEnd()
    {
        //attackSprite.enabled = false;
        Destroy(selectionCircleActual.gameObject);
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
