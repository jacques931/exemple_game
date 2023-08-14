using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemBar : MonoBehaviour
{
    [SerializeField] private ItemDatabase itemDatabase;
    private List<Item> items = new List<Item>();

    private void Start()
    {
        SetPositionSlot();
        SetDataItem();
        ApplyItemSlot();
    }

    private void SetPositionSlot()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            SlotScript slot = transform.GetChild(i).GetChild(0).GetComponent<SlotScript>();
            slot.SetPosition(i);
        }
    }

    private void SetDataItem()
    {
        items = ItemManager.Instance.LoadItemList("itemBar.json").ToList(); // Load Item in Database
        for(int i=0;i<items.Count;i++)
        {
            items[i].SetImage(itemDatabase.item[items[i].id].icon);
        }
    }

    public void AddOrUpdateItem(Item item,int idBarPosition)
    {
        if (idBarPosition >= 0 && idBarPosition < items.Count)
        {
            // L'index se trouve dans la plage actuelle des éléments, mettez simplement à jour l'élément existant.
            items[idBarPosition] = item;
        }
        else if (idBarPosition >= items.Count)
        {
            // L'index est en dehors de la plage actuelle des éléments, agrandissez la liste et ajoutez le nouvel élément.
            for (int i = items.Count; i < idBarPosition; i++)
            {
                items.Add(new Item{isActive=false});
            }

            items.Add(item);
        }

        ItemManager.Instance.SaveItemList(items.ToArray(),"itemBar.json");
    }

    public void RemoveItem(int idItem)
    {
        foreach (Transform slotTransform in transform)
        {
            SlotScript slot = slotTransform.GetChild(0).GetComponent<SlotScript>();
            if (slot.GetItem() !=null && slot.GetItem().isActive && slot.GetItem().id == idItem)
            {
                slot.RemoveItem(1);
                break;
            }
        }
    }

    public void RemoveItemBar(int idItemPosition,int amount=1)
    {
        foreach (Transform slotTransform in transform)
        {
            SlotScript slot = slotTransform.GetChild(0).GetComponent<SlotScript>();
            if (slot.GetPosition() == idItemPosition)
            {
                slot.RemoveItem(amount);
                return;
            }
        }
    }

    private void ApplyItemSlot()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            if(items.Count<=i)
            {
                ItemManager.Instance.SaveItemList(items.ToArray(),"itemBar.json");
                break;
            }
            SlotScript slot = transform.GetChild(i).GetChild(0).GetComponent<SlotScript>();
            if(items[i].icon !=null && items[i].isActive)
            {
                slot.UpdateSlot(items[i]);
            }
        }
        ItemManager.Instance.SaveItemList(items.ToArray(),"itemBar.json");
    }

    public void SaveItem(Item item,int itemPosition,bool isDestroy)
    {
        items[itemPosition] = item;
        if(isDestroy)
            items[itemPosition].id = 0;
        ItemManager.Instance.SaveItemList(items.ToArray(),"itemBar.json");
    }

    public int CheckItemInBar(int idItem)
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
}
