using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerSlash : MonoBehaviour
{
    private Animator animator;
    private DungeonPlayerMelee dungeonPlayerAttack;
    
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

    }
    private void Awake()
    {
        dungeonPlayerAttack = GetComponent<DungeonPlayerMelee>();
        dungeonPlayerAttack.OnPlayerMelee += Slash;
        dungeonPlayerAttack.OnPlayerStop += Stop;
    }

    private void Slash()
    {
        animator.SetBool("isPunch", true);
    }

    private void Stop()
    {
        animator.SetBool("isPunch", false);
    }

    private void OnDestroy()
    {
        dungeonPlayerAttack.OnPlayerMelee -= Slash;
        dungeonPlayerAttack.OnPlayerStop -= Stop;
    }
}
