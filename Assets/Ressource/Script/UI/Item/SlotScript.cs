using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour
{
    private Item item;
    private Sprite startSprite;
    private int positionSlot;

    //Click Variable
    private bool isWaitingForDoubleClick = false;
    private float doubleClickDelay = 0.2f;

    public Item GetItem()
    {
        return item;
    }

    public void SetPosition(int position)
    {
        positionSlot = position;
    }

    public int GetPosition()
    {
        return positionSlot;
    }

    public void UpdateSlot(Item _item)
    {
        item = _item;
        if (startSprite == null)
            startSprite = transform.GetChild(0).GetComponent<Image>().sprite;

        Image iconButton = transform.GetChild(0).GetComponent<Image>();
        Text amountTxt = transform.GetChild(1).GetComponent<Text>();

        iconButton.sprite = item.icon;
        amountTxt.gameObject.SetActive(item.maxAmount > 1);
        amountTxt.text = item.amount.ToString();

        if (IsInItemBar())
        {
            CanvasManager.instance.itemBar.AddOrUpdateItem(item, positionSlot);
        }
    }

    public void ClickItem()
    {
        if (!isWaitingForDoubleClick)
        {
            SoundManager.instance.Sound(0);
            isWaitingForDoubleClick = true;
            Invoke("ResetDoubleClickFlag", doubleClickDelay);
        }
        else // Double-Clique
        {
            ApplyItem();
        }

    }

    public void ApplyItem()
    {
        //Verifier qu'il est sur un terrain de combat
        PlayerMove player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        if (item != null && item.isActive && !player.GetStopMove())
        {
            if (item.itemType == ItemType.Consumable && GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>() != null)
            {
                bool applyEffect = false;

                foreach (ItemEffect itemEffect in item.itemEffect)
                {
                    if (ItemManagerScene.instance.ApplyItemEffect(itemEffect))
                    {
                        applyEffect = true;
                    }
                }

                if (applyEffect)
                {
                    SoundManager.instance.Sound(20);
                    RemoveItem(1);
                }
            }
            if (item.itemType == ItemType.Pokeball)
            {
                CanvasManager.instance.pokeballManager.SetPokeball(item.id, (int)item.itemEffect[0].valueEffect, IsInItemBar());
            }

        }
    }

    private void ResetDoubleClickFlag()
    {
        isWaitingForDoubleClick = false;
    }

    public void RemoveItem(int amount)
    {
        item.amount-=amount;
        bool isDestroy = false;
        if (item.amount < 1 || item.maxAmount==1)
        {
            DestroyItem();
            isDestroy = true;
        }

        if (IsInItemBar())
            CanvasManager.instance.itemBar.SaveItem(item, positionSlot, isDestroy);
        else
            CanvasManager.instance.inventory.SaveItem(item, positionSlot, isDestroy);

        transform.GetChild(1).GetComponent<Text>().text = item.amount.ToString();
        CanvasManager.instance.questFrame.ApplyText();
    }

    public void DestroyItem()
    {
        int id = item.id;
        item = new Item { id = id, isActive = false };
        //Met le canvas par defaut null
        transform.GetChild(0).GetComponent<Image>().sprite = startSprite;
        transform.GetChild(1).gameObject.SetActive(false);
        CanvasManager.instance.tooltip.gameObject.SetActive(false);

        if (IsInItemBar())
            CanvasManager.instance.itemBar.SaveItem(item, positionSlot, true);
        else
            CanvasManager.instance.inventory.SaveItem(item, positionSlot, true);

        CanvasManager.instance.questFrame.ApplyText();
    }

    public void MouseEnter()
    {
        if (item != null && item.isActive)
        {
            string texteItem = "Rarity : " + item.rarity + '\n' +
                    item.description + '\n' +
                    (item.GetEffectText() != "" ? item.GetEffectText() + '\n' : "") +
                    "Price : " + item.price + " Gold";

            GameObject itemPanel;
            if (IsInItemBar())
                itemPanel = transform.parent.parent.gameObject;
            else
                itemPanel = transform.parent.parent.parent.parent.parent.gameObject;

            CanvasManager.instance.tooltip.SetTooltip(transform.position, itemPanel, item.icon, item.name, item.itemType.ToString(), texteItem);
        }
    }

    public void MouseExit()
    {
        CanvasManager.instance.tooltip.gameObject.SetActive(false);
    }

    public bool IsInItemBar()
    {
        return GetComponentInParent<ItemBar>() != null;
    }
}
