using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateShop : MonoBehaviour
{
   [SerializeField] private Transform vitaminGrid;
   [SerializeField] private VitaminDatabase vitaminDatabase;
   [SerializeField] private Transform weaponGrid;
   [SerializeField] private WeaponDatabase weaponDatabase;
   [SerializeField] private Transform keyItemGrid;
   [SerializeField] private KeyItemDatabase KeyItemDatabase;
   [SerializeField] private ShopMenu shopMenu;
   [SerializeField] private GameObject shopButton_prefab;

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
       foreach (Transform child in keyItemGrid)
       {
           Destroy(child.gameObject);
       }
       foreach (KeyItem k in keyItemList)
       {
            yield return null;
            GameObject slot = Instantiate(shopButton_prefab, keyItemGrid);
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
       }
   }

   private IEnumerator PopulateWeaponPanel()
   {
        foreach (Transform child in weaponGrid)
        {
           Destroy(child.gameObject);
        }
        foreach (Weapon w in weaponList)
        {
            yield return null;
            GameObject slot = Instantiate(shopButton_prefab, weaponGrid);
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
        }
    }

   private IEnumerator PopulateVitaminPanel()
   {
        foreach (Transform child in vitaminGrid)
       {
           Destroy(child.gameObject);
       }
       foreach (Vitamin v in vitaminList)
       {
            yield return null;
            GameObject slot = Instantiate(shopButton_prefab, vitaminGrid);
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
       // What does this even do...
       /* foreach(GameObject slot in vitaminSlots)
       {
           Button btn = slot.GetComponent<Button>();
           btn.onClick.RemoveAllListeners();
       } */
   }

}
