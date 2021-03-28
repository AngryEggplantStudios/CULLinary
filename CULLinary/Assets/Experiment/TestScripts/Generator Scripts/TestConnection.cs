using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestConnection : MonoBehaviour
{
    [SerializeField] private bool isConnected = false;
    [SerializeField] private GameObject[] spawnRooms;
    [SerializeField] private GameObject deadend;
    [SerializeField] private float bias; //How far from the centre should the connection point spawn the next room?

    public IEnumerator GenerateRoom()
    {
        int generatedProb = Random.Range(0, spawnRooms.Length);
        GameObject roomToGenerate = spawnRooms[generatedProb];
        Quaternion rotation = Quaternion.LookRotation(this.transform.forward);
        Vector3 position = GenerateRelativeVector(rotation.eulerAngles.y, bias);
        GameObject generatedRoom = Instantiate(roomToGenerate, position, rotation);
        yield return null;
        TestConnection[] TestConnections = generatedRoom.GetComponentsInChildren<TestConnection>();
        CheckCollision validatorNewRoom = generatedRoom.GetComponentInChildren<CheckCollision>();
        TestConnection chosenPoint = TestConnections[0];
        validatorNewRoom.TurnOnCollider();
        yield return new WaitForSeconds(0.05f);

        if (validatorNewRoom.GetIsCollided())
        {
            Destroy(generatedRoom);
            yield return null;
        }
        else
        {
            TestMapGen.AddGeneratedRoom(generatedRoom);
            this.SetConnected();
            chosenPoint.SetConnected();
            TestMapGen.AddTestConnections(TestConnections);
        }
    }

    public IEnumerator GenerateDeadend()
    {
        GameObject roomToGenerate = deadend;
        Quaternion rotation = Quaternion.LookRotation(this.transform.forward);
        Vector3 position = GenerateRelativeVector(rotation.eulerAngles.y, bias);
        GameObject generatedRoom = Instantiate(roomToGenerate, position, rotation);
        yield return null;
        CheckCollision validatorNewRoom = generatedRoom.GetComponentInChildren<CheckCollision>();
        validatorNewRoom.TurnOnCollider();
        yield return new WaitForSeconds(0.05f);
        if (validatorNewRoom.GetIsCollided())
        {
            Destroy(generatedRoom);
            yield return null;
        }
        else
        {
            this.SetConnected();
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
