using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{

    [SerializeField] private GameObject enemyToSpawn;
    [Tooltip("Minimum amount of enemies to spawn per trigger")]
    [SerializeField] private int minEnemy;
    [Tooltip("Maximum amount of enemies to spawn per trigger")]
    [SerializeField] private int maxEnemy;
    [Tooltip("Random displacement of enemy spawn in X/Z axes")]
    [SerializeField] private float distRange;

    public void activateSpawn()
    {
        int enemyNum = Random.Range(minEnemy, maxEnemy + 1);
        instantiateEnemy();
    }

    private void instantiateEnemy()
    {
        float distX = Random.Range(-distRange, distRange);
        float distZ = Random.Range(-distRange, distRange);
        Vector3 enemyTransform = new Vector3(transform.position.x + distX, transform.position.y, transform.position.z + distZ);
        GameObject mobSpawned = Instantiate(enemyToSpawn, enemyTransform, Quaternion.identity);
        mobSpawned.transform.SetParent(gameObject.transform);
    }
    
    public void destroyAllSpawns()
    {
        int children = transform.childCount;
        for (int i = 0; i < children; i++)
        {
            BossSpawnMinionScript minion = transform.GetChild(i).GetComponent<BossSpawnMinionScript>();
            minion.Die();
        }
    }

}
