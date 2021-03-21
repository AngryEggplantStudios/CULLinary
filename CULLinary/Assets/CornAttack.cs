using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornAttack : EnemyAttack
{
    [SerializeField] private Transform throwObject;
    private SphereCollider attackCollider;
    private DungeonPlayerHealth healthScript;
    private bool canDealDamage;
    private Transform player; 

    private void Awake()
    {

        attackCollider = gameObject.GetComponent<SphereCollider>();
        canDealDamage = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void throwCorn(Vector3 sourcePosition, Vector3 targetPosition)
    {
        Transform cornTransform = Instantiate(throwObject, sourcePosition, Quaternion.identity);
        cornTransform.GetComponent<EnemyProjectile>().Setup(sourcePosition, targetPosition);
    }

    public override void attackPlayerStart()
    {
        //this.selectionCircleActual = Instantiate(this.selectionCirclePrefab);
        //this.selectionCircleActual.transform.SetParent(this.transform, false);
        //this.selectionCircleActual.transform.eulerAngles = new Vector3(90, 0, 0);
        //attackCollider.enabled = true;
    }

    public override void attackPlayerDealDamage()
    {
        canDealDamage = true;
        throwCorn(transform.position, player.transform.position);
    }


    public override void attackPlayerEnd()
    {
        //Destroy(selectionCircleActual.gameObject);
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
