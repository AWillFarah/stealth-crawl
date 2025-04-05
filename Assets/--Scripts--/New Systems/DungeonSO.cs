using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dungeon", menuName = "Dungeon")]
public class DungeonSO : ScriptableObject
{
    public string dungeonName;
    public Material wallMaterial;
    public Material floorMaterial;
    public int dungeonSize;
}
