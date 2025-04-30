using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EndFloor
{
  treasureRoom, bossRoom  
}

[CreateAssetMenu(fileName = "Dungeon", menuName = "Dungeon")]
public class DungeonSO : ScriptableObject
{
    [Header("Dungeon Generation")]
    public string dungeonName;
    public Material wallMaterial;
    public Material floorMaterial;
    public int dungeonSize;
    [Range(0, 30)] public int roomCount = 1;
    [Range(1,20)] public int roomXSizeMin, roomXSizeMax, roomYSizeMin, roomYSizeMax = 5;
    [Range(1, 40)] public int numOfFloors;
    
    [Header("NPC Spawns")]
    [Range (0, 20)] public int numOfNPCS;
    public GameObject[] npcs;
    
    [Header("Item Spawns")]
    [Range (0, 20)] public int numOfItems;
    public GameObject[] items;
    
    [Header("End Room")]
    public EndFloor endFloor = EndFloor.treasureRoom;
}
