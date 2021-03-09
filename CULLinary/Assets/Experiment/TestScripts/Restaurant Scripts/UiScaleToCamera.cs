using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiScaleToCamera : MonoBehaviour
{
    public GameObject ui;
    public Camera cam;

    // Original window size when I positioned the screen
    int originalHeight = 547;
    int originalWidth = 1280;

    // Start is called before the first frame update
    void Start()
    {
        int height = cam.pixelHeight;
        int width = cam.pixelWidth;

        Debug.Log("height : " + height + "width: " + width);
        ui.transform.localScale = new Vector3(ui.transform.localScale.x * height / originalHeight,
                                              ui.transform.localScale.y * width / originalWidth,
                                              ui.transform.localScale.z);
    }

    // Update is called once per frame
    void Update() { }
}
