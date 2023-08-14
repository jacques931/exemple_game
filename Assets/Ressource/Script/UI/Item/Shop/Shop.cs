using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private GameObject itemSlot;
    [SerializeField] private Transform slotContent;

    private void Start()
    {
        CreateItemInShop();
    }

    private void CreateItemInShop()
    {
        foreach(Item item in itemDatabase.item)
        {
            if(item.isInShop)
            {
                GameObject itemPanel = Instantiate(itemSlot,slotContent);
                itemPanel.GetComponent<SlotShop>().UpdateSlot(item);
            }
            
        }
    }
}
