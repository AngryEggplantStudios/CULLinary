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
        weapon.GetComponent<Collider>().enabled = true;
    }

    private void Stop()
    {
        weapon.GetComponent<Collider>().enabled = false;
        animator.SetBool("isPunch", false);
    }

    private void OnDestroy()
    {
        dungeonPlayerAttack.OnPlayerMelee -= Slash;
        dungeonPlayerAttack.OnPlayerStop -= Stop;
    }
}
