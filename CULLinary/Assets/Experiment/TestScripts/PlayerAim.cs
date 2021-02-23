using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 15;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform cameraLookAt;
    [SerializeField] private GameObject reticle;

    [SerializeField] private Cinemachine.AxisState xAxis;
    [SerializeField] private Cinemachine.AxisState yAxis;

    private Animator animator;
    private bool isAiming = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
            reticle.SetActive(true);
            animator.SetBool("isAim", true);
            animator.SetFloat("AimAngle", Vector3.Angle(Vector3.down, mainCamera.transform.forward)/180);
        }
        else
        {
            isAiming = false;
            reticle.SetActive(false);
            animator.SetBool("isAim", false);
        }
    }

    public bool GetIsAiming()
    {
        return this.isAiming;
    } 

    private void FixedUpdate()
    {
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);
        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.deltaTime);
    }
}
