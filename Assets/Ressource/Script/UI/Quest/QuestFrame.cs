using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestFrame : MonoBehaviour
{
    [SerializeField] private Transform textArea;

    private void Start()
    {
        ApplyText();
    }

    public void ApplyText()
    {
        string[] allQuestTxt = CanvasManager.instance.questManager.GetQuestInfoTxt().ToArray();
        for(int i=0;i<textArea.childCount;i++)
        {
            if(i<allQuestTxt.Length)
            {
                textArea.GetChild(i).gameObject.SetActive(true);
                textArea.GetChild(i).GetComponent<Text>().text = allQuestTxt[i];
            }
            else
            {
                textArea.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

}
