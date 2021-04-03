using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerProjectile : MonoBehaviour
{
    [SerializeField] private Transform throwingKnife;

    private DungeonPlayerAim dungeonPlayerAim;
    private int damage;

    private void Awake()
    {
        dungeonPlayerAim = GetComponent<DungeonPlayerAim>();
        dungeonPlayerAim.OnPlayerShoot += ThrowKnife;
    }

    private void Start()
    {
        damage = PlayerManager.playerData == null ? 20 : PlayerManager.playerData.GetRangeDamage();
    }

    private void ThrowKnife(Vector3 sourcePosition, Vector3 targetPosition)
    {
        Transform knifeTransform = Instantiate(throwingKnife, sourcePosition, Quaternion.identity);
        knifeTransform.GetComponent<Projectile>().Setup(sourcePosition, targetPosition, damage);
    }

    private void OnDestroy()
    {
        dungeonPlayerAim.OnPlayerShoot -= ThrowKnife;
    }
}
