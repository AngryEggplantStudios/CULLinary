using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimap : MonoBehaviour
{
    [SerializeField] Camera minimapCamera;
    [SerializeField] Transform navArrow;
    [SerializeField] Transform portalIcon;
    [SerializeField] Transform clownIcon;

    Transform playerBody;
    Transform portal;
    Transform clown;
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

        GameObject temp;
        if (temp = GameObject.Find("DungeonPortal")) portal = temp.transform;
        if (temp = GameObject.Find("ClownPortal")) clown = temp.transform;
    }

    void Update()
    {
        navArrow.eulerAngles = new Vector3(0, 0, -playerBody.eulerAngles.y);
        SetIconPos(portalIcon, portal);
        SetIconPos(clownIcon, clown);
    }

    void SetIconPos(Transform icon, Transform target) {
        if (target == null)
        {
            icon.gameObject.SetActive(false);
            return;
        }

        icon.gameObject.SetActive(true);
        Vector3 screenPos = minimapCamera.WorldToScreenPoint(target.position);
        Vector3 localPos = new Vector3(
                screenPos.x - width/2, 
                screenPos.y - height/2, 
                0);
        
        if (localPos.magnitude > width/2)
        {
            localPos = localPos.normalized * width/2;
        }
        
        icon.GetComponent<RectTransform>().anchoredPosition = localPos;
    }
}
