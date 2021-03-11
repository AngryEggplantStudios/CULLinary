using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Generator Settings")]
    [Tooltip("Limit the dungeon map generated to a fixed number of rooms.")]
    [SerializeField] private bool limitByRooms;
    [Tooltip("The maximum number of rooms (excluding the starting room) that the algorithm can generate if limitByRooms is enabled.")]
    [SerializeField] private int roomLimit;
    [Tooltip("Confine the area of the dungeon generated to specific X and Z parameters")]
    [SerializeField] private bool limitByArea;
    [Tooltip("X parameter to limit by in world coordinates")]
    [SerializeField] private float x;
    [Tooltip("Z parameter to limit by in world coordinates")]
    [SerializeField] private float z;
    
    [Header("Start Settings")]
    [Tooltip("Starting room in which the algorithm will branch out to generate new rooms")]
    [SerializeField] private GameObject startingRoom;
    [Tooltip("A list of special rooms to be generated at the end")]
    [SerializeField] private static GameObject[] specialRooms;
    private static Stack<ConnectionPoint> connectionPoints = new Stack<ConnectionPoint>(); //Stack which stores all the connection points
    private static int roomCounter = 0; //To keep track of number of rooms being instantiated
    private bool isGenerated = false; //So that it only generates once
    private IEnumerator GenerateMap()
    {
        ConnectionPoint[] startingPoints = startingRoom.GetComponentsInChildren<ConnectionPoint>();
        foreach (ConnectionPoint c in startingPoints)
        {
            connectionPoints.Push(c);
        }
        yield return null;
        if (limitByRooms)
        {
            while (connectionPoints.Count > 0 && roomCounter < roomLimit)
            {
                ConnectionPoint currentPoint = connectionPoints.Pop();
                yield return null;
                yield return StartCoroutine(currentPoint.GenerateRoom());
            }
        }
        else if (limitByArea)
        {
            while (connectionPoints.Count > 0)
            {
                ConnectionPoint currentPoint = connectionPoints.Pop();
                yield return null;
                yield return StartCoroutine(currentPoint.GenerateRoom());
            }
        }
        else {
            while (connectionPoints.Count > 0)
            {
                ConnectionPoint currentPoint = connectionPoints.Pop();
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
                connectionPoints.Push(c);
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
