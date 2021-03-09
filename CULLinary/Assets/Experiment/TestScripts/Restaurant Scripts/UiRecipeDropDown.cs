using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiRecipeDropDown : MonoBehaviour
{
    public TextMeshPro textElement;
    public Collider textCollider;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider == textCollider)
            {
                Debug.Log(textElement.text + " selected!");
            }
        }
    }
}
