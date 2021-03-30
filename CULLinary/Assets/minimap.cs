using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimap : MonoBehaviour
{
    [SerializeField] Camera minimapCamera;
    [SerializeField] Transform navArrow;
    [SerializeField] Transform portalIcon;
    [SerializeField] Transform portal;

    Transform playerBody;
    float width;
    float height;

    void Awake()
    {
        playerBody = GameObject.FindGameObjectWithTag("PlayerBody").transform;
    }

    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        width = rt.sizeDelta.x;
        height = rt.sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        navArrow.eulerAngles = new Vector3(0, 0, -playerBody.eulerAngles.y);
        Vector3 portalScreenPos = minimapCamera.WorldToScreenPoint(portal.position);
        Vector3 portalLocalPos = new Vector3(
                portalScreenPos.x - width/2, 
                portalScreenPos.y - height/2, 
                0);
        
        if (portalLocalPos.magnitude > width/2)
        {
            portalLocalPos = portalLocalPos.normalized * width/2;
        }
        
        portalIcon.GetComponent<RectTransform>().anchoredPosition = portalLocalPos;
    }
}
