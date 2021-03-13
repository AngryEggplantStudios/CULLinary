using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public NavMeshAgent agent;

    private enum State
    {
        Roaming,
        Idle,
        ChaseTarget,
        AttackTarget,
        ShootingTarget,
        GoingBackToStart,
    }

    [SerializeField] private float maxHealth;
    [SerializeField] private float distanceTriggered = 5f;
    [SerializeField] private float stopChase = 10f;
    [SerializeField] private float wanderTimer;
    [SerializeField] private float idleTimer;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float collideDamage;


    [SerializeField] private GameObject hpBar_prefab;
    private GameObject hpBar;
    private Image hpBarFull;

    [System.Serializable] private class LootTuple
    {
        [SerializeField] private GameObject loot;
        [SerializeField] private int ratio;

        public LootTuple(GameObject loot, int ratio)
        {
            this.loot = loot;
            this.ratio = ratio;
        }

        public GameObject GetLoot()
        {
            return loot;
        }

        public int GetRatio()
        {
            return ratio;
        }
    }

    [SerializeField] private LootTuple[] lootTuples;
    private float wanderRadius = 15.0f;
    private Vector3 startingPosition;
    private Vector3 roamPosition;
    private float timer;
    private float attackTimer;
    private float nextShootTime;
    private float dist;
    private float health;
    private Animator animator;
    private Camera cam;
    private State state;
    private Transform player;
    private GameObject lootDropped;
    private DungeonPlayerHealth healthScript;
    private bool canAttack = true;
    private Renderer rend;
    private Color[] originalColors;
    private Color onDamageColor = Color.white;

    private void Awake()
    {
        state = State.Idle;
        health = maxHealth;
    }

    private void Start()
    {
        startingPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = player.GetComponentInChildren<Camera>();
        animator = GetComponentInChildren<Animator>();
        timer = wanderTimer;        
        SetupFlash();
        SetupLoot();
        SetupHpBar();
    }

    private void SetupFlash()
    {
        rend = GetComponentInChildren<Renderer>();
        originalColors = new Color[rend.materials.Length];
        for (var i = 0; i < rend.materials.Length; i++) {
            originalColors[i] = rend.materials[i].color;
        }
    }

    private void SetupLoot()
    {
        int currentWeight = 0;
        Dictionary<GameObject, int> dropTuples = new Dictionary<GameObject, int>();
        foreach (var loot in lootTuples)
        {
            currentWeight += loot.GetRatio();
            dropTuples.Add(loot.GetLoot(), currentWeight);
        }
        int randomWeight = Random.Range(1, currentWeight + 1);
        foreach (var tpl in dropTuples)
        {
            if (randomWeight <= tpl.Value)
            {
                lootDropped = tpl.Key;
                return;
            }
        }
        lootDropped = lootTuples[0].GetLoot();
        return;
    }

    private void SetupHpBar()
    {
        hpBar = Instantiate(hpBar_prefab);
        hpBar.transform.SetParent(GameObject.Find("UI").transform);
        hpBarFull = hpBar.transform.Find("hpBar_full").gameObject.GetComponent<Image>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (healthScript != null)
        {
            healthScript.HandleHit(collideDamage);
            Debug.Log("Collided!");
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

    private void Update()
    {
        float directionVector;
        switch (state)
        {
            default:
            case State.Idle:
                animator.SetBool("isMoving", false);
                timer += Time.deltaTime;
                FindTarget();
                if (timer >= idleTimer)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                    agent.SetDestination(newPos);
                    timer = 0;
                    state = State.Roaming;
                    roamPosition = newPos;
                }
                break;
            case State.Roaming:
                animator.SetBool("isMoving", true);
                timer += Time.deltaTime;
                FindTarget();
                Vector3 distanceToFinalPosition = transform.position - roamPosition;
                //without this the eggplant wandering will be buggy as it may be within the Navmesh Obstacles itself
                if (timer >= wanderTimer || distanceToFinalPosition.magnitude < 3.0f)
                {
                    timer = 0;
                    state = State.Idle;
                }
                break;
            case State.ChaseTarget:
                animator.SetBool("isMoving", true);

                    //Debug.Log("Chase");
                    //transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                directionVector = Vector3.Distance(transform.position, player.position);
                if (directionVector <= agent.stoppingDistance)
                {
                    transform.LookAt(player);
                    // Target within attack range
                    state = State.AttackTarget;
                    // Add new state to attack player
                } else
                {
                    agent.SetDestination(player.position);

                }

                if (Vector3.Distance(transform.position, player.position) > stopChase)
                {
                    // Too far, stop chasing
                    state = State.GoingBackToStart;
                }
                break;
            case State.AttackTarget:
                transform.LookAt(player);
                animator.ResetTrigger("attack");
                if (canAttack == true)
                {
                    Debug.Log("Attack");
                    animator.SetTrigger("attack");
                    canAttack = false;
                    StartCoroutine(DelayFire());
                }
                animator.SetBool("isMoving", false);
                directionVector = Vector3.Distance(transform.position, player.position);
                if (directionVector > agent.stoppingDistance)
                {
                    // Target within attack range
                    state = State.ChaseTarget;
                }

                break;
            case State.GoingBackToStart:
                animator.SetBool("isMoving", true);
                float reachedPositionDistance = 5f;
                transform.LookAt(startingPosition);
                agent.SetDestination(startingPosition);
                if (Vector3.Distance(transform.position, startingPosition) <= reachedPositionDistance)
                {
                    // Reached Start Position
                    state = State.Idle;
                }
                break;
        }

        // Set HP bar to current position
        hpBar.transform.position = cam.WorldToScreenPoint(transform.position);
    }

    private IEnumerator DelayFire()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        canAttack = true;
    }

    private void FindTarget()
    {
        dist = Vector3.Distance(player.position, transform.position);

        if (dist <= distanceTriggered)
        {
            timer = 0;
            state = State.ChaseTarget;
        }
    }

    public void HandleHit(float damage)
    {
        this.health -= damage;
        hpBarFull.fillAmount = health/maxHealth;
        StartCoroutine(FlashOnDamage());

        if (this.health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        DropLoot();
        Destroy(hpBar, 0.2f);
        Destroy(gameObject, 0.2f);
    }

    private IEnumerator FlashOnDamage()
    {
        for (var i = 0; i < rend.materials.Length; i++) {
            rend.materials[i].color = onDamageColor;
        }

        float duration = 0.1f;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }

        for (var i = 0; i < rend.materials.Length; i++) {
            rend.materials[i].color = originalColors[i];
        }
    }

    private void DropLoot()
    {
        Instantiate(lootDropped, transform.position, Quaternion.identity);
    }

    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector2 randPos = Random.insideUnitCircle * dist;
        Vector3 randDirection = new Vector3(randPos.x, transform.position.y, randPos.y);
        while ((randDirection - origin).magnitude < 7.0f)
        {
            randPos = Random.insideUnitCircle * dist;
            randDirection = new Vector3(randPos.x, transform.position.y, randPos.y);
        }
        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}