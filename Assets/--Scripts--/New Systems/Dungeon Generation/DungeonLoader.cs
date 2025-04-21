using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonLoader : MonoBehaviour
{
    public static DungeonSO dungeonToLoad;
    
    public void LoadDungeon(DungeonSO dungeon)
    {
        dungeonToLoad = dungeon;

        Exit.floorsRemaining = dungeonToLoad.numOfFloors;
        SceneManager.LoadScene("Maze");
    }
}
