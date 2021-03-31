using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MapGeneratorNew : MonoBehaviour
{
    public GameObject startingRoom;
    public GameObject[] fillerRooms;
    public GameObject[] spawnRooms;
    public GameObject deadend;
    public int mapSize;
    public float fillerRatio = 0.5f;

    private static GameObject parent;
    private static List<GameObject> roomPool = new List<GameObject>();
    private static List<GameObject> generatedRooms = new List<GameObject>();
    private static int roomLimit;

    /*
    For Loading Screen usage
    */
    public static bool isGenerated = false;
    public static float roomProgress = 0f;
    public static bool isGeneratingRooms = true;
    public static bool isBuildingNavMesh = true;

    // Singleton
    private static MapGeneratorNew _instance;
    public static MapGeneratorNew Instance { get { return _instance; } }

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
    }

    private void Start()
    {
        parent = new GameObject("Dungeon");
        parent.AddComponent<NavMeshSurface>();


        StartCoroutine(GenerateMap());
    }

    private IEnumerator GenerateMap()
    {
        int mapLength = (mapSize * 2 + 1);
        int roomLimit = mapLength * mapLength - 1;

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
        GentlyShuffle(roomPool, 10);

        // Spiral out from the center and spawn rooms
        Spiral(mapLength, mapLength, (x, y, i) => {
            if (x == 0 && y == 0) return;

            GameObject generatedRoom = Instantiate(
                    roomPool[i-1],
                    new Vector3(x * 11, 0, y * 11), 
                    Quaternion.Euler(0, rng.Next(4) * 90, 0),
                    parent.transform);
                    
            roomProgress += 1f / roomLimit;
            generatedRooms.Add(generatedRoom);
        });

        isGeneratingRooms = false;
        yield return new WaitForSeconds(0.03f);

        //Let us build the navmesh now for the AI
        parent.GetComponent<NavMeshSurface>().BuildNavMesh();
        yield return new WaitForSeconds(0.03f);
        //reactivate Deco
        foreach (GameObject room in generatedRooms)
        {
            room.transform.Find("Environment").Find("Deco").gameObject.SetActive(true);
        }
        isBuildingNavMesh = false;

        isGenerated = true;

        // Generate deadends
        for (int x = -mapSize - 1; x <= mapSize + 1; x++)
        {
            for (int y = -mapSize - 1; y <= mapSize + 1; y++)
            {
                if ((x == -mapSize - 1) || (x == mapSize + 1) || (y == -mapSize - 1) ||( y == mapSize + 1))
                {
                    Instantiate(deadend,
                            new Vector3(x * 11, 0, y * 11), 
                            new Quaternion(0, 0, 0, 0),
                            parent.transform);
                }
            }
        }
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

    static void GentlyShuffle<T>(IList<T> list, int dist)
    {
        int n = list.Count;
        while (n > dist)
        {
            n--;
            int k = n - rng.Next(dist);
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

    static void Spiral( int X, int Y, Action<int, int, int> f){
        int x,y,dx,dy;
        x = y = dx =0;
        dy = -1;
        int t = Math.Max(X,Y);
        int maxI = t*t;
        for(int i =0; i < maxI; i++){
            if ((-X/2 <= x) && (x <= X/2) && (-Y/2 <= y) && (y <= Y/2)){
                f(x, y, i);
            }
            if( (x == y) || ((x < 0) && (x == -y)) || ((x > 0) && (x == 1-y))){
                t = dx;
                dx = -dy;
                dy = t;
            }
            x += dx;
            y += dy;
        }
    }
}