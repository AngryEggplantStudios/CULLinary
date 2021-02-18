using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
  [SerializeField] Animator animator;

  private CharacterController controller;
  private Vector3 direction;
  private Vector3 normalizedDirection;
  private Vector3 moveDirection;

  private void Start()
  {
    animator = GetComponent<Animator>();
  }

  private void Update()
  {
    float moveVertical = Input.GetAxisRaw("Vertical");
    float moveHorizontal = Input.GetAxisRaw("Horizontal");

    direction = new Vector3(moveHorizontal, 0.0f, moveVertical);
    normalizedDirection = direction.normalized;
  }
}
