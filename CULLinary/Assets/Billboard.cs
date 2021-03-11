using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;
    void Start()
    {
        //This gets the Main Camera from the Scene
        this.cam = Camera.main.transform;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);    
    }
}
