using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loot : MonoBehaviour
{
    [SerializeField] private Item itemForLoot;
    [SerializeField] private int itemNo;

    private GameObject player;
    private AudioSource audioSource;
    [SerializeField] private AudioClip itemPickupSound;

    [SerializeField] private GameObject itemPickupNotif_prefab;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = player.transform.GetComponentInChildren<AudioSource>();
    }

    public Item GetItem()
    {
        return itemForLoot;
    }

    public int GetItemNo()
    {
        return itemNo;
    }

    public void PickUp()
    {
        // Notif
        GameObject itemPickupNotif = Instantiate(itemPickupNotif_prefab);
        itemPickupNotif.transform.SetParent(GameObject.Find("UI").transform);
        itemPickupNotif.transform.GetComponentInChildren<Image>().sprite = itemForLoot.icon;

        // Sound
        audioSource.clip = itemPickupSound;
        audioSource.Play();

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        DungeonPlayerInventory inventory = other.GetComponent<DungeonPlayerInventory>();
        if (inventory != null)
        {
            inventory.AddItemIntoInventory(this);
        }
    }
}
