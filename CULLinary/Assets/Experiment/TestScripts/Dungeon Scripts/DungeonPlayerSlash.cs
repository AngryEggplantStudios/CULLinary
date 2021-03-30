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

    // Perform raycasting to rotate the player
    public DungeonRaycaster raycastLayer;
    public GameObject playerToRotate;
    public DungeonPlayerLocomotion rotationLocomotion;
    public float rotateSpeed = 0.5f;

    // Constants
    private const float MAX_DIST_CAM_TO_GROUND = 100f;
    private const float MELEE_ANIMATION_TIME_SECONDS = 0.10f;

    private Collider weaponCollider;
    private Vector3 rotateToFaceDirection;
    
    private void Awake()
    {
        dungeonPlayerAttack = GetComponent<DungeonPlayerMelee>();
        dungeonPlayerAttack.OnPlayerMelee += Slash;
        weaponCollider = weapon.GetComponent<Collider>();
        animator = GetComponent<Animator>();
        weaponCollider.enabled = false;
    }
    
    private IEnumerator RotatePlayer() 
    {
        for (float i = 0.0f;
             i < MELEE_ANIMATION_TIME_SECONDS;
             i = i + Time.deltaTime * rotateSpeed) 
        {
            rotationLocomotion.Rotate(rotateToFaceDirection, rotateSpeed);
            yield return null;
        }
    }

    private void Slash()
    {
        RaycastHit hit;
        if (raycastLayer.RaycastMouse(out hit, MAX_DIST_CAM_TO_GROUND)) {
            rotateToFaceDirection =
                new Vector3(hit.point.x - playerToRotate.transform.position.x,
                            0.0f,
                            hit.point.z - playerToRotate.transform.position.z);
            StartCoroutine("RotatePlayer");
        }
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
