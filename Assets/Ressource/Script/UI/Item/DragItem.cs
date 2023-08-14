using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject draggedObject;
    private SlotScript draggedSlot;

    private PlayerMove player;

    private void OnDisable()
    {
        if(draggedObject!=null)
        {
            Destroy(draggedObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggedSlot = GetComponent<SlotScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        if(draggedSlot.GetItem()!=null && draggedSlot.GetItem().isActive && !player.GetStopMove())
        {
            // Créer une copie de l'objet actuel pour le glisser-déposer
            SoundManager.instance.Sound(7);
            GameObject clickedImage = transform.gameObject;
            draggedObject = Instantiate(clickedImage, clickedImage.transform.position, clickedImage.transform.rotation, CanvasManager.instance.transform);
            draggedObject.GetComponent<Image>().color = new Color(0,0,0,0);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        bool playerisDead = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().GetStopMove();
        if (draggedObject != null)
        {
            if(player!=null && player.GetStopMove())
            {
                Destroy(draggedObject);
            }
            draggedObject.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(draggedObject!=null)
        {
            Destroy(draggedObject);
            // Utiliser un raycast pour détecter l'objet sous le point final du drag
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            // Vérifier si un objet a été détecté et s'il contient le script SlotScript
            foreach (var result in results)
            {
                SlotScript slotScript = result.gameObject.GetComponent<SlotScript>();
                if(slotScript==draggedSlot)
                    return;

                if (slotScript != null)
                {
                    Item itemEndDrag = slotScript.GetItem();
                    Item draggedItem = draggedSlot.GetComponent<SlotScript>().GetItem();

                    if (itemEndDrag != null && itemEndDrag.isActive) // Si il a deja un item
                    {
                        if (draggedItem.id == itemEndDrag.id && draggedItem.maxAmount > 1)
                        {
                            int newAmount = itemEndDrag.amount + draggedItem.amount;
                            itemEndDrag.amount = Mathf.Clamp(newAmount, 0, itemEndDrag.maxAmount);
                            draggedItem.amount = Mathf.Clamp(newAmount - itemEndDrag.amount, 0, draggedItem.maxAmount);
                            slotScript.UpdateSlot(itemEndDrag);

                            if (draggedItem.amount == 0)
                            {
                                draggedSlot.GetComponent<SlotScript>().DestroyItem();
                            }
                            else
                            {
                                draggedSlot.GetComponent<SlotScript>().UpdateSlot(draggedItem);
                            }
                        }
                        else
                        {
                            draggedSlot.GetComponent<SlotScript>().UpdateSlot(itemEndDrag);
                            slotScript.UpdateSlot(draggedItem);
                        }
                    }
                    else // Si le slot est vide
                    {
                        slotScript.UpdateSlot(draggedSlot.GetComponent<SlotScript>().GetItem());
                        draggedSlot.DestroyItem();
                    }
                    CanvasManager.instance.pokeballManager.CheckHavePokeball();
                    CanvasManager.instance.inventory.RecoveryItem();
                    CanvasManager.instance.monsterCatch.SetCurrentData();
                    CanvasManager.instance.questFrame.ApplyText();
                    return;
                }
            }

            bool isInInventory = results.Any(result => result.gameObject.name == "Inventory");
            bool isInShop = results.Any(result => result.gameObject.name == "Shop");

            if(isInShop)
            {
                CanvasManager.instance.buyShop.gameObject.SetActive(true);
                CanvasManager.instance.buyShop.SetBuyShop(draggedSlot.GetItem(),draggedObject.transform.position,false,!draggedSlot.IsInItemBar(),draggedSlot.GetPosition(),draggedSlot.GetItem().amount);
            }
            else if(!isInInventory)
            {
                string texte = "Are you sure you want to delete this item ?";
                CanvasManager.instance.confirmPanel.SetActive(true);
                CanvasManager.instance.confirmPanel.GetComponent<ConfirmPanelScript>().SetConfirmPanel(texte,DestroyItem);
            }
            
        }
    }

    private void DestroyItem()
    {
        draggedSlot.DestroyItem();
        SoundManager.instance.Sound(10);
    }
        
}
