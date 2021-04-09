using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRangedAttack : EnemyAttack
{
    [SerializeField] private Transform throwObject;
    [SerializeField] private Material lineMaterial;
    private DungeonPlayerHealth healthScript;
    private bool canDealDamage;
    private Transform player;
    private bool projectAttack = false;
    private float startingAngle;
    private float fov = 90f;
    private float viewDistance = 50f;
    private List<LineRenderer> listOfRenderers;
    private List<Vector3> firePositions;
    private const float LINE_HEIGHT_FROM_GROUND = 0.1f;
    private int rayCount = 13;
    private int offsetRay = 0;
 
    private void Awake()
    {
        canDealDamage = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        listOfRenderers = new List<LineRenderer>();
        for (int i = 0; i < rayCount; i++)
        {
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
        activateStage1();
    }

    private void throwCorn(Vector3 sourcePosition, Vector3 targetPosition)
    {
        Transform cornTransform = Instantiate(throwObject, sourcePosition, Quaternion.identity);
        cornTransform.GetComponent<EnemyProjectile>().Setup(sourcePosition, targetPosition);
    }

    public void activateStage1()
    {
        rayCount = 5;
        offsetRay = 2;
    }

    public void activateStage2()
    {
        rayCount = 9;
        offsetRay = 4;
    }

    public void activateStage3()
    {
        fov = 150;
        rayCount = 13;
        offsetRay = 6;
    }
    private void Update()
    {
        if (projectAttack)
        {
            Vector3 playerDirection = (player.transform.position - transform.position).normalized;
            float angleIncrease = fov / rayCount;
            float angle = angleIncrease * offsetRay * -1;
            int layerMask = LayerMask.GetMask("Environment");
            Vector3 finalDirection;
            firePositions = new List<Vector3>();
            for (int i = 0; i < rayCount; i++)
            {
                finalDirection = Quaternion.Euler(0, angle, 0) * playerDirection;
                finalDirection.y = LINE_HEIGHT_FROM_GROUND;
                RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer
                Vector3 sourcePosition;
                Vector3 targetPosition;
                sourcePosition = new Vector3(transform.position.x, LINE_HEIGHT_FROM_GROUND, transform.position.z);
                if (Physics.Raycast(sourcePosition, finalDirection, out hit, viewDistance))
                {
                    LineRenderer lRend = listOfRenderers[i];
                    targetPosition = new Vector3(hit.point.x, LINE_HEIGHT_FROM_GROUND, hit.point.z);
                    lRend.SetPosition(0, sourcePosition);
                    lRend.SetPosition(1, targetPosition);
                }
                else
                {
                    LineRenderer lRend = listOfRenderers[i];
                    targetPosition = finalDirection * viewDistance + sourcePosition;
                    targetPosition.y = LINE_HEIGHT_FROM_GROUND;
                    lRend.SetPosition(0, sourcePosition);
                    lRend.SetPosition(1, targetPosition);
                }
                firePositions.Add(targetPosition);
                angle += angleIncrease;
            }
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
    }
    public void attackPlayerStartFlashing()
    {
        StartCoroutine("prepareToFire");
    }

    public override void attackPlayerDealDamage()
    {
        projectAttack = false;
        canDealDamage = true;
        for (int i = 0; i < firePositions.Count; i++)
        {
            throwCorn(transform.position, firePositions[i]);
        }

    }


    public override void attackPlayerEnd()
    {
        //Destroy(selectionCircleActual.gameObject);
        canDealDamage = false;
        for (int i =  0; i < listOfRenderers.Count; i++)
        {
            listOfRenderers[i].enabled = false;
        }
        StopCoroutine("prepareToFire");

    }

}
