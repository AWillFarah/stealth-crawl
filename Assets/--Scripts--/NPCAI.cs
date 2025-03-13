using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class NPCAI : EntityMovement
{
    [Header("Inscribed")]
    private Vector2[] directions = new Vector2[] { Vector2.left, Vector2.right, Vector2.up, Vector2.down };
    public Pathfinder pathfinder;
    [SerializeField] private float costToChangePath;
    
    
    [Header("Dynamic")]
    public List<Vector3> npcPath = new List<Vector3>();

    private float newPathCost;
    private float oldPathCost;
    
    
    
    //[Header("Dynamic")]
  
    void Awake()
    {
        
    }

    public void StartTurn()
    {
        Pathfinder.S.CreatePath(this);
        
        
    }


    public void ChoosePath(List<Vector3> path)
    {
        // We need to calculate if it is worth changing paths
        if (npcPath != null)
        {
            foreach (Vector3 node in path)
            {
                // Let's calculate the gCost first (distance from the node to the start)
                float gCostX = Mathf.Abs(node.x - transform.position.x);
                float gCostY = Mathf.Abs(node.z - transform.position.z);
                float gCost = gCostX + gCostY;
                
                // Now lets do hCost. We need the final point on the list for this
                float hCostX = Mathf.Abs(node.x - path[path.Count - 1].x);
                float hCostY = Mathf.Abs(node.z - path[path.Count - 1].z);
                float hCost = hCostX + hCostY;
                
                // Lastly, fCost
                float fCost = gCost + hCost;
                newPathCost += fCost;
            }
            foreach (Vector3 node in npcPath)
            {
                // Let's calculate the gCost first (distance from the node to the start)
                float gCostX = Mathf.Abs(node.x - transform.position.x);
                float gCostY = Mathf.Abs(node.z - transform.position.z);
                float gCost = gCostX + gCostY;
                
                // Now lets do hCost. We need the final point on the list for this
                float hCostX = Mathf.Abs(node.x - path[path.Count - 1].x);
                float hCostY = Mathf.Abs(node.z - path[path.Count - 1].z);
                float hCost = hCostX + hCostY;
                
                // Lastly, fCost
                float fCost = gCost + hCost;
                oldPathCost += fCost;
            }

            if (newPathCost < (oldPathCost * costToChangePath))
            {
                npcPath.Clear();
                npcPath.AddRange(path);
            }
        }
        else
        {
            npcPath.AddRange(path);
        } 
        
        npcPath.AddRange(path);
        
        // First lets check if path point 0 = our current position
        Vector3 transRouned = transform.position;
        transRouned.x = Mathf.RoundToInt(transform.position.x);
        transRouned.y = 0.5f;
        transRouned.z = Mathf.RoundToInt(transform.position.z);
        
        if (npcPath[0] == transRouned)
        {
            
            npcPath.RemoveAt(0);
        }
        Vector3 directionV3 = npcPath[0] - transform.position;
        // We will need to turn this into a vector2
        float dX = directionV3.x;
        float dY = directionV3.z;
        Vector2 directionV2 = new Vector2((int)dX, (int)dY).normalized;
        Vector2Int directionInt = Vector2Int.RoundToInt(directionV2);
        Move(directionInt);
        print(directionInt);
        
    }
    
    public void Move(Vector2 direction)
    {
        //We need a delay to the movement so we will need to use an IEnum
        IEnumerator moveSpace = MoveSpace(direction);
        StartCoroutine(moveSpace); 
        TurnManager.S.EndTurn(gameObject);
    }
    
    
}


