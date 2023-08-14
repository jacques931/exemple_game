using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private bool canOpenPanel;
    private string namePanel;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T) && canOpenPanel)
        {
            SoundManager.instance.Sound(8);
            GameObject panel = CanvasManager.instance.GetPanelByName(namePanel);
            if((namePanel=="Shop" || namePanel=="Craft") && panel.activeSelf==CanvasManager.instance.inventory.gameObject.activeSelf)
            {
                CanvasManager.instance.inventory.gameObject.SetActive(!panel.active);
            }
            panel.SetActive(!panel.active);
            if(namePanel=="Heal" && panel.activeSelf)
            {
                panel.GetComponent<MonsterPanel>().HealAllMonster();
                string message = "All your monsters were cured";
                CanvasManager.instance.SystemMessage(message);
            }
        }
    }

    public void ChangeStatutBool(string name,bool value)
    {
        namePanel = name;
        canOpenPanel = value;
        if(!canOpenPanel)
            CanvasManager.instance.GetPanelByName(namePanel).SetActive(false);
    }
}
