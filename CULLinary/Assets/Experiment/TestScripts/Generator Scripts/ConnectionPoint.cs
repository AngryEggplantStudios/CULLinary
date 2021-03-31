using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConnectionPoint : MonoBehaviour
{
    [SerializeField] private bool isConnected = false;
    [SerializeField] private float bias; //How far from the centre should the connection point spawn the next room?

    public IEnumerator GenerateRoom(GameObject roomToGenerate, bool isDeadEnd = false)
    {
        Quaternion rotation = Quaternion.LookRotation(this.transform.forward);
        Vector3 position = GenerateRelativeVector(rotation.eulerAngles.y, bias);
        GameObject generatedRoom = Instantiate(roomToGenerate, position, rotation);
        yield return null;

        ConnectionPoint[] connectionPoints = null;
        ConnectionPoint chosenPoint = null;
        if (!isDeadEnd)
        {
            connectionPoints = generatedRoom.GetComponentsInChildren<ConnectionPoint>();
            chosenPoint = connectionPoints[0];
        }
        CheckCollision validatorNewRoom = generatedRoom.GetComponentInChildren<CheckCollision>();
        validatorNewRoom.TurnOnCollider();
        yield return new WaitForSeconds(0.03f);

        if (validatorNewRoom.GetIsCollided())
        {
            Destroy(generatedRoom);
            yield return null;
        }
        else
        {
            this.SetConnected();
            if (!isDeadEnd)
            {
                //deactivate Deco before generating NavMesh
                generatedRoom.transform.Find("Environment").Find("Deco").gameObject.SetActive(false);

                MapGenerator.AddGeneratedRoom(generatedRoom);
                chosenPoint.SetConnected();
                MapGenerator.AddConnectionPoints(connectionPoints);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Validator"))
        {
            isConnected = true;
        }
    }

    public Vector3 GenerateRelativeVector(float eulerAngle, float bias)
    {
        float xBias = 0f;
        float zBias = 0f;
        if (eulerAngle > -10.0f && eulerAngle < 10.0f)
        {
            zBias = bias;
        }
        else if (eulerAngle > 80.0f && eulerAngle < 100.0f)
        {
            xBias = bias;
        }
        else if (eulerAngle > 170.0f && eulerAngle < 190.0f)
        {
            zBias = -bias;
        }
        else if (eulerAngle > 260.0f && eulerAngle < 280.0f)
        {
            xBias = -bias;
        }
        return new Vector3(this.transform.position.x + xBias, this.transform.position.y, this.transform.position.z + zBias);
    }

    public void SetConnected()
    {
        isConnected = true;
    }

    public bool GetIsConnected()
    {
        return isConnected;
    }

}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConnectionPoint : MonoBehaviour
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
    [SerializeField] private SpawnRoom[] MapGenerator.spawnRooms;
    [SerializeField] private GameObject mg.deadend;
    [SerializeField] private GameObject parentRef;
    [SerializeField] private GameObject validatorRef;
    [SerializeField] private float bias;

    private List<SpawnRoom> spawnList = new List<SpawnRoom>();

    public IEnumerator GenerateRoom()
    {
        float generatedProb = Random.Range(0f, 1f);
        GameObject roomToGenerate = mg.deadend;
        foreach (SpawnRoom sr in MapGenerator.spawnRooms)
        {
            if (generatedProb <= sr.GetCumProb() && !spawnList.Contains(sr))
            {
                roomToGenerate = sr.GetCorridor();
                spawnList.Add(sr);
                break;
            }
        }
        yield return null;

        if (roomToGenerate == mg.deadend)
        {
            GameObject generatedRoom = Instantiate(roomToGenerate, transform.position, Quaternion.identity);
            yield return null;
            generatedRoom.name = "Deadend";
            this.SetConnected();
        }
        else if (roomToGenerate != null)
        {

            GameObject generatedRoom = Instantiate(roomToGenerate, transform.position, Quaternion.identity);
            yield return null;
            ConnectionPoint[] connectionPoints = generatedRoom.GetComponentsInChildren<ConnectionPoint>();
            ConnectionPoint chosenPoint = connectionPoints[0];

            CheckCollision validatorNewRoom = chosenPoint.GetValidatorRef().GetComponent<CheckCollision>();

            yield return StartCoroutine(PositionRoom(chosenPoint));
            validatorNewRoom.TurnOnCollider();
            yield return null;
            yield return null;
            yield return null;
            yield return StartCoroutine(CheckValidity(validatorNewRoom, generatedRoom, chosenPoint, connectionPoints));
        }

    }

    private IEnumerator CheckValidity(CheckCollision validatorNewRoom, GameObject generatedRoom, ConnectionPoint chosenPoint, ConnectionPoint[] connectionPoints)
    {
        yield return null;
        yield return null;
        yield return null;
        if (validatorNewRoom.GetIsCollided() && !validatorNewRoom.GetIsEnd())
        {
            Destroy(generatedRoom);
            yield return null;
            MapGenerator.AddConnectionPoints(new ConnectionPoint[]{this});
        }
        else
        {
            
            //Debug.Log("Room is valid. Let's add it to the dungeon!");
            MapGenerator.AddRoomCounter();
            MapGenerator.AddGeneratedRoom(generatedRoom);
            //yield return StartCoroutine(generatedRoom.GetComponentInChildren<MeshCombiner>().CombineMeshes());
            this.SetConnected();
            chosenPoint.SetConnected();
            //Debug.Log("LMAO");
            MapGenerator.AddConnectionPoints(connectionPoints);
        }
    }

    private IEnumerator PositionRoom(ConnectionPoint newRoom)
    {
        Transform newTransform = newRoom.GetParentRef().transform;
        Transform currentTransform = this.transform;
        newTransform.rotation = Quaternion.LookRotation(currentTransform.forward);
        yield return null;
        yield return null;
        yield return null;
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
        yield return null;
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
*/