using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

public enum characterType
{
    player, npc
}

public class CharacterMovement : MonoBehaviour
{
    [Header("Inscribed")]
    public float defaultMovementDelay = 0.5f;
    private InputSystem_Actions playerControls;
    public characterType characterType = characterType.npc;
    public int teamNumber = 0;
    
    [SerializeField] private float costToChangePath = 5;
    private CharacterBattleManager characterBattleManager;
    
    [SerializeField] Renderer rend;
    
    [Header("Dynamic")]
    public float movementDelay = 0.5f;
    private bool validSpace = false;
    public List<Vector3> npcPath = new List<Vector3>();
    public bool isMoving = false; // Checking if we are already moving
    
    private float newPathCost;
    private float oldPathCost;

    private bool hasPath;
    private Vector3 pathDestination;
    
    private Transform target;
    
    private void Start()
    {
        characterBattleManager = gameObject.GetComponent<CharacterBattleManager>();
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
            bool attack = playerControls.Player.Attack.ReadValue<float>() > 0;
            bool turn = playerControls.Player.Turn.ReadValue<float>() > 0;
            Vector2Int directionInt = Vector2Int.RoundToInt(move);
            
            if (directionInt != Vector2.zero && !turn)
            {
                 // Converting to vector 3
                 Vector3 directionV3 = new Vector3(directionInt.x, 0, directionInt.y);
                 isMoving = true;
                 PlayerMovement(directionV3);
            }
            else if (directionInt != Vector2.zero && turn)
            {
                Vector3 directionV3 = new Vector3(transform.position.x + directionInt.x, 0.5f, transform.position.z + directionInt.y);
                transform.LookAt(directionV3);
                
            }

            if (attack && !isMoving)
            {
                characterBattleManager.Attack();
                isMoving = true;
            }
            
        }
        else
        {
            if(target == null)
            {
                if(!hasPath) Pathfinder.S.CreatePath(this, PickAPoint());
                else Pathfinder.S.CreatePath(this, pathDestination);
            }
            // Let's check if our target is within reach
            else if (target != null &&
                     Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
                         out RaycastHit hit, 1.41f))
            {
                print("attacking!");
                transform.LookAt(hit.point);
                characterBattleManager.Attack();
                isMoving = true;
            }
            else
            {
                Pathfinder.S.CreatePath(this, target.position);
            }
            if(!isMoving) NPCMovement();
            
        }
    }

    private Vector3 PickAPoint()
    {
        float xPoint = UnityEngine.Random.Range(0, MazeGenerator.S.mazeWidth);
        float yPoint = UnityEngine.Random.Range(0, MazeGenerator.S.mazeHeight);
        pathDestination = new Vector3(xPoint, 0.5f, yPoint);
        hasPath = true;
        return new Vector3(xPoint, 0.5f, yPoint);
        
    }
    
    /// <summary>
    /// I could handle this all in the else statement but this is going to be long enough to justify a void
    /// </summary>
    private void NPCMovement()
    {
        
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
        
        // Checking that our current position is spot 0
        Vector3 transRouned = transform.position;
        transRouned.x = Mathf.RoundToInt(transform.position.x);
        transRouned.y = 0.5f;
        transRouned.z = Mathf.RoundToInt(transform.position.z);
        
        if (npcPath[0] == transRouned) npcPath.RemoveAt(0);
        
        // We need this to be an if statement because we need to check right after we remove the point if
        // we have any points in our list
        if(npcPath.Count == 0)
        {
            hasPath = false;
            
            TurnManager.S.EndTurn();
            return;
        } 
        
        Vector3 distance = npcPath[0] - transform.position;
        Vector3 directionV3 = new Vector3(distance.x, 0.5f, distance.z).normalized;
        
        float dirX = Mathf.RoundToInt(directionV3.x);
        float dirz = Mathf.RoundToInt(directionV3.z);
        
        float myX = Mathf.RoundToInt(transform.position.x);
        float myZ = Mathf.RoundToInt(transform.position.z);
        
        Vector3 newPos = new Vector3(myX + dirX, 0.5f, myZ + dirz);
        transform.LookAt(newPos);
        // Let's check if this position is already occupied before we move
        if (Pathfinder.S.isPositionOccupied(newPos))
        {
            TurnManager.S.EndTurn();
            return;
        }
        StartCoroutine(MoveTowardsTarget(directionV3));
        
        isMoving = true;
    }

    public void FoundSomething(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            CharacterMovement c = obj.GetComponent<CharacterMovement>();
            
            if(c.teamNumber != teamNumber) target = obj.transform;
            
        }
    }
    
    private void PlayerMovement(Vector3 directionV3)
    {
        float dirX = Mathf.RoundToInt(directionV3.x);
        float dirz = Mathf.RoundToInt(directionV3.z);
        
        float myX = Mathf.RoundToInt(transform.position.x);
        float myZ = Mathf.RoundToInt(transform.position.z);
        
        Vector3 newPos = new Vector3(myX + dirX, 0.5f, myZ + dirz);
        transform.LookAt(newPos);
        // If the position is taken up then just skip your turn
        if (Pathfinder.S.isPositionOccupied(newPos))
        {
            TurnManager.S.EndTurn();
            return;
        }
        else if (!Physics.Linecast(transform.position, newPos, out RaycastHit hit))
        {
            StartCoroutine(MoveTowardsTarget(directionV3));
            
        }
        else
        {
            isMoving = false;
            TurnManager.S.EndTurn(); 
        }
    }
    
    
    
    public IEnumerator MoveTowardsTarget(Vector3 directionVector3)
    {
       
        // We are getting the direction we want to move in + our current position
        float dirX = Mathf.RoundToInt(directionVector3.x);
        float dirz = Mathf.RoundToInt(directionVector3.z);
        
        float myX = Mathf.RoundToInt(transform.position.x);
        float myZ = Mathf.RoundToInt(transform.position.z);
        
        Vector3 newPos = new Vector3(myX + dirX, 0.5f, myZ + dirz);
        
        if (!rend.isVisible) movementDelay = 0.001f;
        else movementDelay = defaultMovementDelay;
        
        float elapsedTime = 0;
        while (elapsedTime < movementDelay)
        {
            transform.position=Vector3.Lerp(transform.position , newPos,(elapsedTime / movementDelay));
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
            TurnManager.S.EndTurn();
        } 
        
        isMoving = false;
    }
}
