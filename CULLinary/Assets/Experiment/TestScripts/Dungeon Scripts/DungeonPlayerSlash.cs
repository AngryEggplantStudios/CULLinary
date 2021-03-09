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
        animator.SetBool("isMelee", true);
    }

    private void OnDestroy()
    {
        dungeonPlayerAttack.OnPlayerMelee -= Slash;
    }

    public void AttackStart()
    {
        weapon.GetComponent<Collider>().enabled = true;
        audioSource.clip = attackSounds[Random.Range(0, attackSounds.Length)];
        audioSource.Play();
    }

    public void AttackEnd()
    {
        animator.SetBool("isMelee", false);
        weapon.GetComponent<Collider>().enabled = false;
        dungeonPlayerAttack.StopInvoking();
    }
}
