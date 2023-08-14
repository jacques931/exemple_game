using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotShop : MonoBehaviour
{
    private Item item;

    public void UpdateSlot(Item _item)
    {
        item = _item;
        transform.GetChild(0).GetComponent<Image>().sprite = item.icon;
    }

    public void MouseEnter()
    {
        if (item != null && item.isActive)
        {
            string texteItem = "Rarity : " + item.rarity + '\n' +
                    item.description + '\n' +
                    (item.GetEffectText() != "" ? item.GetEffectText() + '\n' : "") +
                    "Price : " + item.priceInShop + " Gold";

            GameObject itemPanel = GetComponentInParent<Shop>().gameObject;

            CanvasManager.instance.tooltip.SetTooltip(transform.position, itemPanel, item.icon, item.name, item.itemType.ToString(), texteItem);
        }
    }

    public void MouseExit()
    {
        CanvasManager.instance.tooltip.gameObject.SetActive(false);
    }

    public void ClickBuyItem()
    {
        SoundManager.instance.Sound(0);
        if(item.priceInShop<=PlayerPrefs.GetInt("money"))
        {
            CanvasManager.instance.buyShop.gameObject.SetActive(true);
            CanvasManager.instance.buyShop.SetBuyShop(item,transform.position,true);
        }
        else
        {
            string texte = "You don't have enough money to buy this item";
            CanvasManager.instance.SystemMessage(texte,0.7f);
        }
    }
}
