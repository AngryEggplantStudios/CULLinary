using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerAim : MonoBehaviour
{
    [SerializeField] private Texture2D reticle;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject gameOver;
    private Animator animator;
    private DungeonPlayerRange dungeonPlayerRange;

    // Perform raycasting for the throwing knife
    public DungeonRaycaster raycastLayer;

    //Projectile Class
    public delegate void PlayerProjectileDelegate(Vector3 sourcePosition, Vector3 targetPosition);
    public event PlayerProjectileDelegate OnPlayerShoot;

    //UI
    private Vector2 cursorHotspot;
    private LineRenderer lineRenderer;
    public Animator cooldownAnimator;

    //Defaults
    private const float MAX_DIST_CAM_TO_GROUND = 100f;
    private const float LINE_HEIGHT_FROM_GROUND = 0.01f;

    private Vector3 targetPosition = new Vector3();
    private Vector3 sourcePosition = new Vector3();
    private Vector3 lookVector = new Vector3();
    private bool targetFound = false;
    private bool canShoot = true;

    //Audio
    [SerializeField] private AudioSource audioSourceAttack;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioSource audioSourceCdRefreshed;

    private void Start()
    {
        canShoot = true;
        animator = GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();

        cursorHotspot = new Vector2(reticle.width / 2, reticle.height / 2);
        lineRenderer.positionCount = 0;
    }
    private void Awake()
    {
        dungeonPlayerRange = GetComponent<DungeonPlayerRange>();
        dungeonPlayerRange.OnPlayerAim += Aim;
        dungeonPlayerRange.OnPlayerStop += StopAim;

    }

    private void Aim()
    {
        if (!gameOver.activeSelf)
        {
            animator.SetBool("isAim", true);

            //Enable reticle
            Cursor.SetCursor(reticle, cursorHotspot, CursorMode.Auto);

            //Draw line from player to mouse
            lineRenderer.positionCount = 2;
            
            RaycastHit hit;
            bool hitGround;

            if (raycastLayer)
            {
                hitGround = raycastLayer.RaycastMouse(out hit, MAX_DIST_CAM_TO_GROUND);
            }
            else
            {
                Ray ray =Camera.main.ScreenPointToRay(Input.mousePosition);
                hitGround = Physics.Raycast(ray, out hit, MAX_DIST_CAM_TO_GROUND, LayerMask.NameToLayer("Ground"));
            }

            if (hitGround) {
                this.lookVector = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                this.sourcePosition = new Vector3(transform.position.x, LINE_HEIGHT_FROM_GROUND, transform.position.z);
                this.targetPosition = new Vector3(hit.point.x, LINE_HEIGHT_FROM_GROUND, hit.point.z);

                playerBody.transform.LookAt(this.lookVector);
                lineRenderer.SetPosition(0, this.sourcePosition);
                lineRenderer.SetPosition(1, this.targetPosition);
                targetFound = true;
            }
            else
            {
                Debug.Log("Not clicking on Ground during Ranged Attack");
                targetFound = false;
            }
        }
    }

    private void Update()
    {
        if (gameOver.activeSelf)
        {
            canShoot = false;
        }
        if (Input.GetMouseButtonUp(1) && targetFound && canShoot) {
            canShoot = false;
            OnPlayerShoot?.Invoke(sourcePosition, targetPosition);
            audioSourceAttack.clip = attackSound;
            audioSourceAttack.Play();
            StartCoroutine(DelayFire());
        }
    }

    private IEnumerator DelayFire()
    {
        cooldownAnimator.SetTrigger("StartCooldown");
        yield return new WaitForSeconds(1.0f);
        canShoot = true;
        audioSourceCdRefreshed.Play();
    }

    //Cleanup event to provoke
    private void StopAim()
    {
        animator.SetBool("isAim", false);

        // Disable reticle
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        // Delete line
        lineRenderer.positionCount = 0;
    }

    private void OnDestroy()
    {
        dungeonPlayerRange.OnPlayerAim -= Aim;
        dungeonPlayerRange.OnPlayerStop -= StopAim;
    }
    public void disableMovement()
    {
        canShoot = false;
        dungeonPlayerRange.OnPlayerAim -= Aim;
        dungeonPlayerRange.OnPlayerStop -= StopAim;
    }
    



}
