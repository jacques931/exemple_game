using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PokeballDatabase", menuName = "Database/PokeballDatabase", order = 0)]
public class PokeballDatabase : ScriptableObject
{
    public Pokeball[] pokeball;
}