using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    private bool canTeleport;
    [SerializeField] private GameObject nextTeleport;

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<PlayerMove>().SetcanTeleport(true,nextTeleport.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<PlayerMove>().SetcanTeleport(false);
        }
    }
}
