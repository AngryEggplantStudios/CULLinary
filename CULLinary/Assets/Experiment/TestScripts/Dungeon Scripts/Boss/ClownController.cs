using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2;
    [SerializeField] float turningSpeed = 100; // deg/s
    [SerializeField] float stoppingDistance = 1;
    [Tooltip("Distance to stop rotating")]
    [SerializeField] float lookingDistance = 0.3f;
    [SerializeField] float meleeRange = 1;

    [SerializeField] Transform lowerJaw;
    [SerializeField] IKFootSolver leftFoot;
    [SerializeField] IKFootSolver rightFoot;

    Transform player;
    float originalY;
    float jawOriginalY;

    private State state;
    private bool canMelee;
    private Vector3 originalPosition;
    private Vector3 localPosition;
    private Vector3 localFinalPosition;
    public int interpolationFramesCount = 120; // Number of frames to completely interpolate between the 2 positions
    int elapsedFrames = 0;
    private BossRangedAttack rangedAttackScript;
    private bool coroutineRangedRunning = false;
    private bool openingMouth = true;
    private bool idleCooldownRunning = false;

    private enum State
    {
        Roaming,
        Idle,
        RangedAttack,
        ChaseTarget,
        AttackTarget,
        ShootingTarget,
        GoingBackToStart,
    }

    void Start()
    {
        state = State.Roaming;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalY = transform.position.y;
        jawOriginalY = lowerJaw.localPosition.y;
        originalPosition = gameObject.transform.position;
        localPosition = lowerJaw.localPosition;
        localFinalPosition = new Vector3(localPosition.x, -0.045f, localPosition.z);
        rangedAttackScript = gameObject.transform.GetComponentInChildren<BossRangedAttack>();
    }

    void Update()
    {
        float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        switch (state)
        {
            default:
            case State.Idle:
                if (!openingMouth)
                {
                    lowerJaw.localPosition = Vector3.Lerp(localFinalPosition, localPosition, interpolationRatio);
                } else
                {
                    lowerJaw.localPosition = new Vector3(
                        lowerJaw.localPosition.x,
                        jawOriginalY - Mathf.Abs(Mathf.Sin(Time.fixedTime * Mathf.PI * 2) * 0.01f),
                        lowerJaw.localPosition.z);
                }
                if (!idleCooldownRunning)
                {
                    StartCoroutine("idleCooldownCoroutine");
                }
                transform.position = new Vector3(
                        transform.position.x,
                        originalY + Mathf.Sin(Time.fixedTime * Mathf.PI * 1) * 0.2f,
                        transform.position.z);
                break;
            case State.RangedAttack:
                if (distanceToPlayer > 0.2f)
                {
                    quicklyLookAt(player);
                }
                if (openingMouth)
                {
                    lowerJaw.localPosition = Vector3.Lerp(localPosition, localFinalPosition, interpolationRatio);
                } 
                if (!coroutineRangedRunning)
                {
                    StartCoroutine("rangedCoroutine");
                }
                break;
            case State.Roaming:
                if (distanceToPlayer > lookingDistance)
                {
                    slowlyLookAt(player);
                }

                if (distanceToPlayer > stoppingDistance)
                {
                    moveForward();
                }


                if (distanceToPlayer < meleeRange)
                {
                    stepOn(player);

                }

                // Bob head and jaw for demostration
                transform.position = new Vector3(
                        transform.position.x,
                        originalY + Mathf.Sin(Time.fixedTime * Mathf.PI * 1) * 0.2f,
                        transform.position.z);
                lowerJaw.localPosition = new Vector3(
                        lowerJaw.localPosition.x,
                        jawOriginalY - Mathf.Abs(Mathf.Sin(Time.fixedTime * Mathf.PI * 2) * 0.01f),
                        lowerJaw.localPosition.z);
                break;
        }
        if (elapsedFrames != interpolationFramesCount)
        {
            elapsedFrames = (elapsedFrames + 1);
        }
    }

    IEnumerator idleCooldownCoroutine()
    {
        idleCooldownRunning = true;
        yield return new WaitForSeconds(2);
        idleCooldownRunning = false;
        state = State.RangedAttack;
        elapsedFrames = 0;
        openingMouth = true;
    }

    IEnumerator rangedCoroutine()
    {
        coroutineRangedRunning = true;
        // suspend execution for 5 seconds
        rangedAttackScript.attackPlayerStart();
        yield return new WaitForSeconds(2f);
        rangedAttackScript.attackPlayerStartFlashing();
        yield return new WaitForSeconds(1f);
        rangedAttackScript.attackPlayerDealDamage();
        yield return new WaitForSeconds(0.5f);
        rangedAttackScript.attackPlayerEnd();
        elapsedFrames = 0;
        coroutineRangedRunning = false;
        state = State.Idle;
        openingMouth = false;
    }

    void slowlyLookAt(Transform targetPlayer)
    {
        Vector3 target = targetPlayer.position;
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(Quaternion.LookRotation(target - transform.position).eulerAngles),
            Time.deltaTime * turningSpeed);
    }

    void quicklyLookAt(Transform targetPlayer)
    {
        Vector3 target = targetPlayer.position;
        target.y = transform.position.y;
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(Quaternion.LookRotation(target - transform.position).eulerAngles),
            Time.deltaTime * turningSpeed * 5);
    }

    void moveForward()
    {
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    // both feet must be on the ground
    // target must be above ground and within range
    void stepOn(Transform target)
    {
        // Check if both feet are on the ground
        if (leftFoot.IsMoving() || rightFoot.IsMoving())
        {
            return;
        }

        // Find target on ground
        Ray ray = new Ray(target.position, Vector3.down);
        if (!Physics.Raycast(ray, out RaycastHit info, 100, 1 << LayerMask.NameToLayer("Ground")))
        {
            Debug.Log("stepOn() target " + target.position + " is not above the ground");
            return;
        }
        
        // Find closer foot and step
        if (Vector3.Distance(leftFoot.currentPosition, info.point) < Vector3.Distance(rightFoot.currentPosition, info.point))
        {
            leftFoot.SetTarget(info.point, info.normal);
        }
        else
        {
            rightFoot.SetTarget(info.point, info.normal);
        }
    }
}
