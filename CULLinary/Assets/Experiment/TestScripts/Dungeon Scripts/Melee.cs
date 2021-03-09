using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnTriggerEnter(Collider collider)
    {
        EnemyScript target = collider.GetComponent<EnemyScript>();
        if (target != null)
        {
            target.HandleHit(damage);
            Debug.Log("Melee!");
        }
    }
}
