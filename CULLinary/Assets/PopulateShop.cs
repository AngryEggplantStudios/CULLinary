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
   private List<Button> vitaminButtons = new List<Button>();
   private List<Button> weaponButtons = new List<Button>();
   private List<Button> keyItemButtons = new List<Button>();
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
        shopMenu.SetAllPanelsActive();
        yield return StartCoroutine(PopulateVitaminPanel());
        yield return StartCoroutine(PopulateWeaponPanel());
        yield return StartCoroutine(PopulateKeyItemPanel());
        shopMenu.SelectUponStart();
        isPopulated = true;
   }

   public IEnumerator UpdatePanel(int currentPanelSelected)
   {
        yield return null;
        switch (currentPanelSelected)
        {
            case 0:
                yield return StartCoroutine(UpdateVitaminPanel());
                break;
            case 1:
                yield return StartCoroutine(UpdateWeaponPanel());
                break;
            case 2:
                yield return StartCoroutine(UpdateKeyItemPanel());
                break;
        }
   }

   public IEnumerator PopulateKeyItemPanel()
   {
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
                BlankOutButton(btn);
                btn.onClick.AddListener(() => { shopMenu.SelectAlreadyBought(); });
            }
            else if (PlayerManager.playerData.GetMoney() < k.GetPrice())
            {
                BlankOutButton(btn);
                btn.onClick.AddListener(() => { shopMenu.SelectNoMoney(); });
            }
            else 
            {
                BlankOutButton(btn);
                ReactivateButton(btn);
                btn.onClick.AddListener(() => { shopMenu.SelectItem(k); });
            }
            keyItemButtons.Add(btn);
       }
       
   }

   public IEnumerator PopulateWeaponPanel()
   {
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
                BlankOutButton(btn);
                btn.onClick.AddListener(() => { shopMenu.SelectAlreadyBought(); });
            }
            else if (PlayerManager.playerData.GetMoney() < w.GetPrice())
            {
                BlankOutButton(btn);
                btn.onClick.AddListener(() => { shopMenu.SelectNoMoney(); });
            }
            else 
            {
                ReactivateButton(btn);
                btn.onClick.AddListener(() => { shopMenu.SelectItem(w); });
            }
            weaponButtons.Add(btn);
        }
    }

   public IEnumerator PopulateVitaminPanel()
   {
       foreach (Vitamin v in vitaminList)
       {
            yield return null;
            GameObject slot = Instantiate(shopButton_prefab, vitaminGrid);
            Button btn = slot.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            SetupSlot(v, slot, btn);
            yield return null;
            if (v.healthHeal > 0 && PlayerManager.playerData.GetMaxHealth() == PlayerManager.playerData.GetCurrentHealth())
            {
                BlankOutButton(btn);
                btn.onClick.AddListener(() => { shopMenu.MaxHealthWarning(); });
            }
            else if (PlayerManager.playerData.GetMoney() < v.GetPrice())
            {
                BlankOutButton(btn);
                btn.onClick.AddListener(() => { shopMenu.SelectNoMoney(); });
            }
            else
            {
                ReactivateButton(btn);
                btn.onClick.AddListener(() => { shopMenu.SelectItem(v); });
            }
            vitaminButtons.Add(btn);
       }
   }

   public IEnumerator UpdateVitaminPanel()
   {
       int count = 0;
       foreach (Vitamin v in vitaminList)
       {
            yield return null;
            Button btn = vitaminButtons[count];
            btn.onClick.RemoveAllListeners();
            if (v.healthHeal > 0 && PlayerManager.playerData.GetMaxHealth() == PlayerManager.playerData.GetCurrentHealth())
            {
                BlankOutButton(btn);
                btn.onClick.AddListener(() => { shopMenu.MaxHealthWarning(); });
            }
            else if (PlayerManager.playerData.GetMoney() < v.GetPrice())
            {
                BlankOutButton(btn);
                btn.onClick.AddListener(() => { shopMenu.SelectNoMoney(); });
            }
            else
            {
                ReactivateButton(btn);
                btn.onClick.AddListener(() => { shopMenu.SelectItem(v); });
            }
            count++;
       }
   }

    public IEnumerator UpdateWeaponPanel()
    {
        int count = 0;
        foreach (Weapon w in weaponList)
        {
            yield return null;
            Button btn = weaponButtons[count];
            btn.onClick.RemoveAllListeners();
            if (PlayerManager.playerData.GetIfWeaponBoughtById(w.GetID())) //Should combine with the below else if statement and make the button uninteractable
            {
                BlankOutButton(btn);
                btn.onClick.AddListener(() => { shopMenu.SelectAlreadyBought(); });
            }
            else if (PlayerManager.playerData.GetMoney() < w.GetPrice())
            {
                BlankOutButton(btn);
                btn.onClick.AddListener(() => { shopMenu.SelectNoMoney(); });
            }
            else 
            {
                ReactivateButton(btn);
                btn.onClick.AddListener(() => { shopMenu.SelectItem(w); });
            }
            count++;
        }
    }

    public IEnumerator UpdateKeyItemPanel()
    {
        int count = 0;
        foreach (KeyItem k in keyItemList)
        {
            yield return null;
            Button btn = keyItemButtons[count];
            btn.onClick.RemoveAllListeners();
            if (PlayerManager.playerData.GetIfKeyItemBoughtById(k.GetID())) //Should combine with the below else if statement and make the button uninteractable
            {
                BlankOutButton(btn);
                btn.onClick.AddListener(() => { shopMenu.SelectAlreadyBought(); });
            }
            else if (PlayerManager.playerData.GetMoney() < k.GetPrice())
            {
                BlankOutButton(btn);
                btn.onClick.AddListener(() => { shopMenu.SelectNoMoney(); });
            }
            else 
            {
                ReactivateButton(btn);
                btn.onClick.AddListener(() => { shopMenu.SelectItem(k); });
            }
            count++;
        }
    }

   private void BlankOutButton(Button button)
   {
       Image[] imgs = button.GetComponentsInChildren<Image>();
       foreach (Image img in imgs)
       {
           img.color = Color.grey;
       }
   }

   private void ReactivateButton(Button button)
   {
       Image[] imgs = button.GetComponentsInChildren<Image>();
       foreach (Image img in imgs)
       {
           img.color = Color.white;
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

}
