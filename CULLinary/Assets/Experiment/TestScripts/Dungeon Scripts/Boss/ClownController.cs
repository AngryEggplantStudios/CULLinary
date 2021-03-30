using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject hpBar_prefab;
    [SerializeField] private GameObject damageCounter_prefab;
    [SerializeField] private GameObject enemyAlert_prefab;

    private GameObject hpBar;
    private Image hpBarFull;
    private float health;
    private Camera cam;
    Transform player;
    float originalY;
    float jawOriginalY;
    private List<GameObject> uiList = new List<GameObject>();

    private State state;
    private Vector3 localPosition;
    private Vector3 localFinalPosition;
    public int interpolationFramesCount = 120; // Number of frames to completely interpolate between the 2 positions
    int elapsedFrames = 0;
    private BossRangedAttack rangedAttackScript;
    private BossSpawnAttack spawnAttackScript;

    //booleans to check if coroutine is running
    private bool coroutineRangedRunning = false;
    private bool openingMouth = true;
    private bool idleCooldownRunning = false;
    private bool coroutineMeleeRunning = false;
    private bool coroutineSpawnRunning = false;

    private enum State
    {
        Roaming,
        Idle,
        RangedAttack,
        MeleeAttack,
        SpawnAttack
    }

    void Start()
    {
        state = State.Idle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = player.GetComponentInChildren<Camera>();
        originalY = transform.position.y;
        jawOriginalY = lowerJaw.localPosition.y;
        localPosition = lowerJaw.localPosition;
        //final position of the mouth when fully open
        localFinalPosition = new Vector3(localPosition.x, -0.045f, localPosition.z);
        rangedAttackScript = gameObject.transform.GetComponentInChildren<BossRangedAttack>();
        spawnAttackScript = gameObject.transform.GetComponent<BossSpawnAttack>();
        health = maxHealth;
        SetupHpBar();
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

    private void SpawnDamageCounter(float damage)
    {
        GameObject damageCounter = Instantiate(damageCounter_prefab);
        damageCounter.transform.GetComponentInChildren<Text>().text = damage.ToString();
        SetupUI(damageCounter);
    }

    public void HandleHit(float damage)
    {
        this.health -= damage;
        hpBarFull.fillAmount = health / maxHealth;
        SpawnDamageCounter(damage);

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
            case State.MeleeAttack:
                if (distanceToPlayer > lookingDistance)
                {
                    slowlyLookAt(player);
                }

                if (distanceToPlayer > stoppingDistance)
                {
                    moveForward();
                    leftFoot.meleeAttackEnd();
                    rightFoot.meleeAttackEnd();
                }

                if (!coroutineMeleeRunning)
                {
                    StartCoroutine("meleeCoroutine");
                }
                if (distanceToPlayer < meleeRange)
                {
                    leftFoot.meleeAttackStart();
                    rightFoot.meleeAttackStart();
                    stepOn(player);
                } else
                {
                    leftFoot.meleeAttackEnd();
                    rightFoot.meleeAttackEnd();
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
            case State.SpawnAttack:
                if (!coroutineSpawnRunning)
                {
                    StartCoroutine("spawnCoroutine");
                }
                break;

        }
        if (elapsedFrames != interpolationFramesCount)
        {
            elapsedFrames = (elapsedFrames + 1);
        }
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

    IEnumerator idleCooldownCoroutine()
    {
        idleCooldownRunning = true;
        yield return new WaitForSeconds(4);
        idleCooldownRunning = false;
        //state = State.RangedAttack;
        state = State.MeleeAttack;
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

    IEnumerator meleeCoroutine()
    {
        coroutineMeleeRunning = true;
        yield return new WaitForSeconds(5f);
        coroutineMeleeRunning = false;
        state = State.Idle;
    }

    IEnumerator spawnCoroutine()
    {
        coroutineSpawnRunning = true;
        spawnAttackScript.spawnMobs();
        yield return new WaitForSeconds(5f);
        coroutineSpawnRunning = false;
        state = State.Idle;
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
