using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueDatabase", menuName = "Database/DialogueDatabase", order = 0)]
public class DialogueDatabase : ScriptableObject
{
    public Dialogue[] dialogue;
}

