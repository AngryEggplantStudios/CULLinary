using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTips : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private TipsDatabase tipsDatabase;

    private void Start()
    {
        string randomTip = tipsDatabase.tips[Random.Range(0, tipsDatabase.tips.Count)];
        text.text = "TIP: " + randomTip;
    }
}
