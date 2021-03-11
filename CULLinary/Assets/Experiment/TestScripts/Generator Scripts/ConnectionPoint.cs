using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionPoint : MonoBehaviour
{
    [System.Serializable] private class SpawnRoom
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

    [Header("Connection Settings")]
    [Tooltip("List of spawn rooms that can be instantiated")]
    [SerializeField] private SpawnRoom[] spawnRooms;
    [Tooltip("Default deadend to be instantiated")]
    [SerializeField] private GameObject deadend;
    [Tooltip("Reference to parent")]
    [SerializeField] private GameObject parentRef;
    [Tooltip("Reference to validator")]
    [SerializeField] private GameObject validatorRef;
    [Tooltip("The number of blocks from the centre of the room generated")]
    [SerializeField] private float bias;
    private bool isConnected = false;

    public IEnumerator GenerateRoom(bool checkTransform=false, float x=0f, float z=0f)
    {
        float generatedProb = Random.Range(0f, 1f);
        GameObject roomToGenerate = deadend;
        foreach (SpawnRoom sr in spawnRooms)
        {
            if (generatedProb <= sr.GetCumProb() && !sr.GetTriedSpawning())
            {
                roomToGenerate = sr.GetCorridor();
                sr.SetTriedSpawning();
                break;
            }
        }
        if (roomToGenerate != null)
        {
            yield return null;
            GameObject generatedRoom = Instantiate(roomToGenerate, transform.position, Quaternion.identity);
            yield return null;
            generatedRoom.name = "Generated";
            ConnectionPoint[] connectionPoints = generatedRoom.GetComponentsInChildren<ConnectionPoint>();
            foreach (ConnectionPoint c in connectionPoints)
            {
                c.SetNotConnected(); //Ensure all are not connected first
            }
            ConnectionPoint chosenPoint = connectionPoints[0];
            yield return StartCoroutine(PositionRoom(chosenPoint));
            CheckCollision validatorNewRoom = chosenPoint.GetValidatorRef().GetComponent<CheckCollision>();
            yield return null;
            validatorNewRoom.TurnOnCollider();
            yield return null;
            yield return StartCoroutine(CheckValidity(validatorNewRoom, generatedRoom, chosenPoint, connectionPoints, checkTransform, x, z));
        }
    }

    private IEnumerator CheckValidity(CheckCollision validatorNewRoom, GameObject generatedRoom, 
        ConnectionPoint chosenPoint, ConnectionPoint[] connectionPoints, bool checkTransform=false,
        float x=0f, float z=0f
        )
    {
        yield return null;
        if ((validatorNewRoom.GetIsCollided() && !validatorNewRoom.GetIsEnd()) || (checkTransform && checkBounds(generatedRoom.transform, x, z)))
        {
            Destroy(generatedRoom);
            validatorNewRoom.SetIsNotCollided();
            validatorNewRoom.TurnOffCollider();
            MapGenerator.AddConnectionPoints(new ConnectionPoint[]{this});
            yield return null;
        }
        else
        {
            this.SetConnected();
            chosenPoint.SetConnected();
            MapGenerator.AddRoomCounter();
            MapGenerator.AddConnectionPoints(connectionPoints);
            yield return null;
        }
    }

    private bool checkBounds(Transform transform, float x, float z)
    {
        bool value = Mathf.Abs(transform.position.x) > x || Mathf.Abs(transform.position.z) > z;
        return value;
    }

    private IEnumerator PositionRoom(ConnectionPoint newRoom)
    {
        yield return null;
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
        yield return null;
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
