using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatSystem : MonoBehaviour
{
    [SerializeField] private GameObject cheatScreen;
    [SerializeField] private InputField cheatInput;
    private bool isCheatScreenActivated = false;
    private KeyCode[] sequence = new KeyCode[]{
        KeyCode.C,
        KeyCode.H,
        KeyCode.E,
        KeyCode.A,
        KeyCode.T
    };
    private int sequenceIndex;
    private void Awake()
    {
        Object.DontDestroyOnLoad(this);
    }
    private void Update()
    {
        if (Input.GetKeyDown(sequence[sequenceIndex]))
        {
            if (++sequenceIndex == sequence.Length)
            {
                sequenceIndex = 0;
                cheatInput.text = "";
                cheatScreen.SetActive(true);
                isCheatScreenActivated = true;
            }
        }
        else if (Input.anyKeyDown)
        {
            sequenceIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch(cheatInput.text)
            {
                case "exit":
                    CloseScreen();
                    cheatInput.text = "";
                    break;
                case "makemeelonmusk":
                    AddMoneyToSaveFile(10000000);
                    cheatInput.text = "";
                    break;
            }
        }
    }

    private void CloseScreen()
    {
        cheatScreen.SetActive(false);
        isCheatScreenActivated = false;
    }

    private void AddMoneyToSaveFile(int amount)
    {
        PlayerData data = SaveSystem.LoadData();
        data.money += amount;
        SaveSystem.SaveData(data);
    }

}
