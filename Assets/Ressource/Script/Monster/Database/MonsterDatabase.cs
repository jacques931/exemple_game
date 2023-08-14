using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterDatabase", menuName = "Database/MonsterDatabase", order = 0)]
public class MonsterDatabase : ScriptableObject
{
    public Monster[] monster;
}
