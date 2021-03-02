using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    [SerializeField] private float health;
    [SerializeField] private GameObject loot;

    public void HandleHit(float damage)
    {
        this.health -= damage;
        Debug.Log("Current health: " + health);
        if (this.health <= 0)
        {   
            DropLoot();
            Destroy(gameObject, 0.2f);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Eggplant died uwu");
        
    }

    private void DropLoot()
    {
        Instantiate(loot, transform.position, Quaternion.identity);
    }


}
