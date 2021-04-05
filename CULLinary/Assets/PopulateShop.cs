using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateShop : MonoBehaviour
{
   [SerializeField] private GameObject[] vitaminSlots;
   [SerializeField] private VitaminDatabase vitaminDatabase;
   [SerializeField] private GameObject[] weaponSlots;
   [SerializeField] private WeaponDatabase weaponDatabase;
   [SerializeField] private GameObject[] keyItemSlots;
   [SerializeField] private KeyItemDatabase KeyItemDatabase;
   [SerializeField] private ShopMenu shopMenu;

   public static bool isPopulated = false;
   private List<Vitamin> vitaminList = new List<Vitamin>();
   private List<Weapon> weaponList = new List<Weapon>();
   private List<KeyItem> keyItemList = new List<KeyItem>();

   private void Start()
   {
        isPopulated = false;
        vitaminList = vitaminDatabase.allVitamins;
        weaponList = weaponDatabase.allWeapons;
        keyItemList = KeyItemDatabase.allKeyItems;
        StartCoroutine(Populate());
   }

   private IEnumerator Populate()
   {
        yield return StartCoroutine(PopulateVitaminPanel());
        yield return StartCoroutine(PopulateWeaponPanel());
        yield return StartCoroutine(PopulateKeyItemPanel());
        shopMenu.SelectVitaminPanel();
        isPopulated = true;
   }

   public IEnumerator UpdateUI(int currentPanelSelected)
   {
        shopMenu.SetAllPanelsActive();
        yield return StartCoroutine(PopulateVitaminPanel());
        yield return StartCoroutine(PopulateWeaponPanel());
        yield return StartCoroutine(PopulateKeyItemPanel());
        switch (currentPanelSelected)
        {
            case 0:
                shopMenu.SelectVitaminPanel();
                break;
            case 1:
                shopMenu.SelectWeaponPanel();
                break;
            case 2:
                shopMenu.SelectKeyItemPanel();
                break;
        }
   }

   private IEnumerator PopulateKeyItemPanel()
   {
       int count = 0;
       yield return null;
       foreach (KeyItem k in keyItemList)
       {
            yield return null;
            GameObject slot = keyItemSlots[count];
            Button btn = slot.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            SetupSlot(k, slot, btn);
            yield return null;
            
            if (PlayerManager.playerData.GetIfKeyItemBoughtById(k.GetID())) //Should combine with the below else if statement and make the button uninteractable
            {
                btn.onClick.AddListener(() => { shopMenu.SelectAlreadyBought(); });
            }
            else if (PlayerManager.playerData.GetMoney() < k.GetPrice())
            {
                btn.onClick.AddListener(() => { shopMenu.SelectNoMoney(); });
            }
            else 
            {
                btn.onClick.AddListener(() => { shopMenu.SelectItem(k); });
            }
            
            count++;
       }
        //Cleanup
       for (int i=count; i < keyItemSlots.Length; i++)
       {
            GameObject slot = keyItemSlots[i];
            slot.SetActive(false);
       }
   }

   private IEnumerator PopulateWeaponPanel()
   {
       int count = 0;
       foreach (Weapon w in weaponList)
       {
            yield return null;
            GameObject slot = weaponSlots[count];
            Button btn = slot.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            SetupSlot(w, slot, btn);
            yield return null;

            if (PlayerManager.playerData.GetIfWeaponBoughtById(w.GetID())) //Should combine with the below else if statement and make the button uninteractable
            {
                btn.onClick.AddListener(() => { shopMenu.SelectAlreadyBought(); });
            }
            else if (PlayerManager.playerData.GetMoney() < w.GetPrice())
            {
                btn.onClick.AddListener(() => { shopMenu.SelectNoMoney(); });
            }
            else 
            {
                btn.onClick.AddListener(() => { shopMenu.SelectItem(w); });
            }
            count++;
       }
        //Cleanup
       for (int i=count; i < weaponSlots.Length; i++)
       {
            GameObject slot = weaponSlots[i];
            slot.SetActive(false);
       }
   }

   private IEnumerator PopulateVitaminPanel()
   {
       int count = 0;
       foreach (Vitamin v in vitaminList)
       {
            yield return null;
            GameObject slot = vitaminSlots[count];
            Button btn = slot.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            SetupSlot(v, slot, btn);
            yield return null;
            if (PlayerManager.playerData.GetMoney() < v.GetPrice())
            {
                btn.onClick.AddListener(() => { shopMenu.SelectNoMoney(); });
            }
            else
            {
                btn.onClick.AddListener(() => { shopMenu.SelectItem(v); });
            }
            count++;
       }
       //Cleanup
       for (int i=count; i < vitaminSlots.Length; i++)
       {
            GameObject slot = vitaminSlots[i];
            slot.SetActive(false);
       }
   }

   private void SetupSlot(ShopItem item, GameObject slot, Button btn)
   {
        Text[] slotInfo = slot.GetComponentsInChildren<Text>();
        TooltipTrigger tooltipTrigger = slot.GetComponent<TooltipTrigger>();
        slotInfo[0].text = item.GetPrice().ToString();
        slotInfo[1].text = item.GetName();
        tooltipTrigger.SetContent(item.GetDescription());
        tooltipTrigger.SetHeader(item.GetName());
        Image[] image = slot.GetComponentsInChildren<Image>();
        image[1].sprite = item.GetSprite();
   }

   private void OnDestroy()
   {
       foreach(GameObject slot in vitaminSlots)
       {
           Button btn = slot.GetComponent<Button>();
           btn.onClick.RemoveAllListeners();
       }
   }

}
