using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{    
    public Effect effect;
    public float valueEffect;
    public float timeEffect;
}

public enum Effect
{
    None,
    Heal,
    Speed,
    Attack,
    Shield,
    Recovery_Life,
    Get_Money,
    Skill_Speed,
    Capture_Speed,
    Pokeball_Id,
    Level_Up
}