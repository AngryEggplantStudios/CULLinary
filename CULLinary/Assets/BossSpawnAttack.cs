using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnAttack : MonoBehaviour
{
    // Start is called before the first frame update
    //first 3 spawn points for stage 1
    [SerializeField] private BossSpawn[] spawnPoint1;


    private int totalSpawnPoints = 3;

    public void spawnMobs()
    {
        for (int i = 0; i < totalSpawnPoints; i++)
        {
            spawnPoint1[i].activateSpawn();
        }
    }

}
