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
    // Whether to invert movement directions and keyboard mapping
    [SerializeField] private bool invertKeys = false;
    // Whether run is enabled
    [SerializeField] private bool canRun = true;

    //Directions
    private Vector3 direction;
    private Vector3 normalizedDirection;
    private Vector3 moveDirection;

    //Actions prevented
    private DungeonPlayerRange dungeonPlayerRange;
    private DungeonPlayerMelee dungeonPlayerMelee;
   
    private int directionMultiplier = 1;

    private void Start(){
        dungeonPlayerRange = GetComponent<DungeonPlayerRange>();
        dungeonPlayerMelee = GetComponent<DungeonPlayerMelee>();
        directionMultiplier = invertKeys ? -1 : 1;
    }

    private void Update()
    {
        //Get input
        float moveVertical = Input.GetAxisRaw("Vertical");
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        //Calculations
        direction = new Vector3(moveHorizontal, 0.0f, moveVertical);
        normalizedDirection = direction.normalized * directionMultiplier;
        float targetAngle = Mathf.Atan2(normalizedDirection.x, normalizedDirection.z) * Mathf.Rad2Deg;
        moveDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
        
        bool isRangeInvoked = dungeonPlayerRange ? dungeonPlayerRange.GetIsInvoking() : false;
        bool isMeleeInvoked = dungeonPlayerMelee ? dungeonPlayerMelee.GetIsInvoking() : false;
        if (!isRangeInvoked && !isMeleeInvoked)
        {
            this.SetIsInvoking(true);

            if (direction == Vector3.zero)
            {
                OnPlayerMove?.Invoke(moveDirection.normalized, 0.0f, 0.0f, false);
            }
            else
            {
                if (canRun && Input.GetKey(KeyCode.LeftShift))
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
