using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private enum State
    {
        Roaming,
        ChaseTarget,
        ShootingTarget,
        GoingBackToStart,
    }

    [SerializeField] private float health;
    [SerializeField] private GameObject loot;

    private Vector3 startingPosition;
    private Vector3 roamPosition;
    private float nextShootTime;
    private float dist;
    private Animator animator;
    private State state;
    private Transform player;
    public float distanceTriggered = 5f;
    public float moveSpeed;
    public float attackRange;
    public float stopChase = 10f;
    public GameObject drops;

    private void Awake()
    {
        state = State.Roaming;
    }

    private void Start()
    {
        startingPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
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
        Debug.Log("Current health: " + health);
        if (this.health <= 0)
        {
            DropLoot();
            Destroy(gameObject, 0.2f);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Eggplant died uwu");

    }

    private void DropLoot()
    {
        Instantiate(loot, transform.position, Quaternion.identity);
        /* Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity); */
    }

}