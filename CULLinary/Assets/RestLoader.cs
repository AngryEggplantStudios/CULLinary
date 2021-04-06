using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestLoader : MonoBehaviour
{
    private void Update()
    {
        StartCoroutine(ShowAutosave());
    }

    private IEnumerator ShowAutosave()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

}