using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This script is used for when NPCs are requesting a path to a target transform
/// It finds a path betweem them and the target, rounds out the numbers to whole, and then passes the list
/// back to them
/// </summary>
public class Pathfinder : MonoBehaviour
{
    
    public static Pathfinder S;
    
    [Header("Dynamic")]
    [SerializeField] private Transform targetTransform;
    private CharacterMovement activeInstance;
    private NavMeshTriangulation Triangulation;
    
    public List<Vector3> pathPoints = new List<Vector3>();
    public List<Vector3> occupiedPositions = new List<Vector3>();
    
  

    
    void Start()
    {
        S = this;
        Triangulation = NavMesh.CalculateTriangulation();
        
    }

    public void CreatePath(CharacterMovement npc, Vector3 destination)
    {
        
        Triangulation = NavMesh.CalculateTriangulation();
        /* switch (currentState)
        {
            case npcState.patrol:
                GameObject[] tempArray = GameObject.FindGameObjectsWithTag("Ground");
                int randomIndex = Random.Range(0, tempArray.Length);
                targetTransform = tempArray[randomIndex].transform;
                break;
            case npcState.chasing:
                targetTransform = GameObject.Find("Player").transform;
                break;
        } */
        
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(npc.transform.position, destination,  NavMesh.AllAreas, path))
        {
            pathPoints.Clear();
            for (int i = 0; i < path.corners.Length; i++)
            {
                // We need whole numbers here so we need to round our coordinates
                // We also always want Y to by 0.5 because thats where all our entities are at
                float pathX = Mathf.Round(path.corners[i].x);
                float pathY = 0.5f;
                float pathZ = Mathf.Round(path.corners[i].z);
                Vector3 pathRounded = new Vector3(pathX, pathY, pathZ);
                pathPoints.Add(pathRounded);
            }
            npc.hasPath = true;   
        }
        
    }

    public bool IsPositionOccupied(Vector3 pos)
    {
        occupiedPositions.Clear();
        GameObject[] tempArray = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject temp in tempArray)
        {
            occupiedPositions.Add(temp.transform.position);
        }

        foreach (Vector3 loc in occupiedPositions)
        {
            if (pos == loc)
            {
                return true;
                
            }
        }
        
        
         if (pos.x >= MazeGeneratorRooms.S.mazeWidth || pos.x < 0 || pos.z >= MazeGeneratorRooms.S.mazeHeight || pos.z < 0)
        {
            return true;
        } 
        
        return false;
    }
    
}
