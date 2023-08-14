using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string nameQuest;
    public int idDialogue; // C'est le dialogue du debut
    public int newIdDungeon;

    [Header("Quest Value")]
    public QuestType[] questType;
    [Header("Reward")]
    public int money;
    public Reward[] rewards;

}
