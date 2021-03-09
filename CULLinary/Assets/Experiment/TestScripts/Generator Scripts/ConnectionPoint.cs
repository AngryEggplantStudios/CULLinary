using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionPoint : MonoBehaviour
{
    [SerializeField] private bool isConnected = false;

    [System.Serializable]
    private class SpawnRoom
    {
        [SerializeField] private GameObject corridor;
        [SerializeField] private float cumProb;
        [SerializeField] private int ordinal; //number of connection points
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
        public int GetOrdinal()
        {
            return ordinal;
        }
    }
    [SerializeField] private SpawnRoom[] spawnRooms;
    [SerializeField] private GameObject deadend;
    [SerializeField] private GameObject parentRef;
    [SerializeField] private GameObject validatorRef;
    [SerializeField] private float bias;

    public IEnumerator GenerateRoomTwo()
    {
        Debug.Log("Let us run GenerateRoom Algorithm!");
        float generatedProb = Random.Range(0f, 1f);
        int ordinalSelected = 0;
        GameObject roomToGenerate = deadend;
        foreach (SpawnRoom sr in spawnRooms)
        {
            if (generatedProb <= sr.GetCumProb())
            {
                if (!sr.GetTriedSpawning())
                {
                    Debug.Log("Trying a room");
                    roomToGenerate = sr.GetCorridor();
                    ordinalSelected = sr.GetOrdinal();
                    sr.SetTriedSpawning();
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
        yield return null;

        Debug.Log("Let's generate a random number based on the ordinal");
        int generatedOrdinal = Random.Range(0, ordinalSelected);
        Debug.Log("Chosen ordinal is " + generatedOrdinal);
        Debug.Log("Let's instantiate the room first");

        yield return null;

        GameObject generatedRoom = Instantiate(roomToGenerate, transform.position, Quaternion.identity);

        yield return null;

        generatedRoom.name = "Generated";
        ConnectionPoint[] connectionPoints = generatedRoom.GetComponentsInChildren<ConnectionPoint>();
        ConnectionPoint chosenPoint = connectionPoints[0]; //generatedOrdinal

        yield return StartCoroutine(TryPositioningRoom(chosenPoint));

        CheckCollision validatorNewRoom = chosenPoint.GetValidatorRef().GetComponent<CheckCollision>();
        validatorNewRoom.TurnOnCollider();
        yield return null;

        yield return StartCoroutine(DelayOneFrame(validatorNewRoom, generatedRoom, chosenPoint, connectionPoints));

    }

    private IEnumerator TryPositioningRoom(ConnectionPoint newRoom)
    {
        yield return null;
        PositionRoom(newRoom);
        yield return null;
    }

    private IEnumerator InstantiateRoom(GameObject roomToGenerate, System.Action<GameObject> callback)
    {
        yield return new WaitForEndOfFrame();
        GameObject generatedRoom = Instantiate(roomToGenerate, transform.position, Quaternion.identity);
        yield return new WaitForEndOfFrame();
        callback(generatedRoom);
    }

    private IEnumerator DelayOneFrame(CheckCollision validatorNewRoom, GameObject generatedRoom, ConnectionPoint chosenPoint, ConnectionPoint[] connectionPoints)
    {
        yield return null;
        if (validatorNewRoom.GetIsCollided() && !validatorNewRoom.GetIsEnd())
        {
            Debug.Log("Collided! Need to destroy the room");
            Destroy(generatedRoom);
            MapGenerator.AddConnectionPoints(new ConnectionPoint[]{this});
        }
        else
        {
            Debug.Log("Room is valid. Let's add it to the dungeon!");
            this.SetConnected();
            chosenPoint.SetConnected();
            MapGenerator.AddConnectionPoints(connectionPoints);
        }
    }

    private ConnectionPoint PositionRoom(ConnectionPoint newRoom)
    {
        Transform newTransform = newRoom.GetParentRef().transform;
        Transform currentTransform = this.transform;
        newTransform.rotation = Quaternion.LookRotation(currentTransform.forward);
        float newEulerAngle = newTransform.eulerAngles.y;
        float xBias = 0f;
        float zBias = 0f;
        float newBias = newRoom.GetBias();
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
        newTransform.position = new Vector3(currentTransform.position.x + xBias, currentTransform.position.y, currentTransform.position.z + zBias);
        return newRoom;
    }


    public GameObject GetValidatorRef()
    {
        return validatorRef;
    }

        public GameObject GetParentRef()
    {
        return parentRef;
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
