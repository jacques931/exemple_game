using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    [SerializeField] private string openPanelName;

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<PlayerMove>().ChangeTextIcon("T",true);
            col.GetComponent<PlayerInteract>().ChangeStatutBool(openPanelName,true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<PlayerMove>().ChangeTextIcon("T",false);
            col.GetComponent<PlayerInteract>().ChangeStatutBool(openPanelName,false);
        }
    }
}
