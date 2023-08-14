using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCraftInfo : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Text itemValueTxt;

    private Item item;

    public Item GetItem()
    {
        return item;
    }

    public void SetItemCraft(Item _item)
    {
        item = _item;
        itemIcon.sprite = item.icon;
    }

    public void UpdateCraftInfo(int currentAmount, int requiredAmount)
    {
        string craftAmountText = currentAmount + " / " + requiredAmount;
        Color textColor = currentAmount < requiredAmount ? Color.red : Color.white;

        itemValueTxt.color = textColor;
        itemValueTxt.text = craftAmountText;
    }

    public void MouseEnter()
    {
        if (item != null && item.isActive)
        {
            string texteItem = "Rarity : " + item.rarity + '\n' +
                    item.description + '\n' +
                    (item.GetEffectText() != "" ? item.GetEffectText() + '\n' : "") +
                    "Price : " + item.price + " Gold";

            GameObject panel = GetComponentInParent<CraftItem>().gameObject;

            CanvasManager.instance.tooltip.SetTooltip(transform.position, panel, item.icon, item.name, item.itemType.ToString(), texteItem);
        }
    }

    public void MouseExit()
    {
        CanvasManager.instance.tooltip.gameObject.SetActive(false);
    }
}
