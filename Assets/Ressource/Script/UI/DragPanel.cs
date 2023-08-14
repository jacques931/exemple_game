using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{

    private Vector2 pointerOffset;                        
    private RectTransform canvasRectTransform;          
    private RectTransform panelRectTransform;           

    void Awake()
    {
        Canvas canvas = GetComponentInParent<Canvas>();              
        if (canvas != null)
        {
            canvasRectTransform = canvas.transform as RectTransform;     
            panelRectTransform = transform.parent as RectTransform;        
        }
    }

    public void OnPointerDown(PointerEventData data)                    
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            panelRectTransform, data.position, data.pressEventCamera, out pointerOffset);       
    }

    public void OnDrag(PointerEventData data)                          
    {
        if (panelRectTransform == null)                               
            return;                                           

        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, data.pressEventCamera, out localPointerPosition))
        {
            panelRectTransform.localPosition = localPointerPosition - pointerOffset;
        }
    }
}