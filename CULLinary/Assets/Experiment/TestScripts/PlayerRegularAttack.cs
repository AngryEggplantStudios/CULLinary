using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRegularAttack : MonoBehaviour
{

    private Animator animator;
    private bool isRegularAttack = false;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Punch();
    }

    private void Punch()
    {
        if (Input.GetKey(KeyCode.F))
        {
            isRegularAttack = true;
            animator.SetBool("isPunch", true);
        }
        else
        {
            isRegularAttack = false;
            animator.SetBool("isPunch", false);
        }
    }

    public bool GetIsRegularAttack()
    {
        return this.isRegularAttack;
    }

}
