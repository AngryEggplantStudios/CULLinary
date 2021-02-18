using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
  [SerializeField] private float mouseSensitivity;

  private void Start()
  {
    Cursor.lockState = CursorLockMode.Locked;
  }
  private void Update()
  {
    Rotate();
  }

  private void Rotate()
  {
    //float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
    //transform.Rotate(Vector3.up, mouseX);
  }

}
