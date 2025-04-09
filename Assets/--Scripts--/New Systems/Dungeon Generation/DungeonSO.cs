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
    [Range(0, 10)] public int roomCount = 1;
    [Range(1,20)] public int roomXSizeMin, roomXSizeMax, roomYSizeMin, roomYSizeMax = 5;
}
