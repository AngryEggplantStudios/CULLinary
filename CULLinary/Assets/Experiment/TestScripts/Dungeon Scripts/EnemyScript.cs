using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyScript : Enemy
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
    [SerializeField] private float distanceTriggered;
    [SerializeField] private float stopChase;
    [SerializeField] private float wanderTimer;
    [SerializeField] private float idleTimer;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float collideDamage;

    [SerializeField] private GameObject hpBar_prefab;
    private GameObject hpBar;
    private Image hpBarFull;

    [SerializeField] private GameObject damageCounter_prefab;
    [SerializeField] private GameObject enemyAlert_prefab;
    private List<GameObject> uiList = new List<GameObject>();

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
    [SerializeField] private float wanderRadius;
    private Vector3 startingPosition;
    private Vector3 roamPosition;
    private float timer;
    private float goingBackToStartTimer;
    private float dist;
    private float health;
    private Animator animator;
    private Camera cam;
    private State state;
    private Transform player;
    private GameObject lootDropped;
    private EnemyAttack refScript;
    private bool canAttack = true;
    private Renderer rend;
    private Color[] originalColors;
    private Color onDamageColor = Color.white;
    private bool canMoveDuringAttack = true;

    private void Awake()
    {
        state = State.Idle;
        health = maxHealth;
    }

    private void Start()
    {
        startingPosition = transform.position;
        GameObject attackRadius = gameObject.transform.Find("AttackRadius").gameObject;
        refScript = attackRadius.GetComponent <EnemyAttack>();
        animator = GetComponentInChildren<Animator>();
        timer = wanderTimer;
        goingBackToStartTimer = 0;
        SetupFlash();
        SetupLoot();

        // Make sure player exists (finished loading) before running these
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = player.GetComponentInChildren<Camera>();
        SetupHpBar();
        Debug.Log("Finish start");
    }

    private void SetupFlash()
    {
        rend = GetComponentInChildren<Renderer>();
        originalColors = new Color[rend.materials.Length];
        for (int i = 0; i < rend.materials.Length; i++) {
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
        hpBarFull = hpBar.transform.Find("hpBar_full").gameObject.GetComponent<Image>();
        SetupUI(hpBar);
    }

    private void SetupUI(GameObject ui)
    {
        ui.transform.SetParent(GameObject.FindObjectOfType<InventoryUI>().transform);
        ui.transform.position = cam.WorldToScreenPoint(transform.position);
        uiList.Add(ui);
    }

    private void Update()
    {
        Vector3 playerPositionWithoutYOffset = new Vector3(player.position.x, transform.position.y, player.position.z);
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
                    Vector3 newPos = RandomNavSphere(startingPosition, wanderRadius, -1);
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
                if (timer >= wanderTimer || distanceToFinalPosition.magnitude < 0.5f)
                {
                    timer = 0;
                    state = State.Idle;
                }
                break;
            case State.ChaseTarget:
                animator.SetBool("isMoving", true);
                directionVector = Vector3.Distance(transform.position, playerPositionWithoutYOffset);
                if (directionVector <= agent.stoppingDistance)
                {
                    transform.LookAt(playerPositionWithoutYOffset);
                    // Target within attack range
                    state = State.AttackTarget;
                    // Add new state to attack player
                }
                else
                {
                    agent.SetDestination(playerPositionWithoutYOffset);

                }

                if (Vector3.Distance(transform.position, player.position) > stopChase + 0.1f)
                {
                    // Too far, stop chasing
                    state = State.GoingBackToStart;
                }
                break;
            case State.AttackTarget:
                transform.LookAt(playerPositionWithoutYOffset);
                animator.SetBool("isMoving", false);
                animator.ResetTrigger("attack");
                if (canAttack == true)
                {
                    animator.SetTrigger("attack");
                    canAttack = false;
                    StartCoroutine(DelayFire());
                }
                directionVector = Vector3.Distance(transform.position, playerPositionWithoutYOffset);
                if (directionVector > agent.stoppingDistance && canMoveDuringAttack)
                {
                    // Target within attack range
                    state = State.ChaseTarget;
                }

                break;
            case State.GoingBackToStart:
                goingBackToStartTimer += Time.deltaTime;
                animator.SetBool("isMoving", true);
                float reachedPositionDistance = 1.0f;
                transform.LookAt(startingPosition);
                agent.SetDestination(startingPosition);
                if (Vector3.Distance(transform.position, startingPosition) <= reachedPositionDistance || goingBackToStartTimer > 4.0f)
                {
                    // Reached Start Position
                    animator.SetBool("isMoving", false);
                    state = State.Idle;
                    goingBackToStartTimer = 0;
                }
                break;
        }

        // Set UI to current position
        Vector2 screenPos = cam.WorldToScreenPoint(transform.position);
        if (screenPos != Vector2.zero)
        {
            foreach (GameObject ui in uiList)
            {
                if (ui != null)
                {
                    ui.transform.position = screenPos;
                }
                else
                {
                    uiList.Remove(null);
                }
            }
        }

        
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
            
            SetupUI(Instantiate(enemyAlert_prefab));
        }
    }

    public override void HandleHit(float damage)
    {
        this.health -= damage;
        hpBarFull.fillAmount = health/maxHealth;
        StartCoroutine(FlashOnDamage());
        SpawnDamageCounter(damage);

        if (this.health <= 0)
        {
            Die();
        }
    }

    private void SpawnDamageCounter(float damage)
    {
        GameObject damageCounter = Instantiate(damageCounter_prefab);
        damageCounter.transform.GetComponentInChildren<Text>().text = damage.ToString();
        SetupUI(damageCounter);
    }

    private void Die()
    {
        DropLoot();
        Destroy(hpBar);
        Destroy(gameObject);
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
        while ((randDirection - origin).magnitude < 5.0f)
        {
            randPos = Random.insideUnitCircle * dist;
            randDirection = new Vector3(randPos.x, transform.position.y, randPos.y);
        }
        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
    public void attackPlayerStart()
    {
        canMoveDuringAttack = false;
        refScript.attackPlayerStart();
    }

    public void attackPlayerDealDamage()
    {
        refScript.attackPlayerDealDamage();
    }


    public void attackPlayerEnd()
    {
        canMoveDuringAttack = true;
        refScript.attackPlayerEnd();
    }
}