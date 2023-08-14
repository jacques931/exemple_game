using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTerrain : MonoBehaviour
{
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Text itemName;
    private int idItem;
    
    public void SetInformation(int _idItem)
    {
        idItem = _idItem;
        Item item = itemDatabase.item[idItem];
        itemIcon.sprite = item.icon;
        itemName.text = item.name;
    }

    public void MouseEnter()
    {
        Item item = itemDatabase.item[idItem];
        if(item !=null && item.isActive)
        {
            string texteItem = "Rarity : " + item.rarity + '\n' + 
                    item.description + '\n' + 
                    (item.GetEffectText() != "" ? item.GetEffectText() + '\n' : "") +
                    "Drop chance : " + item.dropChance  + "%" + '\n' + 
                    "Price : " + item.price + " Gold";
                    

            GameObject itemPanel = transform.parent.parent.parent.parent.parent.gameObject;
            CanvasManager.instance.tooltip.SetTooltip(itemIcon.transform.position,itemPanel,item.icon,item.name,item.itemType.ToString(),texteItem);
        }
    
    }

    public void MouseExit()
    {
        CanvasManager.instance.tooltip.gameObject.SetActive(false);
    }

}
