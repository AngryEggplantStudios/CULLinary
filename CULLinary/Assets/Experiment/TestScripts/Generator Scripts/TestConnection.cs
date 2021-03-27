using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestConnection : MonoBehaviour
{
    [SerializeField] private bool isConnected = false;

    [System.Serializable]
    private class SpawnRoom
    {
        [SerializeField] private GameObject corridor;
        [SerializeField] private float cumProb;
        private bool triedSpawning = false;
        public GameObject GetCorridor()
        {
            return corridor;
        }
        public float GetCumProb()
        {
            return cumProb;
        }
        public void SetTriedSpawning()
        {
            triedSpawning = true;
        }
        public bool GetTriedSpawning()
        {
            return triedSpawning;
        }
    }
    [SerializeField] private SpawnRoom[] spawnRooms;
    [SerializeField] private GameObject deadend;
    [SerializeField] private GameObject parentRef;
    [SerializeField] private GameObject validatorRef;
    [SerializeField] private float bias;

    private List<SpawnRoom> spawnList = new List<SpawnRoom>();

    public IEnumerator GenerateRoom()
    {
        float generatedProb = Random.Range(0f, 1f);
        GameObject roomToGenerate = deadend;
        foreach (SpawnRoom sr in spawnRooms)
        {
            if (generatedProb <= sr.GetCumProb() && !spawnList.Contains(sr))
            {
                roomToGenerate = sr.GetCorridor();
                spawnList.Add(sr);
                break;
            }
        }
        yield return null;

        if (roomToGenerate == deadend)
        {
            GameObject generatedRoom = Instantiate(roomToGenerate, transform.position, Quaternion.identity);
            yield return null;
            generatedRoom.name = "Deadend";
            this.SetConnected();
        }
        else if (roomToGenerate != null)
        {
            Quaternion rotation = Quaternion.LookRotation(this.transform.forward);
            yield return null;
            float newEulerAngle = rotation.eulerAngles.y;
            float xBias = 0f;
            float zBias = 0f;
            float newBias = 5;
            if (Mathf.Approximately(newEulerAngle, 0.0f))
            {
                zBias = newBias;   
            }
            else if (Mathf.Approximately(newEulerAngle, 90.0f))
            {
                xBias = newBias;
            }
            else if (Mathf.Approximately(newEulerAngle, 180.0f))
            {
                zBias = -newBias;
            }
            else if (Mathf.Approximately(newEulerAngle, 270.0f))
            {
                xBias = -newBias;
            }
            Vector3 position = new Vector3(this.transform.position.x + xBias, this.transform.position.y, this.transform.position.z + zBias);
            yield return null;
            GameObject generatedRoom = Instantiate(roomToGenerate, position, rotation);
            yield return null;

            TestConnection[] TestConnections = generatedRoom.GetComponentsInChildren<TestConnection>();
            TestConnection chosenPoint = TestConnections[0];
            CheckCollision validatorNewRoom = chosenPoint.GetValidatorRef().GetComponent<CheckCollision>();
            yield return null;
            validatorNewRoom.TurnOnCollider();
            yield return new WaitForSeconds(0.05f);

            if (validatorNewRoom.GetIsCollided() && !validatorNewRoom.GetIsEnd())
            {
                Destroy(generatedRoom);
                yield return null;
                TestMapGen.AddTestConnections(new TestConnection[]{this});
            }
            else
            {
                TestMapGen.AddRoomCounter();
                TestMapGen.AddGeneratedRoom(generatedRoom);
                this.SetConnected();
                chosenPoint.SetConnected();
                TestMapGen.AddTestConnections(TestConnections);
            }
        }

    }

    public GameObject GetValidatorRef()
    {
        return validatorRef;
    }

    public GameObject GetParentRef()
    {
        return parentRef;
    }

    public void SetNotConnected()
    {
        isConnected = false;
    }

    public void SetConnected()
    {
        isConnected = true;
    }

    public bool GetIsConnected()
    {
        return isConnected;
    }

    public float GetBias()
    {
        return bias;
    }

}
