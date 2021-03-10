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
        ChaseTarget,
        ShootingTarget,
        GoingBackToStart,
    }

    [SerializeField] private float health;
    [SerializeField] private float distanceTriggered = 5f;
    [SerializeField] private float stopChase = 10f;
    [SerializeField] private Slider slider;


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
    private Animator animator;
    private State state;
    private Transform player;
    private GameObject lootDropped;

    private Renderer rend;
    private Color[] originalColors;
    private Color onDamageColor = Color.white;

    private void Awake()
    {
        state = State.Roaming;
    }

    private void Start()
    {
        startingPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        slider.maxValue = health;
        slider.value = health;
        rend = GetComponentInChildren<Renderer>();
        originalColors = new Color[rend.materials.Length];
        for (var i = 0; i < rend.materials.Length; i++) {
            originalColors[i] = rend.materials[i].color;
        }
        SetupLoot();
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

                    //Debug.Log("Chase");
                    //transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                    float directionVector = Vector3.Distance(transform.position, player.position);
                if (directionVector <= agent.stoppingDistance)
                {
                    // Target within attack range
                    animator.SetTrigger("attack");
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
            case State.GoingBackToStart:
                animator.SetBool("isMoving", true);
                float reachedPositionDistance = 1f;
                transform.LookAt(startingPosition);
                agent.SetDestination(startingPosition);
                if (Vector3.Distance(transform.position, startingPosition) < reachedPositionDistance)
                {
                    // Reached Start Position
                    state = State.Roaming;
                }
                break;
        }
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
        slider.value = health;
        StartCoroutine(FlashOnDamage());

        if (this.health <= 0)
        {
            DropLoot();
            Destroy(gameObject, 0.2f);
        }
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