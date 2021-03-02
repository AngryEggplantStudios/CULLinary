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
        if (this.health <= 0)
        {   
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Eggplant died uwu");
        DropLoot();
    }

    private void DropLoot()
    {
        Instantiate(loot, transform.position, Quaternion.identity);
    }


}
