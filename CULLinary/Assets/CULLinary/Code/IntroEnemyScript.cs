using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class IntroEnemyScript : Enemy
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
    [SerializeField] private float wanderTimer;
    [SerializeField] private float idleTimer;
    [SerializeField] private GameObject enemyAlert_prefab;
    private List<GameObject> uiList = new List<GameObject>();

    [SerializeField] private float wanderRadius;
    [SerializeField] private AudioSource audioSourceAttack;
    [SerializeField] private AudioClip alertSound;

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
    private EnemyAttack refScript;

    private bool alertOnce = false;


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
        // Make sure player exists (finished loading) before running these
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = player.GetComponentInChildren<Camera>();
    }

    public override void HandleHit(float damage)
    {
        Debug.Log("Method not implemented");
    }


    private void Update()
    {
        Vector3 playerPositionWithoutYOffset = new Vector3(player.position.x, transform.position.y, player.position.z);
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

    private void FindTarget()
    {
        dist = Vector3.Distance(player.position, transform.position);
        if (dist <= distanceTriggered && !alertOnce)
        {
            Alert();
            alertOnce = true;
        }
    }

    private void Alert()
    {
        timer = 0;
        state = State.ChaseTarget;
        
        //SetupUI(Instantiate(enemyAlert_prefab));
        audioSourceAttack.clip = alertSound;
        audioSourceAttack.Play();
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

}