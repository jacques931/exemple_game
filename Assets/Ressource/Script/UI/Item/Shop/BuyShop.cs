using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BuyShop : MonoBehaviour
{
    [SerializeField] InputField inputField;
    private Item item;
    private int amount;

    private string ancienText;
    private bool pointerInPanel;
    private bool isBuyShop;
    private bool haveClick;
    private int itemPosition;
    private bool isInInventory;

    private void Update()
    {
        if(ancienText!=inputField.text)
        {
            if(isBuyShop)
                RestrictionBuy();
            else
                RestrictionSell();
            ancienText = inputField.text;
        }
        if(Input.GetMouseButtonDown(0) && !pointerInPanel)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetBuyShop(Item _item,Vector3 slotPosition,bool _isBuyShop,bool _isInInventory=true,int _itemPosition=-1,int _amount=1)
    {
        haveClick = false;
        item = _item;
        isBuyShop = _isBuyShop;
        amount = _amount;
        itemPosition = _itemPosition;
        isInInventory = _isInInventory;
        inputField.text = amount.ToString();

        Button button = transform.GetChild(2).GetComponent<Button>();
        button.onClick.RemoveAllListeners();

        if(isBuyShop)
        {
            transform.GetChild(0).GetComponent<Text>().text = "How many do you want to buy ?";
            button.onClick.AddListener(() => BuyItem());
        } 
        else
        {
            transform.GetChild(0).GetComponent<Text>().text = "How many do you want to sell for " + (int.Parse(inputField.text)*item.price) + " gold";
            button.onClick.AddListener(() => SellItem());
        }

        float addAMount = 150;
        if(slotPosition.x + addAMount >1470)
        {
            addAMount = -addAMount;
        }
        transform.position = new Vector3(slotPosition.x + addAMount ,Mathf.Clamp(slotPosition.y, 170f, 730f),transform.position.z);
    }

    private void RestrictionBuy()
    {
        int maxItemBuy = Mathf.FloorToInt(PlayerPrefs.GetInt("money") / item.priceInShop);
        int inputAmount = int.Parse(inputField.text);

        if(item.maxAmount==1)
        {
            inputField.text = 1 + "";
        }
        else if(inputAmount>maxItemBuy)
        {
            inputField.text = maxItemBuy.ToString();
        }
    }

    private void RestrictionSell()
    {
        int inputAmount = int.Parse(inputField.text);

        if(item.maxAmount==1)
        {
            inputField.text = 1 + "";
        }
        else if(inputAmount>item.amount)
        {
            inputField.text = item.amount.ToString();
        }

        inputAmount = int.Parse(inputField.text);
        
        transform.GetChild(0).GetComponent<Text>().text = "How many do you want to sell for " + (inputAmount*item.price) + " gold";
    }

    public void BuyItem()
    {
        if(!haveClick)
        {
            int inputAmount = int.Parse(inputField.text);
            CanvasManager.instance.inventory.AddItemInInventory(item.id,inputAmount);
            CanvasManager.instance.questManager.CheckQuestID(item.id,inputAmount,TypeOfQuest.Buy_Item);
            int price = item.priceInShop * inputAmount;
            int currentMoney = PlayerPrefs.GetInt("money") - price;
            PlayerPrefs.SetInt("money",currentMoney);
            CanvasManager.instance.inventory.SetGoldCanvas();

            SoundManager.instance.Sound(6);

            haveClick=true;
            gameObject.SetActive(false);
        }
    }

    public void SellItem()
    {
        if(!haveClick)
        {
            int inputAmount = int.Parse(inputField.text);
            int priceSell = inputAmount * item.price;
            int currentMoney = PlayerPrefs.GetInt("money") + priceSell;
            PlayerPrefs.SetInt("money",currentMoney);
            CanvasManager.instance.inventory.SetGoldCanvas();
            if(isInInventory)
                CanvasManager.instance.inventory.RemoveItemInventory(itemPosition,inputAmount);
            else
                CanvasManager.instance.itemBar.RemoveItemBar(itemPosition,inputAmount);

            CanvasManager.instance.pokeballManager.CheckHavePokeball();
            SoundManager.instance.Sound(2);

            haveClick=true;
            gameObject.SetActive(false);
        }
    }

    public void PointerInPanel()
    {
        pointerInPanel = true;
    }

    public void PointerExitPanel()
    {
        pointerInPanel = false;
    }
}
