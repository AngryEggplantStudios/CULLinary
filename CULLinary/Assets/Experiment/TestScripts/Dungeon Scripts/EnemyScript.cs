using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    private enum State
    {
        Roaming,
        ChaseTarget,
        ShootingTarget,
        GoingBackToStart,
    }

    [SerializeField] private float maxHealth;
    [SerializeField] private float distanceTriggered = 5f;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private float stopChase = 10f;

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

    private Vector3 startingPosition;
    private Vector3 roamPosition;
    private float nextShootTime;
    private float dist;
    private float health;
    private Animator animator;
    private Camera cam;
    private State state;
    private Transform player;
    private GameObject lootDropped;

    private Renderer rend;
    private Color[] originalColors;
    private Color onDamageColor = Color.white;

    private void Awake()
    {
        state = State.Roaming;
        health = maxHealth;
    }

    private void Start()
    {
        startingPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = player.GetComponentInChildren<Camera>();
        animator = GetComponentInChildren<Animator>();
        
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

    private void Update()
    {
        animator.ResetTrigger("attack");

        switch (state)
        {
            default:
            case State.Roaming:
                animator.SetBool("isMoving", false);
                FindTarget();
                break;
            case State.ChaseTarget:
                animator.SetBool("isMoving", true);
                transform.LookAt(player);
                if (Vector3.Distance(transform.position, player.position) < attackRange)
                {
                    // Target within attack range
                    animator.SetTrigger("attack");
                    //Debug.Log("Attack Player");
                    // Add new state to attack player
                }
                else
                {
                    //Debug.Log("Chase");
                    transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                }

                if (Vector3.Distance(transform.position, player.position) > stopChase)
                {
                    // Too far, stop chasing
                    state = State.GoingBackToStart;
                }
                break;
            case State.GoingBackToStart:
                animator.SetBool("isMoving", true);
                float reachedPositionDistance = 1f;
                transform.LookAt(startingPosition);
                transform.position = Vector3.MoveTowards(transform.position, startingPosition, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, startingPosition) < reachedPositionDistance)
                {
                    // Reached Start Position
                    state = State.Roaming;
                }
                break;
        }

        // Set HP bar to current position
        hpBar.transform.position = cam.WorldToScreenPoint(transform.position);
    }

    private void FindTarget()
    {
        dist = Vector3.Distance(player.position, transform.position);

        if (dist <= distanceTriggered)
        {
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
}