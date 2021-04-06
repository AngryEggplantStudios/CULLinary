using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopLoader : MonoBehaviour
{
    [SerializeField] private Text text;
    private void Update()
    {
        if (PopulateShop.isPopulated)
        {
            StartCoroutine(Delay());
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

}
