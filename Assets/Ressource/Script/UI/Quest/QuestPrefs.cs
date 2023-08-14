using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPrefs : MonoBehaviour
{
    [SerializeField] private Image questIcon;
    [SerializeField] private Text questTxt;
    [SerializeField] private Text areaTxt;

    public void UpdateQuest(Sprite sprite,string quest,QuestType questType,int numberQuest)
    {
        questIcon.sprite = sprite;
        questTxt.text = quest;
        areaTxt.text = questType.messageArea;

        questTxt.color = PlayerPrefs.GetInt("Quest" + numberQuest) < questType.amount ? Color.red : Color.green;
    }

    public void UpdateReward(Item item, string reward)
    {
        transform.GetChild(0).GetComponent<SlotScript>().UpdateSlot(item);
        questTxt.text = reward;
    }

    public void UpdateRewardMonster(Monster monster, string reward)
    {
        transform.GetChild(0).GetComponent<MonsterSlot>().InstanceIcon(monster);
        questTxt.text = reward;
    }
}
