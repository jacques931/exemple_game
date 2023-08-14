using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRoom : MonoBehaviour
{
    [SerializeField] private int nextRoomId;

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<PlayerMove>().ChangeTextIcon("T",true);
            transform.GetComponentInParent<TerrainGroup>().SetCanChangeRoom(true,nextRoomId);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<PlayerMove>().ChangeTextIcon("T",false);
            transform.GetComponentInParent<TerrainGroup>().SetCanChangeRoom(false,nextRoomId);
        }
    }
}
