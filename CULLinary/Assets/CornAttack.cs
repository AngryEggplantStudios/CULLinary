using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornAttack : EnemyAttack
{
    [SerializeField] private Transform throwObject;
    [SerializeField] private Material lineMaterial;
    private SphereCollider attackCollider;
    private DungeonPlayerHealth healthScript;
    private bool canDealDamage;
    private Transform player;

    private int rayCount = 1;
    private List<LineRenderer> listOfRenderers;
    private List<Vector3> firePositions;
    private const float LINE_HEIGHT_FROM_GROUND = 0.09f;
    private bool projectAttack = false;
    private float viewDistance = 50f;
    private void Awake()
    {

        attackCollider = gameObject.GetComponent<SphereCollider>();
        canDealDamage = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        listOfRenderers = new List<LineRenderer>();
        GameObject gameObjectChild = new GameObject();
        gameObjectChild.transform.parent = gameObject.transform;
        LineRenderer lRend = gameObjectChild.AddComponent<LineRenderer>();
        lRend.positionCount = 2;
        lRend.startWidth = 0.01f;
        lRend.endWidth = 0.01f;
        lRend.enabled = false;
        lRend.material = lineMaterial;
        lRend.SetColors(Color.red, Color.red);
        listOfRenderers.Add(lRend);
    }

    private void Update()
    {
        if (projectAttack)
        {
            Vector3 playerDirection = (player.transform.position - transform.position).normalized;
            float angle = 0;
            int layerMask = LayerMask.GetMask("Environment");
            Vector3 finalDirection;
            firePositions = new List<Vector3>();
            finalDirection = Quaternion.Euler(0, angle, 0) * playerDirection;
            finalDirection.y = LINE_HEIGHT_FROM_GROUND;
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            Vector3 sourcePosition;
            Vector3 targetPosition;
            sourcePosition = new Vector3(transform.position.x, LINE_HEIGHT_FROM_GROUND, transform.position.z);
            if (Physics.Raycast(sourcePosition, finalDirection, out hit, viewDistance))
            {
                LineRenderer lRend = listOfRenderers[0];
                targetPosition = new Vector3(hit.point.x, LINE_HEIGHT_FROM_GROUND, hit.point.z);
                lRend.SetPosition(0, sourcePosition);
                lRend.SetPosition(1, targetPosition);
            }
            else
            {
                LineRenderer lRend = listOfRenderers[0];
                targetPosition = finalDirection * viewDistance + sourcePosition;
                targetPosition.y = LINE_HEIGHT_FROM_GROUND;
                lRend.SetPosition(0, sourcePosition);
                lRend.SetPosition(1, targetPosition);
            }
            firePositions.Add(targetPosition);
            
        }


    }
    private IEnumerator prepareToFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < rayCount; i++)
            {
                listOfRenderers[i].enabled = !(listOfRenderers[i].enabled);
            }
        }

    }
    public override void attackPlayerStart()
    {
        //this.selectionCircleActual = Instantiate(this.selectionCirclePrefab);
        //this.selectionCircleActual.transform.SetParent(this.transform, false);
        //this.selectionCircleActual.transform.eulerAngles = new Vector3(90, 0, 0);
        //attackCollider.enabled = true;
        projectAttack = true;
        for (int i = 0; i < rayCount; i++)
        {
            listOfRenderers[i].enabled = true;
        }
        StartCoroutine("prepareToFire");
    }


    private void throwCorn(Vector3 sourcePosition, Vector3 targetPosition)
    {
        Transform cornTransform = Instantiate(throwObject, sourcePosition, Quaternion.identity);
        cornTransform.GetComponent<EnemyProjectile>().Setup(sourcePosition, targetPosition);
    }

    public override void attackPlayerDealDamage()
    {
        projectAttack = false;
        canDealDamage = true;
        for (int i = 0; i < firePositions.Count; i++)
        {
            throwCorn(transform.position, firePositions[i]);
        }
        for (int i = 0; i < listOfRenderers.Count; i++)
        {
            listOfRenderers[i].enabled = false;
        }
        StopCoroutine("prepareToFire");
    }


    public override void attackPlayerEnd()
    {
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
