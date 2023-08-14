using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftItem : MonoBehaviour
{
    [SerializeField] private ItemCraftDatabase itemCraftDatabase;
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private GameObject slotCraft;
    [SerializeField] private Transform slotContent;
    //Pour les information
    [SerializeField] private GameObject craftIcon;
    [SerializeField] private Transform craftContent;
    [SerializeField] private Text itemAmountTxt;
    
    private ItemCraft itemCraft;
    private int craftNumber;

    private void Start()
    {
        CreateItemInCraft();
        itemCraft = itemCraftDatabase.itemCraft[0];
        SelectItemCraft(itemCraft);
        slotContent.transform.GetChild(0).GetComponent<Image>().color = Color.red;
    }

    private void Update()
    {
        UpdateTextInCraft();
    }

    private void CreateItemInCraft()
    {
        foreach(ItemCraft item in itemCraftDatabase.itemCraft)
        {
            GameObject slot = Instantiate(slotCraft,slotContent);
            slot.GetComponent<SlotCraft>().UpdateSlot(itemDatabase.item[item.idCraftItem],item);
        }
    }

    public void SelectItemCraft(ItemCraft item)
    {
        itemCraft = item;
        itemAmountTxt.text = item.craftAmount.ToString();
        craftNumber = 1;
        RemoveCraftInformation();
        ResetCraftItemIcon();
        ApplyInformationCraft(item);
    }

    private void ResetCraftItemIcon()
    {
        foreach(Transform slot in slotContent)
        {
            slot.GetComponent<Image>().color = Color.white;
        }
    }

    private void ApplyInformationCraft(ItemCraft item)
    {
        foreach (ItemCraftValue itemValues in item.itemCraft)
        {
            GameObject itemIconObject = Instantiate(craftIcon, craftContent);
            Item newItem = itemDatabase.item[itemValues.idItemToCraft];
            itemIconObject.GetComponent<ItemCraftInfo>().SetItemCraft(newItem);

            int currentAmount = CanvasManager.instance.inventory.GetAllItemNumber(newItem.id);
            int requiredAmount = itemValues.amountCraft * craftNumber;

            itemIconObject.GetComponent<ItemCraftInfo>().UpdateCraftInfo(currentAmount, requiredAmount);
        }
    }

    private void RemoveCraftInformation()
    {
        foreach(Transform child in craftContent)
        {
            Destroy(child.gameObject);
        }
    }

    public void ModifyCraftAmount(int amountModifier)
    {
        SoundManager.instance.Sound(0);
        Item item = itemDatabase.item[itemCraft.idCraftItem];
        int newAMount = itemCraft.craftAmount*(craftNumber+amountModifier);

        if(newAMount>0 && item.maxAmount>=newAMount)
        {
            craftNumber += amountModifier;
            itemAmountTxt.text = newAMount.ToString();
            UpdateTextInCraft();
        }
    }

    private void UpdateTextInCraft()
    {
        if(craftContent.childCount==itemCraft.itemCraft.Length)
        {
            for (int i = 0; i < craftContent.childCount; i++)
            {
                ItemCraftValue itemValues = itemCraft.itemCraft[i];
                Item item = itemDatabase.item[itemValues.idItemToCraft];

                int currentAmount = CanvasManager.instance.inventory.GetAllItemNumber(item.id);
                int requiredAmount = itemValues.amountCraft * craftNumber;

                craftContent.GetChild(i).GetComponent<ItemCraftInfo>().UpdateCraftInfo(currentAmount, requiredAmount);
            }
        }
        
    }

    public void ClickToCraftItem()
    {
        SoundManager.instance.Sound(0);
        if(CanCraftItem())
        {
            CanvasManager.instance.confirmPanel.SetActive(true);
            string texte = "Are you sure you want to create this object ?";
            CanvasManager.instance.confirmPanel.GetComponent<ConfirmPanelScript>().SetConfirmPanel(texte,CraftItemFonction);
        }
        else
        {
            string message = "You don't have enough materials to create this item";
            CanvasManager.instance.SystemMessage(message);
        }
        
    }

    private void CraftItemFonction()
    {
        for (int i = 0; i < craftContent.childCount; i++)
        {
            ItemCraftValue itemValues = itemCraft.itemCraft[i];
            Item item = itemDatabase.item[itemValues.idItemToCraft];
            int requiredAmount = itemValues.amountCraft * craftNumber;
            CanvasManager.instance.inventory.RemoveItem(item.id,requiredAmount);
        }

        SoundManager.instance.Sound(21);
        CanvasManager.instance.questManager.CheckQuestID(itemCraft.idCraftItem,itemCraft.craftAmount * craftNumber,TypeOfQuest.Craft);
        CanvasManager.instance.inventory.AddItemInInventory(itemCraft.idCraftItem,itemCraft.craftAmount * craftNumber);
        SelectItemCraft(itemCraftDatabase.itemCraft[0]);
        slotContent.transform.GetChild(0).GetComponent<Image>().color = Color.red;
    }

    private bool CanCraftItem()
    {
        for (int i = 0; i < craftContent.childCount; i++)
        {
            ItemCraftValue itemValues = itemCraft.itemCraft[i];
            Item item = itemDatabase.item[itemValues.idItemToCraft];

            int currentAmount = CanvasManager.instance.inventory.GetAllItemNumber(item.id);
            int requiredAmount = itemValues.amountCraft * craftNumber;

            if(currentAmount<requiredAmount)
            {
                return false;
            }
        }

        return true;
    }

}
