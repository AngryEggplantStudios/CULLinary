using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiOpenRecipeMenu : MonoBehaviour
{
    public GameObject ui;
    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider != null && hit.collider.gameObject.tag == "RecipeMenuActivator")
            {
                ui.SetActive(true);
            }
        }
    }
}
