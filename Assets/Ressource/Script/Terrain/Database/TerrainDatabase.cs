using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainDatabase", menuName = "Database/TerrainDatabase", order = 0)]
public class TerrainDatabase : ScriptableObject
{
    public Terrain[] terrain;
}
