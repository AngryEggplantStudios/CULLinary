using System.Collections;
using System;
using UnityEditor;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    [SerializeField] public bool onAwake;
    [SerializeField] public bool addCollider;

    [SerializeField] private bool generateAsset;

    private void Awake()
    {
        if (onAwake)
        {
            StartCoroutine(CombineDelay());
        }
    }

    private IEnumerator CombineDelay()
    {
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(CombineMeshes());
    }
    public IEnumerator CombineMeshes()
    {
        Vector3 basePosition = transform.position;
        Quaternion baseRotation = transform.rotation;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        ArrayList materials = new ArrayList();
        ArrayList combineInstanceArrays = new ArrayList();
        MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
        yield return null;

        foreach (MeshFilter meshFilter in meshFilters)
        {
            MeshRenderer meshRenderer = meshFilter.GetComponent<MeshRenderer>();

            if (!meshRenderer ||
                !meshFilter.sharedMesh ||
                meshRenderer.sharedMaterials.Length != meshFilter.sharedMesh.subMeshCount)
            {
                continue;
            }

            for (int s = 0; s < meshFilter.sharedMesh.subMeshCount; s++)
            {
                int materialArrayIndex = Contains(materials, meshRenderer.sharedMaterials[s].name);
                if (materialArrayIndex == -1)
                {
                    materials.Add(meshRenderer.sharedMaterials[s]);
                    materialArrayIndex = materials.Count - 1;
                }
                combineInstanceArrays.Add(new ArrayList());

                CombineInstance combineInstance = new CombineInstance();
                combineInstance.transform = meshRenderer.transform.localToWorldMatrix;
                combineInstance.subMeshIndex = s;
                combineInstance.mesh = meshFilter.sharedMesh;
                (combineInstanceArrays[materialArrayIndex] as ArrayList).Add(combineInstance);
            }
        }
        yield return null;

        // Get / Create mesh filter & renderer
        MeshFilter meshFilterCombine = gameObject.GetComponent<MeshFilter>();
        if (meshFilterCombine == null)
        {
            meshFilterCombine = gameObject.AddComponent<MeshFilter>();
        }
        MeshRenderer meshRendererCombine = gameObject.GetComponent<MeshRenderer>();
        if (meshRendererCombine == null)
        {
            meshRendererCombine = gameObject.AddComponent<MeshRenderer>();
        }
        yield return null;

        // Combine by material index into per-material meshes
        // also, Create CombineInstance array for next step
        Mesh[] meshes = new Mesh[materials.Count];
        CombineInstance[] combineInstances = new CombineInstance[materials.Count];

        for (int m = 0; m < materials.Count; m++)
        {
            CombineInstance[] combineInstanceArray = (combineInstanceArrays[m] as ArrayList).ToArray(typeof(CombineInstance)) as CombineInstance[];
            meshes[m] = new Mesh();
            meshes[m].CombineMeshes(combineInstanceArray, true, true);

            combineInstances[m] = new CombineInstance();
            combineInstances[m].mesh = meshes[m];
            combineInstances[m].subMeshIndex = 0;
        }
        yield return null;

        // Combine into one
        meshFilterCombine.sharedMesh = new Mesh();
        meshFilterCombine.sharedMesh.CombineMeshes(combineInstances, false, false);
        yield return null;

        // Destroy other meshes
        foreach (Mesh oldMesh in meshes)
        {
            oldMesh.Clear();
            DestroyImmediate(oldMesh);
        }
        yield return null;

        // Assign materials
        Material[] materialsArray = materials.ToArray(typeof(Material)) as Material[];
        meshRendererCombine.materials = materialsArray;

        foreach (MeshFilter meshFilter in meshFilters)
        {
            if (meshFilter)
            {
                DestroyImmediate(meshFilter.gameObject);
            }
        }
        if (addCollider)
        {
            gameObject.AddComponent<MeshCollider>();
        }
        
        yield return null;
        transform.position = basePosition;
        yield return null;
        transform.rotation = baseRotation;
        yield return null;

        var mf = gameObject.GetComponent<MeshFilter>();
        if (mf && generateAsset)
        {
            string parentName = gameObject.transform.parent.parent.name;
            string parentIndex = parentName[parentName.Length - 1].ToString();
            var savePath = "Assets/CULLinary/Prefabs/DungeonRooms/Meshes/" + parentIndex + "_" + gameObject.name + ".asset";
            //var savePath = "Assets/CULLinary/Prefabs/DungeonRooms/Meshes/Deadend Edge.asset";
            Debug.Log("Saved Mesh to:" + savePath);
            
            // Comment out this line if building
            //AssetDatabase.CreateAsset(mf.mesh, savePath);
        }
    }

    private int Contains(ArrayList searchList, string searchName)
     {
         for (int i = 0; i < searchList.Count; i++)
         {
             if (((Material)searchList[i]).name == searchName)
             {
                 return i;
             }
         }
         return -1;
     }
}
