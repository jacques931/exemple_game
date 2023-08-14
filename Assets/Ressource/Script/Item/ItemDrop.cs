using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int idItem;
    private int itemAmount=1;
    private int destroyTime=30;

    public int GetIdItem()
    {
        return idItem;
    }

    private void Start()
    {
        Destroy(gameObject,destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            if(CanvasManager.instance.inventory.CanHaveMultiItem(idItem))
            {
                itemAmount = Random.Range(1,5);
            }
            if(!CanvasManager.instance.inventory.InventoryIsFull(idItem,itemAmount))
            {
                CanvasManager.instance.inventory.AddItemInInventory(idItem,itemAmount);
                SoundManager.instance.Sound(1);
                Destroy(gameObject);
            }
            else
            {
                string message = "Your inventory is full !";
                CanvasManager.instance.SystemMessage(message);
            }
            
        }
    }
}
