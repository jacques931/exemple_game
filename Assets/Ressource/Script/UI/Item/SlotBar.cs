using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotBar : MonoBehaviour
{
    [SerializeField] private KeyCode keycode;

    private void Update()
    {
        if(Input.GetKeyDown(keycode))
        {
            transform.GetChild(0).GetComponent<SlotScript>().ApplyItem();
        }
    }
    
}
