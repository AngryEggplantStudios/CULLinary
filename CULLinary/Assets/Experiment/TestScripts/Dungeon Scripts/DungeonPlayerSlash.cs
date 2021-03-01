using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DungeonPlayerSlash : MonoBehaviour
{
    private Animator animator;
    private DungeonPlayerAttack dungeonPlayerAttack;
    private UnityEvent onPlayerAttackEvent; 
    
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void Awake()
    {
        dungeonPlayerAttack = GetComponent<DungeonPlayerAttack>();
        onPlayerAttackEvent = dungeonPlayerAttack.GetOnPlayerAttack();
    }

    public void PlayerSlash()
    {
        animator.SetBool("isPunch", true);
        onPlayerAttackEvent.RemoveListener(PlayerSlash);
    }

    public void PlayerStop()
    {
        animator.SetBool("isPunch", false);
        onPlayerAttackEvent.RemoveListener(PlayerStop);
    }
}
