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
    public GameObject specialClownRoom;
    public GameObject deadendEdge;
    public GameObject deadendCorner;
    public int mapSize = 5;
    public float fillerRatio = 0.2f;
    public int randomness = 30;
    // This dialogue is for the clown room
    public DialogueLoader dialogueForClownRoom;

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
    public static bool isBuildingNavMesh = false;
    public static bool isLoadingGame = false;

    
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
        if (PlayerManager.instance != null)
        {
            PlayerManager.LoadData();
        }

        generatedRooms.Clear();
        roomProgress = 0f;
        isGenerated = false;
        isGeneratingRooms = true;
        isBuildingNavMesh = false;

        int mapLength = (mapSize * 2 + 1);
        roomLimit = mapLength * mapLength - 1;

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

        if (PlayerManager.playerData != null && specialClownRoom != null && PlayerManager.playerData.GetIfKeyItemBoughtById(0))
        {
            spawnRoomPool[UnityEngine.Random.Range(Mathf.RoundToInt(spawnRoomPool.Count * 0.75f), spawnRoomPool.Count)] = specialClownRoom; //It will only appear in the last 25% of rooms
        }

        roomPool = MergeShuffle(fillerRoomPool, spawnRoomPool).ToList();
        GentlyShuffle(roomPool, randomness);

        // Spiral out from the center and spawn rooms
        /*
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
        */

        //Spiral out from the center and spawn rooms
        int x,y,dx,dy;
        x = y = dx =0;
        dy = -1;
        int t = Math.Max(mapLength,mapLength);
        int maxI = t*t;
        for(int i =0; i < maxI; i++){
            if ((-mapLength/2 <= x) && (x <= mapLength/2) && (-mapLength/2 <= y) && (y <= mapLength/2)){
                if (x != 0 || y != 0)
                {
                    yield return StartCoroutine(GenerateRoom(x, y, i));
                } 
            }
            if( (x == y) || ((x < 0) && (x == -y)) || ((x > 0) && (x == 1-y))){
                t = dx;
                dx = -dy;
                dy = t;
            }
            x += dx;
            y += dy;
        }
        foreach (GameObject room in generatedRooms)
        {
            room.transform.Find("Environment").Find("Deco").gameObject.SetActive(false);
        }
        isGeneratingRooms = false;
        isBuildingNavMesh = true;
        yield return new WaitForSeconds(0.03f);
        //Let us build the navmesh now for the AI
        parent.GetComponent<NavMeshSurface>().BuildNavMesh();
        yield return new WaitForSeconds(0.03f);
        //reactivate Deco
        foreach (GameObject room in generatedRooms)
        {
            room.transform.Find("Environment").Find("Deco").gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(0.03f);
        isBuildingNavMesh = false;
        isLoadingGame = true;
        
        // Generate deadends
        for (int i = 0; i < 4; i++)
        {
            for (int j = -mapSize - 1; j <= mapSize; j++)
            {
                yield return null;
                switch (i)
                {
                    case 0:
                        Instantiate(j == (-mapSize - 1) ? deadendCorner : deadendEdge,
                                new Vector3(j * 11, 0, (mapSize + 1) * 11), 
                                Quaternion.Euler(0, 0, 0),
                                parent.transform);
                        break;
                    case 1:
                        Instantiate(j == (-mapSize - 1) ? deadendCorner : deadendEdge,
                                new Vector3((mapSize + 1) * 11, 0, -j * 11), 
                                Quaternion.Euler(0, 90, 0),
                                parent.transform);
                        break;
                    case 2:
                        Instantiate(j == (-mapSize - 1) ? deadendCorner : deadendEdge,
                                new Vector3(-j * 11, 0, -(mapSize + 1) * 11), 
                                Quaternion.Euler(0, 180, 0),
                                parent.transform);
                        break;
                    case 3:
                        Instantiate(j == (-mapSize - 1) ? deadendCorner : deadendEdge,
                                new Vector3(-(mapSize + 1) * 11, 0, j * 11),  
                                Quaternion.Euler(0, 270, 0),
                                parent.transform);
                        break;
                }
            }
        }

        // Assign dialogue loader to the clown room
        GameObject clownRoom = GameObject.Find("ClownPortal");
        if (clownRoom != null) {
            BossPortal portal = clownRoom.GetComponentInChildren(typeof(BossPortal)) as BossPortal;
            if (portal != null) {
                portal.dialogueLoader = dialogueForClownRoom;
            } else {
                Debug.Log("No found portal. No load dialogue for portal");
            }
        } else {
            Debug.Log("No found clown room. No load dialogue for portal");
        }

        isLoadingGame = false;
        isGenerated = true;
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
        while (n > 1)
        {
            n--;
            int k = n - rng.Next(Mathf.Min(dist, n));
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

    private IEnumerator GenerateRoom(int x, int y, int i)
    {
        GameObject generatedRoom = Instantiate(
                roomPool[i-1],
                new Vector3(x * 11, 0, y * 11), 
                Quaternion.Euler(0, rng.Next(4) * 90, 0),
                parent.transform);

        yield return null;
        roomProgress += 1f / roomLimit;
        generatedRooms.Add(generatedRoom);
    }
}