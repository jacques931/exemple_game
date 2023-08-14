using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public int id;
    [HideInInspector] public bool isActive=true;
    public Rarity rarity;
    public ItemType itemType;
    public Sprite icon;
    public string description;
    public int price;
    public int maxAmount;
    public int amount;
    public int dropChance;
    public ItemEffect[] itemEffect;
    [Header("Shop")]
    public bool isInShop;
    public int priceInShop;

    // A modifier selon les images
    public void SetImage(Sprite sprite)
    {
        icon = sprite;
    }

    public string GetEffectText()
    {
        string textEffect = "";
        int effectCount = itemEffect.Length;

        foreach(ItemEffect effect in itemEffect)
        {
            switch (effect.effect)
            {
                case Effect.Heal:
                    textEffect += "¤ Recover " + effect.valueEffect + " Life";
                    break;
                case Effect.Pokeball_Id:
                    textEffect += "¤ Capture a monster";
                    break;
                case Effect.Level_Up:
                    textEffect += "¤ Increase " + effect.valueEffect + " level during battle";
                    break;
                //Buff
                case Effect.Speed:
                    textEffect += "¤ Increase speed of " + effect.valueEffect;
                    break;
                case Effect.Shield:
                    textEffect += "¤ Increase defense of " + effect.valueEffect;
                    break;
                case Effect.Attack:
                    textEffect += "¤ Increase attack of " + effect.valueEffect + "%";
                    break;
                case Effect.Recovery_Life:
                    textEffect += "¤ Increases life regeneration " + effect.valueEffect + "%";;
                    break;
                case Effect.Get_Money:
                    textEffect += "¤ Increases the money you earn by " + effect.valueEffect + "%";;
                    break;
                case Effect.Skill_Speed:
                    textEffect += "¤ Reduce skill cooldown by " + effect.valueEffect + " %";;
                    break;
                case Effect.Capture_Speed:
                    textEffect += "¤ Reduce capture cooldown by " + effect.valueEffect + " %";;
                    break;
                default:
                    break;
            }

            if (--effectCount > 0)
            {
                textEffect += '\n';
            }
        }

        return textEffect;
    }

}

public enum ItemType
{
    Consumable,
    Quest_Item,
    Material=4,
    Pokeball,
    Resurrection,
    Unlock_Skill
}