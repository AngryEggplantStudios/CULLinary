using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiButtonClose : MonoBehaviour
{
    public GameObject ui;
    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ui.SetActive(false);
        }
    }
}
