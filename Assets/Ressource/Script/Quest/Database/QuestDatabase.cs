using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDatabase", menuName = "Database/QuestDatabase", order = 0)]
public class QuestDatabase : ScriptableObject
{
    public Quest[] quest;
}
