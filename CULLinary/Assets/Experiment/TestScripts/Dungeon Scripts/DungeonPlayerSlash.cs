using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerSlash : MonoBehaviour
{
    private Animator animator;
    private DungeonPlayerMelee dungeonPlayerAttack;

    [SerializeField] private GameObject weapon;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] attackSounds;

    private Collider weaponCollider;
    
    private void Awake()
    {
        dungeonPlayerAttack = GetComponent<DungeonPlayerMelee>();
        dungeonPlayerAttack.OnPlayerMelee += Slash;
        weaponCollider = weapon.GetComponent<Collider>();
        animator = GetComponent<Animator>();
        weaponCollider.enabled = false;
    }

    private void Slash()
    {
        animator.SetBool("isMelee", true);
    }

    private void OnDestroy()
    {
        dungeonPlayerAttack.OnPlayerMelee -= Slash;
    }

    public void AttackStart()
    {
        weaponCollider.enabled = true;
        audioSource.clip = attackSounds[Random.Range(0, attackSounds.Length)];
        audioSource.Play();
    }

    public void AttackEnd()
    {
        animator.SetBool("isMelee", false);
        weaponCollider.enabled = false;
        dungeonPlayerAttack.StopInvoking();
    }
}
