using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheatSystem : MonoBehaviour
{
    [SerializeField] private GameObject cheatScreen;
    [SerializeField] private InputField cheatInput;
    [SerializeField] private Text successUI;
    private bool isCheatScreenActivated = false;
    private KeyCode[] sequence = new KeyCode[]{
        KeyCode.C,
        KeyCode.H,
        KeyCode.E,
        KeyCode.A,
        KeyCode.T
    };
    private int sequenceIndex;
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
                    ChangeSuccessStatus("Oh my you are rich daddy~");
                    cheatInput.text = "";
                    break;
                case "makemepoor": //to delete, for testing
                    SetMoneyOnSaveFile(1000);
                    ChangeSuccessStatus("Time to get a job");
                    cheatInput.text = "";
                    break;
                case "makemegoboss": //Go boss room
                    cheatInput.text = "";
                    ChangeSuccessStatus("Are you ready to rumble? Starting in 5 seconds...");
                    StartCoroutine(GoBossRoom());
                    break;
                case "reset":
                    cheatInput.text = "";
                    ChangeSuccessStatus("Resetting save file to default");
                    ResetSaveFile();
                    break;
                case "makemegordonramsay":
                    cheatInput.text = "";
                    ChangeSuccessStatus("It's fooking raw!!!");
                    SetInventoryOnSaveFile(3);
                    break;
            }
        }
    }

    private void CloseScreen()
    {
        cheatScreen.SetActive(false);
        isCheatScreenActivated = false;
    }

    private void SetInventoryOnSaveFile(int count)
    {
        //4,6,7,11 

        PlayerData data = SaveSystem.LoadData();
        int[] currentItems = new int[4]{4, 6, 12, 11};
        InventoryItemData[] items = new InventoryItemData[4];
        for (int i=0; i < items.Length; i++)
        {
            InventoryItemData gameItem = new InventoryItemData(currentItems[i], count);
            items[i] = gameItem;
        }
        data.SetInventoryString(JsonArrayParser.ToJson(items, true));
        SaveSystem.SaveData(data);
    }

    private void ChangeSuccessStatus(string str)
    {
        successUI.text = str;
    }

    private IEnumerator GoBossRoom()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene((int)SceneIndexes.BOSS);
    }


    private void SetMoneyOnSaveFile(int amount)
    {
        PlayerData data = SaveSystem.LoadData();
        data.SetMoney(amount);
        SaveSystem.SaveData(data);
    }

    private void ResetSaveFile()
    {
        PlayerData data = new PlayerData();
        data.SetCurrentIndex((int)SceneIndexes.REST);
        SaveSystem.SaveData(data);
    }

    private void AddMoneyToSaveFile(int amount)
    {
        PlayerData data = SaveSystem.LoadData();
        data.SetMoney(data.GetMoney() + amount);
        SaveSystem.SaveData(data);
    }

}
