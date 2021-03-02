using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerAim : MonoBehaviour
{
    [SerializeField] private Texture2D reticle;
    [SerializeField] private GameObject playerBody;
    private Animator animator;
    private DungeonPlayerRange dungeonPlayerRange;

    //UI
    private Vector2 cursorHotspot;
    private LineRenderer lineRenderer;

    //Defaults
    private const float MAX_DIST_CAM_TO_GROUND = 100f;
    private const float LINE_HEIGHT_FROM_GROUND = 0.2f;


    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        lineRenderer = GetComponent<LineRenderer>();
        
        cursorHotspot = new Vector2(reticle.width/2, reticle.height/2);
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
        animator.SetBool("isAim", true);

        //Enable reticle
        Cursor.SetCursor(reticle, cursorHotspot, CursorMode.Auto);

        //Draw line from player to mouse
        lineRenderer.positionCount = 2;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, MAX_DIST_CAM_TO_GROUND, 1 << LayerMask.NameToLayer("Ground")))
            {
                Debug.Log("Hit!");
                playerBody.transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
                lineRenderer.SetPosition(0, new Vector3(transform.position.x, LINE_HEIGHT_FROM_GROUND, transform.position.z));
                lineRenderer.SetPosition(1, new Vector3(hit.point.x, LINE_HEIGHT_FROM_GROUND, hit.point.z));
            }

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



}
