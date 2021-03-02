using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerController : DungeonPlayerAction
{

    public delegate void PlayerMoveDelegate(Vector3 direction, float speed, float animValue, bool isMoving);
    public delegate void PlayerRotateDelegate(Vector3 direction, float speed);
    public event PlayerMoveDelegate OnPlayerMove;
    public event PlayerRotateDelegate OnPlayerRotate;

    //Speed
    [SerializeField] private float walkSpeed = 10.0f;
    [SerializeField] private float runSpeed = 20.0f;
    [SerializeField] private float turnSpeed = 10.0f;

    //Cameras
    [SerializeField] private GameObject cam;

    //Directions
    private Vector3 direction;
    private Vector3 normalizedDirection;
    private Vector3 moveDirection;

    //Actions prevented
    private DungeonPlayerRange dungeonPlayerRange;
    private DungeonPlayerMelee dungeonPlayerAttack;
   

    private void Start(){
        dungeonPlayerRange = GetComponent<DungeonPlayerRange>();
        dungeonPlayerAttack = GetComponent<DungeonPlayerMelee>();
    }

    private void Update()
    {
        //Get input
        float moveVertical = Input.GetAxisRaw("Vertical");
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        //Calculations
        direction = new Vector3(moveHorizontal, 0.0f, moveVertical);
        normalizedDirection = direction.normalized;
        float targetAngle = Mathf.Atan2(normalizedDirection.x, normalizedDirection.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
        moveDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
        
        if (!dungeonPlayerRange.GetIsInvoking() && !dungeonPlayerAttack.GetIsInvoking())
        {
            this.SetIsInvoking(true);

            if (direction == Vector3.zero)
            {
                OnPlayerMove?.Invoke(moveDirection.normalized, 0.0f, 0.0f, false);
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    OnPlayerMove?.Invoke(moveDirection.normalized, runSpeed, 1.0f, true);
                }
                else
                {
                    OnPlayerMove?.Invoke(moveDirection.normalized, walkSpeed, 0.5f, true);
                }
            }

            if (direction != Vector3.zero)
            {
                OnPlayerRotate?.Invoke(normalizedDirection, turnSpeed);
            }
        }
        else 
        {
            this.SetIsInvoking(false);
        }
    }

}
