using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject startingRoom;
    
    private static Stack<ConnectionPoint> connectionPoints = new Stack<ConnectionPoint>();
    private bool isGenerated = false;

    private IEnumerator GenerateMapTwo()
    {
        Debug.Log("Starting Generation of Map");
        ConnectionPoint[] startingPoints = startingRoom.GetComponentsInChildren<ConnectionPoint>();
        Debug.Log("Collecting connection points of starting room");
        yield return null;
        foreach (ConnectionPoint c in startingPoints)
        {
            connectionPoints.Push(c);
        }
        yield return null;
        while (connectionPoints.Count > 0) //Include special rooms that will only generate at the end
        {
            Debug.Log("Popping a connection point");
            ConnectionPoint currentPoint = connectionPoints.Pop();
            yield return null;
            yield return StartCoroutine(currentPoint.GenerateRoomTwo());
        }
        Debug.Log(connectionPoints.Count);
        yield return new WaitForSeconds(10);
        Debug.Log(connectionPoints.Count);
    }

    public static void AddConnectionPoints(ConnectionPoint[] points)
    {
        foreach (ConnectionPoint c in points)
        {
            Debug.Log("LOL");
            if (!c.GetIsConnected())
            {
                Debug.Log("Wow!");
                connectionPoints.Push(c);
            }
            
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isGenerated)
        {
            isGenerated = true;
            Debug.Log("Generate map!");
            StartCoroutine(GenerateMapTwo());
        }

    }

}
