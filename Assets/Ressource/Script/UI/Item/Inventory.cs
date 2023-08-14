using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [SerializeField] private ItemDatabase itemData;
    private List<Item> items = new List<Item>();
    [SerializeField] private Transform itemSlotGroup;
    [SerializeField] private Text goldTxt;

    private void Awake()
    {
        SetDataItem();
        SetGoldCanvas();
        ApplyItemSlot();
        SetPositionSlot();
        gameObject.SetActive(false);
    }

    public bool InventoryIsFull(int idItem, int amount)
    {
        foreach (Transform itemSlotTransform in itemSlotGroup)
        {
            foreach (Transform slotTransform in itemSlotTransform)
            {
                SlotScript slot = slotTransform.GetComponent<SlotScript>();
                Item item = slot.GetItem();

                if (item == null || !item.isActive || (item.id == idItem && item.amount + amount <= item.maxAmount))
                {
                    return false; // Si un emplacement est disponible ou s'il y a de la place pour l'objet, l'inventaire n'est pas plein.
                }
            }
        }
        return true; // Si aucun emplacement n'est disponible, l'inventaire est plein.
    }

    public void RemoveItem(int idItem, int totalAmount=1)
    {
        foreach (Transform itemSlotTransform in itemSlotGroup)
        {
            foreach (Transform slotTransform in itemSlotTransform)
            {
                SlotScript slot = slotTransform.GetComponent<SlotScript>();
                Item item = slot.GetItem();

                if (item != null && item.isActive && item.id == idItem)
                {
                    int amountToRemove = Mathf.Min(item.amount, totalAmount);
                    slot.RemoveItem(amountToRemove);
                    totalAmount -= amountToRemove;

                    if (totalAmount <= 0)
                        return;
                }
            }
        }
    }

    public void RemoveItemInventory(int idItemPosition,int amount=1)
    {
        foreach (Transform itemSlotTransform in itemSlotGroup)
        {
            foreach (Transform slotTransform in itemSlotTransform)
            {
                SlotScript slot = slotTransform.GetComponent<SlotScript>();
                if (slot.GetPosition() == idItemPosition)
                {
                    slot.RemoveItem(amount);
                    return;
                }
            }
        }
    }

    public void RecoveryItem()
    {
        List<Item> newItems = new List<Item>();
        for(int i=0;i<itemSlotGroup.childCount;i++)
        {
            for(int j=0;j<itemSlotGroup.GetChild(i).childCount;j++)
            {
                int number = i*itemSlotGroup.GetChild(i).childCount + j;
                SlotScript slot = itemSlotGroup.GetChild(i).GetChild(j).GetComponent<SlotScript>();
                if(slot.GetItem()!=null && slot.GetItem().isActive)
                {
                    newItems.Add(slot.GetItem());
                }
                else
                {
                    Item newItem = new Item{isActive=false};
                    newItems.Add(newItem);
                }
            }
        }

        items = newItems;
        ItemManager.Instance.SaveItemList(items.ToArray());
    }

    public int GetAllItemNumber(int idItem)
    {
        int totalItem = 0;
        for(int i=0;i<items.Count;i++)
        {
            if(items[i].id == idItem)
            {
                totalItem += items[i].amount;
            }
        }

        return totalItem;
    }

    public void SetGoldCanvas()
    {
        goldTxt.text = PlayerPrefs.GetInt("money") + " Gold";
    }

    private void SetDataItem()
    {
        items = ItemManager.Instance.LoadItemList().ToList(); // Load Item in Database
        for(int i=0;i<items.Count;i++)
        {
            items[i].SetImage(itemData.item[items[i].id].icon);
        }
    }

    public void SaveItem(Item item,int itemPosition,bool isDestroy)
    {
        items[itemPosition] = item;
        if(isDestroy)
            items[itemPosition].id = 0;
        ItemManager.Instance.SaveItemList(items.ToArray());
    }

    public ItemDatabase GetItemDatabase()
    {
        return itemData;
    }

    public int CheckItemInInventory(int idItem)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (idItem!=0 && items[i].id == idItem)
            {
                return i;
            }
        }
        return -1; // Retourne -1 si l'élément n'est pas trouvé dans la liste
    }

    private void SetPositionSlot()
    {
        for(int i=0;i<itemSlotGroup.childCount;i++)
        {
            for(int j=0;j<itemSlotGroup.GetChild(i).childCount;j++)
            {
                int number = i*itemSlotGroup.GetChild(i).childCount + j;
                SlotScript slot = itemSlotGroup.GetChild(i).GetChild(j).GetComponent<SlotScript>();
                slot.SetPosition(number);
            }
        }
    }

    private void ApplyItemSlot()
    {
        for(int i=0;i<itemSlotGroup.childCount;i++)
        {
            for(int j=0;j<itemSlotGroup.GetChild(i).childCount;j++)
            {
                int number = i*itemSlotGroup.GetChild(i).childCount + j; // Il ont toujours le meme nombre d'enfant
                if(items.Count<=number)
                {
                    ItemManager.Instance.SaveItemList(items.ToArray());
                    return;
                }
                SlotScript slot = itemSlotGroup.GetChild(i).GetChild(j).GetComponent<SlotScript>();
                if(items[number].icon !=null && items[number].isActive)
                {
                    slot.UpdateSlot(items[number]);
                }
                    
            }
        }
        ItemManager.Instance.SaveItemList(items.ToArray());

    }

    public int GetIdItemToUnlockSkill(int idMonster)
    {
        for(int i=0;i<itemData.item.Length;i++)
        {
            if(itemData.item[i].itemType == ItemType.Unlock_Skill)
            {
                if(itemData.item[i].itemEffect[0].valueEffect == idMonster)
                {
                    return itemData.item[i].id;
                }
            }
        }

        return -1;
    }

    public void AddItemInInventory(int idItem,int itemAmount) // Normalement, l'id et la meme chose que la postion dans la liste
    {   
        int idInInventory = CheckItemInInventory(idItem);
        if(idInInventory !=-1)
        {
            if(items[idInInventory].isActive && items[idInInventory].amount + itemAmount<=itemData.item[idItem].maxAmount)
            {
                items[idInInventory].amount +=itemAmount;
                CanvasManager.instance.questFrame.ApplyText();
            }
            else
            {
                CreateNewItem(idItem,itemAmount);
            }
        }
        else
        {
            CreateNewItem(idItem,itemAmount);
        }
        ApplyItemSlot();
        
    }

    public bool CanHaveMultiItem(int idItem)
    {
        Item item = itemData.item[idItem];
        if(item.maxAmount!=-1)
        {
            return true;
        }
        return false;
    }

    private void CreateNewItem(int idItem,int itemAmount)
    {
        if(!InventoryIsFull(idItem,itemAmount))
        {
            Item addItem = itemData.item[idItem];
            addItem.amount = addItem.maxAmount>1? itemAmount : 1;
            int idPosition = CheckItemNull();
            if(idPosition!=-1)
            {
                items[idPosition] = addItem;
            }
            else
            {
                items.Add(addItem);
            }
            CanvasManager.instance.questFrame.ApplyText();
        }
        else
        {
            string message = "Your inventory is full !";
            CanvasManager.instance.SystemMessage(message);
        }
    }

    private int CheckItemNull()
    {
        for(int i=0;i<items.Count;i++)
        {
            if(!items[i].isActive)
            {
                return i;
            }
        }

        return -1;
    }



}
