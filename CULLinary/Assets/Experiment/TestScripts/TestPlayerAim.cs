using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerAim : GenericCharacterBehaviour
{
  [SerializeField] private float turnSpeed;
  [SerializeField] private Camera mainCamera;
  [SerializeField] private Transform cameraLookAt;
  [SerializeField] private GameObject reticle;
  [SerializeField] private Cinemachine.AxisState xAxis;
  [SerializeField] private Cinemachine.AxisState yAxis;

  public GenericCharacterBehaviour[] suppressedBh;
  private Animator animator;
  override public void InvokeBehaviour()
  {
    foreach (GenericCharacterBehaviour charBh in suppressedBh)
    {
      if (charBh.GetIsInvoked())
      {
        return;
      }
    }
    if (Input.GetMouseButton(1))
    {
      this.SetIsInvoked(true);
      reticle.SetActive(true);
      animator.SetBool("isAim", true);

    }
    else
    {
      this.SetIsInvoked(false);
      reticle.SetActive(false);
      animator.SetBool("isAim", false);
    }
  }

  void Start()
  {
    animator = GetComponentInChildren<Animator>();
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