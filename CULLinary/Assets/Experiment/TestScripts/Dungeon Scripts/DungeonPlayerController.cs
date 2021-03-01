using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerController : MonoBehaviour
{    
    //Speed
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    //Cameras
    [SerializeField] private GameObject cam; //Main camera

    //Directions
    private Vector3 direction;
    private Vector3 normalizedDirection;
    private Vector3 moveDirection;

    //Defaults
    private float moveSpeed = 1.0f;
    private float turnSpeed = 10.0f;
    private Animator animator;
    private CharacterController controller;
    private PlayerAim playerAim;
    private PlayerRegularAttack playerRegularAttack;
    private GameObject playerBody;

    private void Start(){
        
    }

}
