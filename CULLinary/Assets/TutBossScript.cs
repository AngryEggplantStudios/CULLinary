using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TutBossScript : MonoBehaviour
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
    [SerializeField] private GameObject endingBurgers;

    private GameObject hpBar;
    private Image hpBarFull;
    private float health;
    private Camera cam;
    Transform player;
    float originalY;
    float jawOriginalY;
    private List<GameObject> uiList = new List<GameObject>();
    private float elapsed = 0.0f;

    private State state;
    private Vector3 localPosition;
    private Vector3 localFinalPosition;
    public int interpolationFramesCount = 120; // Number of frames to completely interpolate between the 2 positions
    int elapsedFrames = 0;

    //booleans to check if coroutine is running
    private bool openingMouth = true;
    private bool idleCooldownRunning = false;
    private bool coroutineMeleeRunning = false;

    private enum State
    {
        Idle,
        MeleeAttack,
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = player.GetComponentInChildren<Camera>();
        originalY = transform.position.y;
        jawOriginalY = lowerJaw.localPosition.y;
        health = maxHealth;
        //SetupHpBar();
        elapsed = 0.0f;
        state = State.Idle;
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


    void Update()
    {
        float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        elapsed += Time.deltaTime;

        switch (state)
        {
            default:
            case State.Idle:
                if (!openingMouth)
                {
                    lowerJaw.localPosition = Vector3.Lerp(localFinalPosition, localPosition, interpolationRatio);
                }
                else
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
                }
                else
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
        yield return new WaitForSeconds(0.5f);
        SetupUI(Instantiate(enemyAlert_prefab));
        yield return new WaitForSeconds(1);
        idleCooldownRunning = false;
        state = State.MeleeAttack;
        elapsedFrames = 0;
        openingMouth = true;
    }

    IEnumerator meleeCoroutine()
    {
        coroutineMeleeRunning = true;
        yield return new WaitForSeconds(5f);
        coroutineMeleeRunning = false;
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
