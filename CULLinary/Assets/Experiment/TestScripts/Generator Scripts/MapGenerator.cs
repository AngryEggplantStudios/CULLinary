using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    public GameObject startingRoom;
    public GameObject[] fillerRooms;
    public GameObject[] spawnRooms;
    public GameObject deadend;
    public int roomLimit;
    public float fillerRatio = 0.5f;

    private static GameObject parent;
    private static int roomCounter = 0;
    private static List<GameObject> roomPool;
    private static List<GameObject> generatedRooms;
    
    private static Queue<ConnectionPoint> ConnectionPoints;

    /*
    For Loading Screen usage
    */
    public static bool isGenerated = false;
    public static float roomProgress = 0f;
    public static bool isGeneratingDeadends = false;
    public static bool isGeneratingRooms = true;
    public static bool isBuildingNavMesh = false;

    // Singleton
    private static MapGenerator _instance;
    public static MapGenerator Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            _instance = this;
        }
        generatedRooms = new List<GameObject>();
        ConnectionPoints = new Queue<ConnectionPoint>();
        roomPool = new List<GameObject>();
    }

    private void Start()
    {
        parent = new GameObject();
        parent.AddComponent<NavMeshSurface>();

        // Generate pools of rooms
        List<GameObject> fillerRoomPool = new List<GameObject>();
        for (int i = 0; i < fillerRatio * roomLimit; i++)
        {
            fillerRoomPool.Add(fillerRooms[i % fillerRooms.Length]);
        }
        Shuffle(fillerRoomPool);

        List<GameObject> spawnRoomPool = new List<GameObject>();
        int numOfDuplicates = (int)((1.0f - fillerRatio) * roomLimit / spawnRooms.Length) + 1;
        for (int i = 0; i < (1 - fillerRatio) * roomLimit; i++)
        {
            spawnRoomPool.Add(spawnRooms[i / numOfDuplicates]);
        }

        roomPool = MergeShuffle(fillerRoomPool, spawnRoomPool).ToList();

        roomCounter = 0;
        roomProgress = 0f;
        isGeneratingRooms = true;
        isBuildingNavMesh = false;
        isGenerated = false;
        isGeneratingDeadends = false;

        StartCoroutine(GenerateMap());
    }

    private IEnumerator GenerateMap()
    {
        //Add starting room's connection points
        AddConnectionPoints(startingRoom.GetComponentsInChildren<ConnectionPoint>());
        yield return null;

        //While we haven't exceed the room limit,
        //let's try to generate the room for each connection point,
        //starting from the center
        while (ConnectionPoints.Count > 0 && roomPool.Count > 0 )
        {
            ConnectionPoint currentPoint = ConnectionPoints.Dequeue();
            yield return null;
            
            yield return StartCoroutine(currentPoint.GenerateRoom(roomPool[0]));
        }

        isGeneratingRooms = false;
        yield return new WaitForSeconds(0.03f);

        //Let us build the navmesh now for the AI
        isBuildingNavMesh = true;
        parent.GetComponent<NavMeshSurface>().BuildNavMesh();
        yield return new WaitForSeconds(0.03f);
        //reactivate Deco
        foreach (GameObject room in generatedRooms)
        {
            room.transform.Find("Environment").Find("Deco").gameObject.SetActive(true);
        }
        isBuildingNavMesh = false;

        isGenerated = true;

        //For all the connection points left, let us generate the deadend
        //isGeneratingDeadends = true;
        foreach (ConnectionPoint currentPoint in ConnectionPoints)
        {
            yield return null;
            yield return StartCoroutine(currentPoint.GenerateRoom(deadend, true));
        }
        //isGeneratingDeadends = false;
    }

    public static void AddConnectionPoints(ConnectionPoint[] points)
    {
        foreach (ConnectionPoint c in points)
        {
            if (!c.GetIsConnected())
            {
                ConnectionPoints.Enqueue(c);
            }
        }
    }

    public static void AddGeneratedRoom(GameObject room)
    {
        roomPool.RemoveAt(0);
        roomCounter++;
        roomProgress = roomCounter / (float)_instance.roomLimit;
        generatedRooms.Add(room);
        room.transform.parent = parent.transform;
    }
    
    // Shuffle algos for randomising rooms
    static System.Random rng = new System.Random();

    static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    static IEnumerable<T> MergeShuffle<T>(IEnumerable<T> lista, IEnumerable<T> listb)
    {
        int total = lista.Count() + listb.Count();
        var indexes = Enumerable.Range(0, total-1)
                                .OrderBy(_=>rng.NextDouble())
                                .Take(lista.Count())
                                .OrderBy(x=>x)
                                .ToList();

        var first = lista.GetEnumerator();
        var second = listb.GetEnumerator();

        for (int i = 0; i < total; i++)
            if (indexes.Contains(i))
            {
                first.MoveNext();
                yield return first.Current;
            }
            else
            {
                second.MoveNext();
                yield return second.Current;
            }
    }

    static T RemoveAndReturnFirst<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            return default(T);
        }

        T currentFirst = list[0];
        list.RemoveAt(0);
        return currentFirst;
    }
}

/*
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

    
    For Loading Screen usage
    
    public static bool isGenerated = false;
    public static float roomProgress = 0f;
    public static bool isGeneratingRooms = true;
    public static bool isBuildingNavMesh = false;
    public static MapGenerator current;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        parent = new GameObject();
        parent.AddComponent<NavMeshSurface>();
        generatedRooms = new List<GameObject>();
        connectionPoints = new Queue<ConnectionPoint>();

        roomCounter = 0;
        roomProgress = 0f;
        isGeneratingRooms = true;
        isBuildingNavMesh = false;
        isGenerated = false;

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
            while (connectionPoints.Count > 0)
            {
                ConnectionPoint currentPoint = connectionPoints.Dequeue();
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
        roomProgress += 1f / current.roomLimit;
        generatedRooms.Add(room);
        room.transform.parent = parent.transform;
    }

    public static void AddRoomCounter()
    {
        roomCounter++;
    }

}
*/
