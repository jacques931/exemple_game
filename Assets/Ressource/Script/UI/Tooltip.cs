using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private Image iconImg;
    [SerializeField] private Text nameTitle;
    [SerializeField] private Text typeImg;
    [SerializeField] private Text informationsTxt;
    private GameObject openPanel;

    private void Update()
    {
        if(!openPanel.active)
        {
            gameObject.SetActive(false);
        }

    }

    public void SetTooltip(Vector3 toolPosition,GameObject _openPanel,Sprite icon,string name,string type,string description)
    {
        gameObject.SetActive(true);
        openPanel = _openPanel;
        float addAMount = 170;
        if(toolPosition.x + addAMount >1470)
        {
            addAMount = -addAMount;
        }
        transform.position = new Vector3(toolPosition.x + addAMount ,Mathf.Clamp(toolPosition.y, 170f, 730f),transform.position.z);
        
        iconImg.sprite = icon;
        nameTitle.text = name;
        typeImg.text = SeperateUnderscord(type);
        informationsTxt.text = description;
    }

    private string SeperateUnderscord(string texte)
    {
        texte = texte.Replace("_", " ");
        return texte;
    }
}
