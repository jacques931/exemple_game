using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnInCity : MonoBehaviour
{
    [SerializeField] private bool isEndGate;
    [SerializeField] private bool nextDungeon;

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<PlayerMove>().ChangeTextIcon("T",true);
            GameObject.FindGameObjectWithTag("TerrainManager").GetComponent<TerrainObjectManager>().SetCanReturnCity(true,isEndGate,nextDungeon);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<PlayerMove>().ChangeTextIcon("T",false);
            GameObject.FindGameObjectWithTag("TerrainManager").GetComponent<TerrainObjectManager>().SetCanReturnCity(false,isEndGate,nextDungeon);
        }
    }

    public void ClickQuitDungeon()
    {
        SoundManager.instance.Sound(0);
        string texte = "Are you sure you want to leave the dungeon ?";
        CanvasManager.instance.confirmPanel.gameObject.SetActive(true);
        CanvasManager.instance.confirmPanel.GetComponent<ConfirmPanelScript>().SetConfirmPanel(texte,GameObject.FindGameObjectWithTag("TerrainManager").GetComponent<TerrainObjectManager>().ReturnInCity);
    }
}
