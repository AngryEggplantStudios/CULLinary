using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopLoader : MonoBehaviour
{
    private void Update()
    {
        if (PopulateShop.isPopulated)
        {
            StartCoroutine(Delay());
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

}
