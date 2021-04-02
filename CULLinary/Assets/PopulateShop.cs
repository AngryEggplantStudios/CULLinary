using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateShop : MonoBehaviour
{
   [SerializeField] private GameObject[] vitaminSlots;
   [SerializeField] private VitaminDatabase vitaminDatabase;
   [SerializeField] private ShopMenu shopMenu;

   public static bool isPopulated = false;
   private List<Vitamin> vitaminList = new List<Vitamin>();

   private void Start()
   {
       vitaminList = vitaminDatabase.allVitamins;
       StartCoroutine(PopulateVitaminPanel());
   }

   private IEnumerator PopulateVitaminPanel()
   {
       int count = 0;
       foreach (Vitamin v in vitaminList)
       {
           yield return null;
           GameObject slot = vitaminSlots[count];
           Button btn = slot.GetComponent<Button>();
           Text[] slotInfo = slot.GetComponentsInChildren<Text>();
           TooltipTrigger tooltipTrigger = slot.GetComponent<TooltipTrigger>();
           yield return null;
           slotInfo[0].text = v.GetPrice().ToString();
           slotInfo[1].text = v.GetName();
           tooltipTrigger.SetContent(v.GetDescription());
           tooltipTrigger.SetHeader(v.GetName());
           Image[] image = slot.GetComponentsInChildren<Image>();
           image[1].sprite = v.GetSprite();
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
