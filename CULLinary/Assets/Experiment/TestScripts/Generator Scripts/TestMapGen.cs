using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestMapGen : MonoBehaviour
{
    [SerializeField] private GameObject startingRoom;

    [SerializeField] private bool limitByRooms;
    [SerializeField] private int roomLimit;

    private static GameObject parent;
    private static int roomCounter = 0;
    private static List<GameObject> generatedRooms = new List<GameObject>();
    
    private static Queue<TestConnection> TestConnections = new Queue<TestConnection>();

    /*
    For Loading Screen usage
    */
    public static bool isGenerated = false;
    public static float roomProgress = 0f;
    public static bool isGeneratingRooms = true;
    public static bool isBuildingNavMesh = false;
    public static TestMapGen current;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        parent = new GameObject();
        parent.AddComponent<NavMeshSurface>();
        generatedRooms = new List<GameObject>();
        TestConnections = new Queue<TestConnection>();

        roomCounter = 0;
        roomProgress = 0f;
        isGeneratingRooms = true;
        isBuildingNavMesh = false;
        isGenerated = false;

        StartCoroutine(GenerateMap());
    }

    private IEnumerator GenerateMap()
    {
        TestConnection[] startingPoints = startingRoom.GetComponentsInChildren<TestConnection>();
        foreach (TestConnection c in startingPoints)
        {
            TestConnections.Enqueue(c);
        }
        yield return null;
        if (limitByRooms)
        {
            while (TestConnections.Count > 0 && roomCounter < roomLimit ) //Include special rooms that will only generate at the end
            {
                TestConnection currentPoint = TestConnections.Dequeue();
                yield return null;
                yield return StartCoroutine(currentPoint.GenerateRoom());
            }

            isGeneratingRooms = false;
            isBuildingNavMesh = true;
            yield return new WaitForSeconds(0.5f);
            if (roomCounter == roomLimit)
            {
                parent.GetComponent<NavMeshSurface>().BuildNavMesh();
            }
            isBuildingNavMesh = false;
        }
        else {
            while (TestConnections.Count > 0)
            {
                TestConnection currentPoint = TestConnections.Dequeue();
                yield return null;
                yield return StartCoroutine(currentPoint.GenerateRoom());
            }
            isGeneratingRooms = false;
            isBuildingNavMesh = true;
            yield return new WaitForSeconds(0.5f);
            parent.GetComponent<NavMeshSurface>().BuildNavMesh();
            isBuildingNavMesh = false;
        }
        
        isGenerated = true;
    }

    public static void AddTestConnections(TestConnection[] points)
    {
        foreach (TestConnection c in points)
        {
            if (!c.GetIsConnected())
            {
                TestConnections.Enqueue(c);
            }
        }
    }

    public static void AddGeneratedRoom(GameObject room)
    {
        roomProgress += 1f / current.roomLimit;
        generatedRooms.Add(room);
        room.transform.parent = parent.transform;
    }

    public static void AddRoomCounter()
    {
        roomCounter++;
    }

}
