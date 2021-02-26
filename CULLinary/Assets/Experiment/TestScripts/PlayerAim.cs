using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Texture2D reticle;

    private Animator animator;
    private GameObject playerBody;
    private bool isAiming = false;

    // UI
    private Vector2 cursorHotspot;
    private LineRenderer LR;

    // Constants
    private const float MAX_DIST_CAM_TO_GROUND = 100f;
    private const float LINE_HEIGHT_FROM_GROUND = 0.2f;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerBody = GameObject.FindWithTag("PlayerBody");
        cursorHotspot = new Vector2(reticle.width/2, reticle.height/2);
        LR = GetComponent<LineRenderer>();
        LR.positionCount = 0;
    }
    private void Update()
    {
        Aim();
    }

    private void Aim()
    {
        if (Input.GetMouseButton(1))
        {
            isAiming = true;
            animator.SetBool("isAim", true);

            Cursor.SetCursor(reticle, cursorHotspot, CursorMode.Auto);

            // Draw line from player to mouse
            LR.positionCount = 2;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, MAX_DIST_CAM_TO_GROUND, 1 << LayerMask.NameToLayer("Ground")))
            {
                playerBody.transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
                LR.SetPosition(0, new Vector3(transform.position.x, LINE_HEIGHT_FROM_GROUND, transform.position.z));
                LR.SetPosition(1, new Vector3(hit.point.x, LINE_HEIGHT_FROM_GROUND, hit.point.z));
            }
        }
        else
        {
            isAiming = false;
            animator.SetBool("isAim", false);

            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            LR.positionCount = 0;
        }
    }

    public bool GetIsAiming()
    {
        return this.isAiming;
    } 

    private void FixedUpdate()
    {

    }
}
