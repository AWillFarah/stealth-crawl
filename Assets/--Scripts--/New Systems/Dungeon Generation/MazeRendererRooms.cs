using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Handles the placement on the maze cell objects as well as the placement of items, player, and npc
/// </summary>
public class MazeRendererRooms : MonoBehaviour
{
    
    [Header("Inscribed")]
    [SerializeField] MazeGeneratorRooms mazeGenerator;
    [SerializeField] GameObject MazeCellPrefab;
    Dictionary<Vector3Int, MazeCellObject> cells = new Dictionary<Vector3Int, MazeCellObject>();
    
    
    // This is the physical size of our maze cells. Getting this wrong will result in overlapping
    // or visibile gaps between each cell
    public int CellSize = 1;
    public NavMeshSurface navSurface;
    
    [Header("Dynamic")]
    [SerializeField] List<Vector2Int> roomTiles = new List<Vector2Int>();
    [Header("Objects to Spawn")]
    public GameObject exit;
    public GameObject player;
    
    private void Start()
    {
        MazeCellNew[,] maze = mazeGenerator.GetMaze();
        

        for (int x = 0; x < mazeGenerator.mazeWidth; x++)
        {
            for (int y = 0; y < mazeGenerator.mazeHeight; y++)
            {
                GameObject newCell = Instantiate(MazeCellPrefab, new Vector3Int(x * CellSize, 0, y * CellSize), Quaternion.identity, transform);   
                foreach (Transform child in newCell.transform)
                {
                    if (child.CompareTag("Wall"))
                    {
                        
                        child.gameObject.GetComponent<Renderer>().material = mazeGenerator.dungeon.wallMaterial;
                    }

                    if (child.CompareTag("Ground"))
                    {
                        child.gameObject.GetComponent<Renderer>().material = mazeGenerator.dungeon.floorMaterial;
                    }
                }

                
                MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();
                cells.Add(new Vector3Int(x * CellSize, 0, y * CellSize), mazeCell);
                // determine which walls need to be active
                bool top = maze[x, y].topWall;
                bool left = maze[x, y].leftWall;
                
                // Bottom and right walls are deactivated by default unless we are at the bottom or right
                // edge of the maze
                bool right = false;
                bool bottom = false;
                if (x == mazeGenerator.mazeWidth - 1) right = true;
                if (y == 0) bottom = true;
                
                mazeCell.Init(top, bottom, left, right);
            }

         
        }
        navSurface.BuildNavMesh();
        
        // Creating Exit
        SpawnEntities();
    }

    void SpawnEntities()
    {
        // This is to avoid getting repeats
        foreach (Room r in mazeGenerator.rooms)
        {
            foreach (Vector2Int t in r.tilesInRoom)
            {
                roomTiles.Add(t);
            } 
        }
        
        //Room exitRoom = GetRoom();
        Vector2 tile = GetSpawnPosition();
        Instantiate(exit, new Vector3(tile.x, -0.5f, tile.y), Quaternion.identity, transform);
        
        //Room playerSpawmRoom = GetRoom();
        tile = GetSpawnPosition();
        Instantiate(player, new Vector3(tile.x, 0.5f, tile.y), Quaternion.identity, transform);
        
        for (int i = 0; i < mazeGenerator.dungeon.numOfItems; i++)
        {
            tile = GetSpawnPosition();
            GameObject item = mazeGenerator.dungeon.items[Random.Range(0, mazeGenerator.dungeon.items.Length)];
            Instantiate(item, new Vector3(tile.x, 0.5f, tile.y), Quaternion.identity, transform);
        }
        
        for (int i = 0; i < mazeGenerator.dungeon.numOfNPCS; i++)
        {
            tile = GetSpawnPosition();
            GameObject npc = mazeGenerator.dungeon.npcs[Random.Range(0, mazeGenerator.dungeon.npcs.Length)];
            Instantiate(npc, new Vector3(tile.x, 0.5f, tile.y), Quaternion.identity, transform);
        }
        
        /*
        GameObject cam = Camera.main.gameObject;
        CameraMovement camMovement = cam.GetComponent<CameraMovement>();
        camMovement.FindPlayer(newPlayer); */
    }

    public Vector2Int GetSpawnPosition()
    {
        // In case the list is empty!
        if (roomTiles.Count == 0)
        {
            foreach (Room r in mazeGenerator.rooms)
            {
                foreach (Vector2Int t in r.tilesInRoom)
                {
                    roomTiles.Add(t);
                } 
            }  
        }
        
        Vector2Int tile = roomTiles[Random.Range(0, roomTiles.Count - 1)];
        // Remove this tile from the list 
        roomTiles.Remove(tile);
        return tile;
    }
}
