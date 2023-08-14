using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemCraftDatabase", menuName = "Database/ItemCraftDatabase", order = 0)]
public class ItemCraftDatabase : ScriptableObject
{
    public ItemCraft[] itemCraft;
}
