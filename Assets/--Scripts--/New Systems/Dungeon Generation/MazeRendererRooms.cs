using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeRendererRooms : MonoBehaviour
{
    
    [Header("Inscribed")]
    [SerializeField] MazeGeneratorWithRooms mazeGenerator;
    [SerializeField] GameObject MazeCellPrefab;
    Dictionary<Vector3Int, MazeCellObject> cells = new Dictionary<Vector3Int, MazeCellObject>();
    [SerializeField] GameObject exit;
    
    // This is the physical size of our maze cells. Getting this wrong will result in overlapping
    // or visibile gaps between each cell
    public int CellSize = 1;
    public NavMeshSurface navSurface;

    
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
        
        //TileWalls();
        navSurface.BuildNavMesh();
        
        // Creating Exit
        CreateExit();
    }

    void CreateExit()
    {
        Room exitRoom = mazeGenerator.rooms[Random.Range(0, (mazeGenerator.rooms.Count - 1))];
        Vector2 tile = exitRoom.tilesInRoom[Random.Range(0, (exitRoom.tilesInRoom.Count - 1))];
        
        Instantiate(exit, new Vector3(tile.x, -0.2f, tile.y), Quaternion.identity, transform);
    }
}
