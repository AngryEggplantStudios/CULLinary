using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject startingRoom;

    [SerializeField] private bool limitByRooms;
    [SerializeField] private int roomLimit;
    private static int roomCounter = 0;
    
    private static Queue<ConnectionPoint> connectionPoints = new Queue<ConnectionPoint>();
    private bool isGenerated = false;

    private IEnumerator GenerateMap()
    {
        ConnectionPoint[] startingPoints = startingRoom.GetComponentsInChildren<ConnectionPoint>();
        foreach (ConnectionPoint c in startingPoints)
        {
            connectionPoints.Enqueue(c);
        }
        yield return null;
        if (limitByRooms)
        {
            while (connectionPoints.Count > 0 && roomCounter < roomLimit ) //Include special rooms that will only generate at the end
            {
                ConnectionPoint currentPoint = connectionPoints.Dequeue();
                yield return null;
                yield return StartCoroutine(currentPoint.GenerateRoom());
            }
        }
        else {
            while (connectionPoints.Count > 0)
            {
                ConnectionPoint currentPoint = connectionPoints.Dequeue();
                yield return null;
                yield return StartCoroutine(currentPoint.GenerateRoom());
            }
        }
    }

    public static void AddConnectionPoints(ConnectionPoint[] points)
    {
        foreach (ConnectionPoint c in points)
        {
            if (!c.GetIsConnected())
            {
                connectionPoints.Enqueue(c);
            }
            
        }
    }

    public static void AddRoomCounter()
    {
        roomCounter++;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isGenerated)
        {
            isGenerated = true;
            StartCoroutine(GenerateMap());
        }

    }

}
