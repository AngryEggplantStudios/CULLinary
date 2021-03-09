using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerProjectile : MonoBehaviour
{
    [SerializeField] private Transform throwingKnife;

    private DungeonPlayerAim dungeonPlayerAim;

    private void Awake()
    {
        dungeonPlayerAim = GetComponent<DungeonPlayerAim>();
        dungeonPlayerAim.OnPlayerShoot += ThrowKnife;
    }

    private void ThrowKnife(Vector3 sourcePosition, Vector3 endPosition, Vector3 lookVector)
    {
        Transform knifeTransform = Instantiate(throwingKnife, sourcePosition, Quaternion.identity);
        Vector3 projDir = (endPosition - sourcePosition).normalized;
        knifeTransform.GetComponent<Projectile>().Setup(projDir, lookVector);
    }

    private void OnDestroy()
    {
        dungeonPlayerAim.OnPlayerShoot -= ThrowKnife;
    }
}
