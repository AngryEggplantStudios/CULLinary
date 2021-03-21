using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject startingRoom;

    [SerializeField] private bool limitByRooms;
    [SerializeField] private int roomLimit;
    private static GameObject parent;
    private static int roomCounter = 0;
    private static List<GameObject> generatedRooms = new List<GameObject>();
    
    private static Queue<ConnectionPoint> connectionPoints = new Queue<ConnectionPoint>();
    private bool isGenerated = false;
    
    [SerializeField] private GameObject player;

    //need to reinitialize the static components on loadscene.
    private void Start()
    {
        parent = new GameObject();
        parent.AddComponent<NavMeshSurface>();
        roomCounter = 0;
        generatedRooms = new List<GameObject>();
        connectionPoints = new Queue<ConnectionPoint>();
        isGenerated = true;
        StartCoroutine(GenerateMap());
    }

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
            if (roomCounter == roomLimit)
            {
                parent.GetComponent<NavMeshSurface>().BuildNavMesh();
                Debug.Log("Building Navmesh");
            }
        }
        else {
            Debug.Log("Else Added!");
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

    public static void AddGeneratedRoom(GameObject room)
    {
        generatedRooms.Add(room);
        room.transform.parent = parent.transform;
    }

    public static void AddRoomCounter()
    {
        roomCounter++;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && false)
        {
            isGenerated = true;
            StartCoroutine(GenerateMap());
        }

    }

}
