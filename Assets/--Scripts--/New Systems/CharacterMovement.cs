using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public enum characterType
{
    player, enemy, ally
}

public class CharacterMovement : MonoBehaviour
{
    [Header("Inscribed")]
    public float movementDelay = 0.5f;
    private InputSystem_Actions playerControls;
    public characterType characterType = characterType.enemy;
    [SerializeField] private float costToChangePath = 5;

    
    
    [Header("Dynamic")]
    private bool validSpace = false;
    public List<Vector3> npcPath = new List<Vector3>();
    [SerializeField] bool isMoving = false; // Checking if we are already moving
    
    private float newPathCost;
    private float oldPathCost;
    
    private void Start()
    {
        if (characterType != characterType.player) return;
        playerControls = new InputSystem_Actions();
        playerControls.Player.Enable();
        
    }

    private void OnEnable()
    {
        isMoving = false; 
    }
    
    public virtual void FixedUpdate()
    {
        if(isMoving) return; 
        if (characterType == characterType.player)
        {
            Vector2 move = playerControls.Player.Move.ReadValue<Vector2>();
            Vector2Int directionInt = Vector2Int.RoundToInt(move);
            
            if (directionInt != Vector2.zero)
            {
                 // Converting to vector 3
                 Vector3 directionV3 = new Vector3(directionInt.x, 0, directionInt.y);
                 StartCoroutine(MoveTowardsTarget(directionV3));
                 isMoving = true;
            }
            
        }
        else
        {
            Pathfinder.S.CreatePath(this);
            NPCMovement();
            
        }
    }

    /// <summary>
    /// I could handle this all in the else statement but this is going to be long enough to justify a void
    /// </summary>
    private void NPCMovement()
    {
        npcPath.Clear();
        
        // This is A* adjacent chicanery, we check if it is worth it to change the current path we are on
        //to the player
        if (npcPath.Count > 0)
        {
            foreach (Vector3 node in Pathfinder.S.pathPoints)
            {
                // Let's calculate the gCost first (distance from the node to the start)
                float gCostX = Mathf.Abs(node.x - transform.position.x);
                float gCostY = Mathf.Abs(node.z - transform.position.z);
                float gCost = gCostX + gCostY;
                
                // Now lets do hCost. We need the final point on the list for this
                float hCostX = Mathf.Abs(node.x - Pathfinder.S.pathPoints[Pathfinder.S.pathPoints.Count - 1].x);
                float hCostY = Mathf.Abs(node.z - Pathfinder.S.pathPoints[Pathfinder.S.pathPoints.Count - 1].z);
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
                float hCostX = Mathf.Abs(node.x - Pathfinder.S.pathPoints[Pathfinder.S.pathPoints.Count - 1].x);
                float hCostY = Mathf.Abs(node.z - Pathfinder.S.pathPoints[Pathfinder.S.pathPoints.Count - 1].z);
                float hCost = hCostX + hCostY;
                
                // Lastly, fCost
                float fCost = gCost + hCost;
                oldPathCost += fCost;
            }

            if (newPathCost < (oldPathCost * costToChangePath))
            {
                npcPath.Clear();
                npcPath.AddRange(Pathfinder.S.pathPoints);
            }
        }
        else
        {
            npcPath.AddRange(Pathfinder.S.pathPoints);
        } 
        
        // Next lets check if path point 0 = our current position
        Vector3 transRouned = transform.position;
        transRouned.x = Mathf.RoundToInt(transform.position.x);
        transRouned.y = 0.5f;
        transRouned.z = Mathf.RoundToInt(transform.position.z);
        
        if (npcPath[0] == transRouned) npcPath.RemoveAt(0);
        
        Vector3 distance = npcPath[0] - transform.position;
        Vector3 directionV3 = new Vector3(distance.x, 0.5f, distance.z).normalized;
        StartCoroutine(MoveTowardsTarget(directionV3));
        isMoving = true;
    }
    
    public IEnumerator MoveTowardsTarget(Vector3 directionVector3)
    {
        // We need to check if we are on a diagonal or not
        float rayCastDistance = 1.4f;
        
        print(directionVector3.magnitude);
        if (directionVector3.magnitude > 1) rayCastDistance = 1.41f;
        
        Debug.DrawRay(transform.position, directionVector3, Color.red, 1);
        if (!Physics.Raycast(transform.position, directionVector3, out RaycastHit hit, rayCastDistance))
        {
            // We are getting the direction we want to move in + our current position
            float dirX = Mathf.RoundToInt(directionVector3.x);
            float dirz = Mathf.RoundToInt(directionVector3.z);
        
            float myX = Mathf.RoundToInt(transform.position.x);
            float myZ = Mathf.RoundToInt(transform.position.z);
        
            Vector3 newPos = new Vector3(myX + dirX, 0.5f, myZ + dirz);
            float elapsedTime = 0;
            while (elapsedTime < movementDelay)
            {
                transform.position=Vector3.Slerp(transform.position , newPos,(elapsedTime / movementDelay));
                elapsedTime += Time.deltaTime;
                    
                yield return null;
            }

            if (transform.position == newPos)
            {
                float posX = Mathf.RoundToInt(transform.position.x);
                float posZ = Mathf.RoundToInt(transform.position.z);

                transform.position = new Vector3(posX, 0.5f, posZ);
            }

            if (elapsedTime >= movementDelay)
            {
                TurnManagerNew.S.EndTurn();
            } 
        }
        else
        {
            
            TurnManagerNew.S.EndTurn();
        }
        
        isMoving = false;
    }
}
