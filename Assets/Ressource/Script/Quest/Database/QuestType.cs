using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestType
{
    public TypeOfQuest typeOfQuest;
    public int idObject;
    public int amount;
    public string name;
    public string messageArea;
}

public enum TypeOfQuest
{
    Dialogue=0,
    Kill,
    Capture,
    Enter_Dungeon,
    Finish_Dungeon,
    Item,
    Buy_Item,
    Craft
}