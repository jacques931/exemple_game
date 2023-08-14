using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractQuest : MonoBehaviour
{
    [SerializeField] private string objectName;
    private bool havePlayer;

    private void Update()
    {
        if(havePlayer && Input.GetKeyDown(KeyCode.T))
        {
            SoundManager.instance.Sound(8);
            CanvasManager.instance.questManager.CheckQuestDialogue(objectName,TypeOfQuest.Dialogue);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<PlayerMove>().ChangeTextIcon("T",true);
            havePlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<PlayerMove>().ChangeTextIcon("T",false);
            havePlayer = false;
        }
    }
}
