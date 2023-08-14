using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPanel : MonoBehaviour
{
    [SerializeField] private Transform optionInfoContent;
    
    private void ResetOptionContent()
    {
        foreach(Transform child in optionInfoContent)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void ClickToChangePanel(GameObject panel)
    {
        SoundManager.instance.Sound(0);
        ResetOptionContent();
        panel.SetActive(true);
    }
}
