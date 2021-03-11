using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSpawn : MonoBehaviour
{
    private enum SpawnState {
        Inactive,
        Active,
        Loop,
    }

    [SerializeField] private GameObject enemyToSpawn;
    [Tooltip("Minimum amount of enemies to spawn per trigger")]
    [SerializeField] private int minEnemy;
    [Tooltip("Maximum amount of enemies to spawn per trigger")]
    [SerializeField] private int maxEnemy;
    [Tooltip("Random displacement of enemy spawn in X/Z axes")]
    [SerializeField] private float distRange;
    [Tooltip("Is spawner able to be retriggered?")]
    [SerializeField] private bool toLoop;
    [Tooltip("Delay after triggering if toLoop is checked")]
    [SerializeField] private float delayLoopTime;
    [Tooltip("Maximum number of enemies that can be spawned using this spawner")]
    [SerializeField] private int spawnCap; //It will not spawn more than this amount in total
    [Tooltip("Initial Delay")]
    [SerializeField] private int initialDelay = 0;
    private bool delayFlag = false;

    private SpawnState state;
    private bool canSpawn = true;
    private int spawnAmount = 0;

    private void Start()
    {
        this.state = SpawnState.Inactive;
        if (toLoop)
        {
            this.state = SpawnState.Loop;
        }
        if (initialDelay == 0)
        {
            delayFlag = true;
        }
        else
        {
            StartCoroutine(delayTimer(initialDelay));
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && delayFlag)
        {
            if (this.state == SpawnState.Inactive)
            {
                this.state = SpawnState.Active;
                int enemyNum = Random.Range(minEnemy, maxEnemy + 1);
                for (int i = 0; i < enemyNum; i++)
                {
                    instantiateEnemy();
                }
            }
            else if (this.state == SpawnState.Loop && canSpawn)
            {
                canSpawn = false;
                StartCoroutine(spawnTimer(delayLoopTime));
                int enemyNum = Random.Range(minEnemy, maxEnemy + 1);
                for (int i = 0; i < enemyNum; i++)
                {
                    instantiateEnemy();
                }
            }
        }

    }

    private void instantiateEnemy()
    {
        if (spawnAmount < spawnCap)
        {
            float distX = Random.Range(-distRange, distRange);
            float distZ = Random.Range(-distRange, distRange);
            Vector3 enemyTransform = new Vector3(transform.position.x + distX, transform.position.y, transform.position.z + distZ);
            Instantiate(enemyToSpawn, enemyTransform, Quaternion.identity);
            spawnAmount++;
        }
    }

    private IEnumerator delayTimer(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        delayFlag = true;
    }

    private IEnumerator spawnTimer(float delayLoopTime)
    {
        yield return new WaitForSeconds(delayLoopTime);
        canSpawn = true;
        
    }
}
