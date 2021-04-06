using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveIcon : MonoBehaviour
{
    [SerializeField] private Image img;
   
    public void Start()
    {
        StartCoroutine(FadeImage());
    }
 
    private IEnumerator FadeImage()
    {
        while (true)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }

            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }

    }   
}
