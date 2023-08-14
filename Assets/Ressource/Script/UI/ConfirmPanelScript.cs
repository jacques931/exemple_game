using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ConfirmPanelScript : MonoBehaviour
{
    public void SetConfirmPanel(string texte, Action fonctionYes)
    {
        transform.GetChild(0).GetComponent<Text>().text = texte;
        Button button = transform.GetChild(1).GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => fonctionYes.Invoke());
    }

    public void SetDeadPanel(string texte, Action fonctionYes)
    {
        transform.GetChild(0).GetComponent<Text>().text = texte;
        transform.GetChild(1).GetChild(1).gameObject.SetActive(false); // Deactive le bouton "No"
        Button button = transform.GetChild(1).GetChild(0).GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => fonctionYes.Invoke());
        button.onClick.AddListener(() => ClosePanel());
    }

    public void SetRessurectionPanel(string texte, Action fonctionYes, Action fonctionNo)
    {
        transform.GetChild(1).GetChild(1).gameObject.SetActive(true); // Active le bouton "No"
        transform.GetChild(0).GetComponent<Text>().text = texte;

        Button buttonYes = transform.GetChild(1).GetChild(0).GetComponent<Button>();
        buttonYes.onClick.RemoveAllListeners();
        buttonYes.onClick.AddListener(() => fonctionYes.Invoke());
        buttonYes.onClick.AddListener(() => ClosePanel());

        Button buttonNo = transform.GetChild(1).GetChild(1).GetComponent<Button>();
        buttonNo.onClick.RemoveAllListeners();
        buttonNo.onClick.AddListener(() => fonctionNo.Invoke());
        buttonNo.onClick.AddListener(() => ClosePanel());
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }

}
