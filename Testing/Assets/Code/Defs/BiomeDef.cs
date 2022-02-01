using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Biome", menuName = "Game/Biome", order = 1)]
public class BiomeDef : ScriptableObject
{
    public Vector2 tileset_0_offset;
    public Vector2 tileset_1_offset;
    public Vector2 tileset_2_offset;
    public Vector2 treeset_offset;
}
