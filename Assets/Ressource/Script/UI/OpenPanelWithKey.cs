using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanelWithKey : MonoBehaviour
{
    [SerializeField] private KeyPanel[] keyPanel;

    private void Update()
    {
        foreach(KeyPanel element in keyPanel)
        {
            if(Input.GetKeyDown(element.key))
            {
                element.panel.SetActive(!element.panel.active);
            }
        }
    }

    public void CloseAllPanel()
    {
        foreach(KeyPanel element in keyPanel)
        {
            element.panel.SetActive(false);
        }
    }
}
[System.Serializable]
public class KeyPanel
{
    public GameObject panel;
    public KeyCode key;
}
