using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private Item itemForLoot;
    [SerializeField] private int itemNo;

    private GameObject player;
    private AudioSource audioSource;
    [SerializeField] private AudioClip itemPickupSound;

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
        audioSource.clip = itemPickupSound;
        audioSource.Play();
        Debug.Log("PICKUP");
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
