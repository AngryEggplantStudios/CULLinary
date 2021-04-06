using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestLoader : MonoBehaviour
{
    [SerializeField] private Text text;
    private void Update()
    {
        text.text = "Loading Restaurant...";
        StartCoroutine(ShowAutosave());
    }

    private IEnumerator ShowAutosave()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

}