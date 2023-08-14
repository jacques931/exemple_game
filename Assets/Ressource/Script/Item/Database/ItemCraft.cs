using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemCraft
{
    public int idCraftItem;
    public int craftAmount;
    public ItemCraftValue[] itemCraft;
}

[System.Serializable]
public class ItemCraftValue
{
    public int idItemToCraft;
    public int amountCraft;
}
