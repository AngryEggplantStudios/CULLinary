using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnAttack : MonoBehaviour
{
    // Start is called before the first frame update
    //first 3 spawn points for stage 1
    [SerializeField] private BossSpawn[] spawnPoint1;


    private int totalSpawnPoints = 3;
    private int currSpawnPoint = 0;
    private bool stage2On = false;
    public void spawnMobs()
    {
        if (stage2On)
        {
            for (int i = 0; i < totalSpawnPoints; i++)
            {
                spawnPoint1[i].activateSpawn();
            }
        } else
        {
            spawnPoint1[currSpawnPoint].activateSpawn();
            currSpawnPoint = (currSpawnPoint + 1) % 3;
        }

    }

    public void activateStage2()
    {
        stage2On = true;
    }

    public void destroySpawnPoints()
    {
        for (int i = 0; i < totalSpawnPoints; i++)
        {
            spawnPoint1[i].destroyAllSpawns();
        }
    }
}
