using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotCraft : MonoBehaviour
{
    private Item item;
    private CraftItem craftItem;
    private ItemCraft itemCraft;

    public void UpdateSlot(Item _item,ItemCraft _itemCraft)
    {
        item = _item;
        itemCraft = _itemCraft;
        transform.GetChild(0).GetComponent<Image>().sprite = item.icon;

        craftItem = GetComponentInParent<CraftItem>();
    }

    public void MouseEnter()
    {
        if (item != null && item.isActive)
        {
            string texteItem = "Rarity : " + item.rarity + '\n' +
                    item.description + '\n' +
                    (item.GetEffectText() != "" ? item.GetEffectText() + '\n' : "") +
                    "Price : " + item.price + " Gold";

            CanvasManager.instance.tooltip.SetTooltip(transform.position, craftItem.gameObject, item.icon, item.name, item.itemType.ToString(), texteItem);
        }
    }

    public void MouseExit()
    {
        CanvasManager.instance.tooltip.gameObject.SetActive(false);
    }

    public void SelectSlotCraft()
    {
        SoundManager.instance.Sound(0);
        craftItem.SelectItemCraft(itemCraft);
        GetComponent<Image>().color = Color.red;
    }
}
