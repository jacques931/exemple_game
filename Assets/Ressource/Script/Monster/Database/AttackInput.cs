using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackInput
{
    public KeyCode key;
    public string attackName;
    public TypeOfSkill typeAttack;
    public float damage;
    public string attackAnimName;
    public float reload;
    public float addPosAttack;
    public float addPosAttackY;
    [HideInInspector] public bool isAttack;
    [HideInInspector] public bool canUse;
    public Sprite iconAttack;
    public GameObject attackPrefs;
    public AudioClip attackSound;
    [Header("Monster")]
    public float attackDistance;
}

public enum TypeOfSkill
{
    Melee_Attack,
    Ranged_Attack,
    Heal,
    Shield,
    Area_Attack
}
