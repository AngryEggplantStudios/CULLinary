using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnTriggerEnter(Collider collider)
    {
        Enemy target = collider.GetComponent<Enemy>();
        if (target != null)
        {
            target.HandleHit(damage);
        }
    }
}
