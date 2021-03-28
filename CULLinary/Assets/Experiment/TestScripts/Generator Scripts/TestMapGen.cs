using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestMapGen : MonoBehaviour
{
    [SerializeField] private GameObject startingRoom;
    [SerializeField] private int roomLimit;

    private static GameObject parent;
    private static int roomCounter = 0;
    private static List<GameObject> generatedRooms = new List<GameObject>();
    
    private static Queue<TestConnection> testConnections = new Queue<TestConnection>();

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
        testConnections = new Queue<TestConnection>();

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
        //Enqueues all the starting points' connection points
        foreach (TestConnection c in startingPoints)
        {
            testConnections.Enqueue(c);
        }
        yield return null;
        //While we haven't exceed the room limit, let's try to generate the room for each connection point
        while (testConnections.Count > 0 && roomCounter < roomLimit )
        {
            TestConnection currentPoint = testConnections.Dequeue();
            yield return null;
            yield return StartCoroutine(currentPoint.GenerateRoom());
        }
        //For all the connection points left, let us generate the deadend
        foreach (TestConnection t in testConnections)
        {
            yield return null;
            yield return StartCoroutine(t.GenerateDeadend());
        }
        isGeneratingRooms = false;
        isBuildingNavMesh = true;
        yield return new WaitForSeconds(0.05f);
        //Let us build the navmesh now for the AI
        parent.GetComponent<NavMeshSurface>().BuildNavMesh();
        yield return new WaitForSeconds(0.05f);
        isBuildingNavMesh = false;
        isGenerated = true;
    }

    public static void AddTestConnections(TestConnection[] points)
    {
        foreach (TestConnection c in points)
        {
            if (!c.GetIsConnected())
            {
                testConnections.Enqueue(c);
            }
        }
    }

    public static void AddGeneratedRoom(GameObject room)
    {
        roomProgress += 1f / current.roomLimit;
        roomCounter++;
        generatedRooms.Add(room);
        room.transform.parent = parent.transform;
    }
    
}

/*
List<TestConnection> deadendList = new List<TestConnection>();
foreach (GameObject o in generatedRooms)
{
    TestConnection[] tc = o.GetComponentsInChildren<TestConnection>();
    foreach (TestConnection deadend in tc)
    {
        if (!deadend.GetIsConnected())
        {
            deadendList.Add(deadend);
        }
    }
    yield return null;
}
Debug.Log(testConnections.Count);
Debug.Log(deadendList.Count);
*/
