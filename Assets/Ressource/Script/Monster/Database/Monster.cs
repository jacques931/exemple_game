using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monster
{
    [HideInInspector] public int id;
    public string name;
    public Rarity rarity;
    public int idMonster;
    public int level;
    [HideInInspector] public int currentLife;
    [HideInInspector] public int startLife;
    public int maxLife;
    public float defense;
    public float speed;
    public float jump;
    public bool canFly;

    [Header("Monster")]
    public float getXpMonster;
    public int money;
    public float catchChance;
    public int spawnChance;
    public float searchPlayerDistance;
    [Header("Player")]
    public GameObject playerPrefs;
    public Sprite playerIcon;
    [HideInInspector] public float xp;
    public float maxXp;
    public float recoveryLife;
    public int SecondSkillLevel;
    public AttackInput[] input;

    public void SetState(Monster newPlayer)
    {
        playerPrefs = newPlayer.playerPrefs;
        playerIcon = newPlayer.playerIcon;
        for (int i = 0; i < input.Length; i++)
        {
            input[i].iconAttack = newPlayer.input[i].iconAttack;
            input[i].attackPrefs = newPlayer.input[i].attackPrefs;
        }
    }
    
}

