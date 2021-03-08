using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerSlash : MonoBehaviour
{
    private Animator animator;
    private DungeonPlayerMelee dungeonPlayerAttack;
    [SerializeField] private GameObject weapon;
    
    private void Start()
    {
        animator = GetComponent<Animator>();

    }
    private void Awake()
    {
        dungeonPlayerAttack = GetComponent<DungeonPlayerMelee>();
        dungeonPlayerAttack.OnPlayerMelee += Slash;
    }

    private void Slash()
    {
        animator.SetTrigger("melee");
    }

    private void OnDestroy()
    {
        dungeonPlayerAttack.OnPlayerMelee -= Slash;
    }

    public void AttackStart()
    {
        weapon.GetComponent<Collider>().enabled = true;
    }

    public void AttackEnd()
    {
        animator.ResetTrigger("melee");
        weapon.GetComponent<Collider>().enabled = false;
        dungeonPlayerAttack.StopInvoking();
    }
}
