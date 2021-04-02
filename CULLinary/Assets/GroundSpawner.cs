using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    List<Vector3> spawnedLocations = new List<Vector3>();
    // Start is called before the first frame update
    private int spawnNumber = 10;
    [SerializeField] private GameObject toInstantiate;
    void Start()
    {
        Debug.Log(transform.position);
        Vector2 randomSpawnedLocation;
        Vector3 spawnedLocation;
        for (int i = 0; i < spawnNumber; i++)
        {
            randomSpawnedLocation = Random.insideUnitCircle * 3f;
            spawnedLocation = new Vector3(randomSpawnedLocation.x + transform.position.x , 0.03f, randomSpawnedLocation.y + transform.position.z);
            while (!checkIfValidSpawnPoint(spawnedLocation))
            {
                randomSpawnedLocation = Random.insideUnitCircle * 3f;
                spawnedLocation = new Vector3(randomSpawnedLocation.x + transform.position.x, 0.03f, randomSpawnedLocation.y + transform.position.z);
            }            
            spawnedLocations.Add(spawnedLocation);
            Instantiate(toInstantiate, spawnedLocation, Quaternion.identity);
        }
    }

    private bool checkIfValidSpawnPoint(Vector3 potentialSpawn)
    {
        for (int i = 0; i < spawnedLocations.Count; i++)
        {
            Vector3 distance = spawnedLocations[i] - potentialSpawn;
            if (distance.magnitude < 1.0f)
            {
                return false;
            }
        }
        return true;
    }

}
