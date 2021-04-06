using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossSpawnMinionScript : Enemy
{
    public NavMeshAgent agent;

    private enum State
    {
        ChaseTarget,
        AttackTarget,
        ShootingTarget,
    }

    [SerializeField] private float maxHealth;
    [SerializeField] private float timeBetweenAttacks;

    [SerializeField] private GameObject hpBar_prefab;
    private GameObject hpBar;
    private Image hpBarFull;

    [SerializeField] private GameObject damageCounter_prefab;
    [SerializeField] private GameObject enemyAlert_prefab;
    private List<GameObject> uiList = new List<GameObject>();

    [System.Serializable]
    private class LootTuple
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

    [SerializeField] private AudioSource audioSourceDamage;
    [SerializeField] private AudioClip[] stabSounds;
    [SerializeField] private AudioSource audioSourceAttack;
    [SerializeField] private AudioClip alertSound;
    [SerializeField] private AudioClip attackSound;

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
        state = State.ChaseTarget;
        health = maxHealth;
    }

    private void Start()
    {
        GameObject attackRadius = gameObject.transform.Find("AttackRadius").gameObject;
        refScript = attackRadius.GetComponent<EnemyAttack>();
        animator = GetComponentInChildren<Animator>();
        SetupFlash();
        SetupLoot();

        // Make sure player exists (finished loading) before running these
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = player.GetComponentInChildren<Camera>();
        SetupHpBar();
        SetupUI(Instantiate(enemyAlert_prefab));
        audioSourceAttack.clip = alertSound;
        audioSourceAttack.Play();
    }

    private void SetupFlash()
    {
        rend = GetComponentInChildren<Renderer>();
        originalColors = new Color[rend.materials.Length];
        for (int i = 0; i < rend.materials.Length; i++)
        {
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


    public override void HandleHit(float damage)
    {
        this.health -= damage;
        hpBarFull.fillAmount = health / maxHealth;
        StartCoroutine(FlashOnDamage());
        SpawnDamageCounter(damage);
        audioSourceDamage.clip = stabSounds[Random.Range(0, stabSounds.Length)];
        audioSourceDamage.Play();

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
        for (var i = 0; i < rend.materials.Length; i++)
        {
            rend.materials[i].color = onDamageColor;
        }

        float duration = 0.1f;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }

        for (var i = 0; i < rend.materials.Length; i++)
        {
            rend.materials[i].color = originalColors[i];
        }
    }

    private void DropLoot()
    {
        Instantiate(lootDropped, transform.position, Quaternion.identity);
    }

    public void attackPlayerStart()
    {
        canMoveDuringAttack = false;
        refScript.attackPlayerStart();
    }

    public void attackPlayerDealDamage()
    {
        refScript.attackPlayerDealDamage();
        audioSourceAttack.clip = attackSound;
        audioSourceAttack.Play();
    }


    public void attackPlayerEnd()
    {
        canMoveDuringAttack = true;
        refScript.attackPlayerEnd();
    }
}