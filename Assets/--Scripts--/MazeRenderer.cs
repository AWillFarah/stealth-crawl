using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] GameObject MazeCellPrefab;
    Dictionary<Vector3Int, MazeCellObject> cells = new Dictionary<Vector3Int, MazeCellObject>();
    
    
    // This is the physical size of our maze cells. Getting this wrong will result in overlapping
    // or visibile gaps between each cell
    public int CellSize = 1;
    public NavMeshSurface navSurface;

    public Pathfinder pathfinder;
    
    private void Start()
    {
        MazeCell[,] maze = mazeGenerator.GetMaze();
        

        for (int x = 0; x < mazeGenerator.mazeWidth; x++)
        {
            for (int y = 0; y < mazeGenerator.mazeHeight; y++)
            {
                GameObject newCell = Instantiate(MazeCellPrefab, new Vector3Int(x * CellSize, 0, y * CellSize), Quaternion.identity, transform);   
                
                
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
    }

    void TileWalls()
    {
        foreach (KeyValuePair<Vector3Int, MazeCellObject> cell in cells)
        {

            bool flipWall;
            
            Vector3Int keyAbove = new Vector3Int(cell.Key.x * CellSize, 0, (cell.Key.z + 1) * CellSize);
            Vector3Int keyBelow = new Vector3Int(cell.Key.x * CellSize, 0, (cell.Key.z - 1) * CellSize);
            Vector3Int keyLeft = new Vector3Int((cell.Key.x - 1) * CellSize, 0, cell.Key.z * CellSize);
            Vector3Int keyRight = new Vector3Int((cell.Key.x + 1) * CellSize, 0, cell.Key.z * CellSize);
            //---------------First lets handle the left side---------------
            if (cell.Value.leftActive)
            {
                flipWall = false;
                
                
                // We will be assigning points to adjacent walls
                int leftWallPoint = 0;
                // If either of these are out of bounds, we know we need to assign a point
                if((cell.Key.z >= mazeGenerator.mazeHeight - 1))
                {
                    leftWallPoint ++;
                }
                else if (cells[keyAbove].leftActive || cell.Value.topActive)
                {
                    leftWallPoint++;
                    // This tells us we need to flip the one adjacent wall
                    flipWall = true;
                }
                if (keyLeft.x >= 0)
                {
                    if (cells[keyLeft].topActive) flipWall = true;
                }
                
                if (keyBelow.z < 0)
                {
                    leftWallPoint ++;   
                }
                else if (cells[keyBelow].leftActive || cells[keyBelow].topActive)
                {
                    leftWallPoint ++;
                    flipWall = false;
                }

                
                cell.Value.ChangeMesh(true, flipWall, leftWallPoint);
            }
            //---------------Now lets handle the top---------------
            if (cell.Value.topActive)
            {
                flipWall = false;
                int topWallPoint = 0;
                
                // With height we needed to ceh
                if((cell.Key.x >= mazeGenerator.mazeWidth - 1))
                {
                    topWallPoint ++;
                }
                else if (cells[keyRight].topActive || cell.Value.leftActive)
                {
                    topWallPoint ++;
                    // This tells us we need to flip the one adjacent wall
                    flipWall = true;
                }
                if (keyBelow.z > 0)
                {
                    if (cells[keyBelow].topActive) flipWall = true;
                }
                
                if (keyLeft.x < 0)
                {
                    topWallPoint ++;   
                }
                else if (cells[keyLeft].topActive || cells[keyLeft].leftActive)
                {
                    topWallPoint ++;
                    
                }
                
                cell.Value.ChangeMesh(false, flipWall, topWallPoint);
            }
        }
    }
}
