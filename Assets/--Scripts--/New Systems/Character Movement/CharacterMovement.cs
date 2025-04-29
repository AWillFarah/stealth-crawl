using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

public enum CharacterType
{
    Player, NPC, Stationary
}

public enum NPCState
{
    Attacking, Chasing, ChangeWanderPoint, Investigating, Wandering
}

/// <summary>
/// This will handle anything an entity can do on its turn
/// </summary>

public class CharacterMovement : MonoBehaviour
{
    [Header("Inscribed")]
    public float defaultMovementDelay = 0.5f;
    private InputSystem_Actions playerControls;
    public CharacterType characterType = CharacterType.NPC;
    
    [SerializeField] private float costToChangePath = 5;
    private CharacterBattleManager characterBattleManager;
    
    [SerializeField] Renderer rend;
    
    public StateManager stateManager;
    
    [SerializeField] LayerMask playerLayerMask;
    
    [SerializeField] SoundFXSO footStepSound;
    [SerializeField] private GameObject hearing;
    
   
    
    [Header("Dynamic")]
    public float movementDelay = 0.5f;
    public List<Vector3> npcPath = new List<Vector3>();
    public bool isMoving = false; // Checking if we are already moving
    
    private float newPathCost;
    private float oldPathCost;

    [HideInInspector] public bool hasPath;
    
    public Transform target;
    
    private void Start()
    {
        characterBattleManager = gameObject.GetComponent<CharacterBattleManager>();
        if (characterType == CharacterType.Player)
        {
            characterBattleManager.isPlayer = true;
            
        }
    }

    private void OnEnable()
    {
        isMoving = false; 
    }
    
    public virtual void FixedUpdate()
    {
        if(isMoving) return;
        switch (characterType)
        {
            case CharacterType.Player:
                Vector2 move = InputManager.INPUTACTIONS.Player.Move.ReadValue<Vector2>();
                bool attack = InputManager.INPUTACTIONS.Player.Attack.ReadValue<float>() > 0;
                bool turn = InputManager.INPUTACTIONS.Player.Turn.ReadValue<float>() > 0;
                bool pause = InputManager.INPUTACTIONS.Player.Pause.ReadValue<float>() > 0;
            
                Vector2Int directionInt = Vector2Int.RoundToInt(move);
                Vector3 directionV3 = new Vector3(directionInt.x, 0.5f, directionInt.y);
                if (directionInt != Vector2.zero && !turn)
                {
                    // Converting to vector 3
                
                    isMoving = true;
                    PlayerMovement(directionV3);
                }
                else if (directionInt != Vector2.zero && turn)
                {
                    directionV3 = new Vector3(transform.position.x + directionInt.x, 0.5f, transform.position.z + directionInt.y);
                    transform.LookAt(directionV3);
                }

                if (attack && !isMoving)
                {
                    characterBattleManager.Attack(directionV3);
                    isMoving = true;
                }

                if (pause)
                {
                    UIManager.S.TogglePauseMenu();
                    InputManager.TOGGLEACTIONMAP(InputManager.INPUTACTIONS.UI);
                }
                break;
            case CharacterType.NPC:
                stateManager.CheckState();

                switch (stateManager.currentAIState)
                {
                    case(AIState.wandering):
                        if (!hasPath || npcPath.Count == 0)
                        { 
                            Refresh(PickAPoint()); 
                        }
                        NPCMovement();
                        break;
                    case(AIState.investigating):
                        // This will remain constant so we should be fine with just running it once
                        Refresh(stateManager.currentState.investigatePos);
                        NPCMovement();
                        break;
                    case(AIState.chasing):
                        if(target == null) TurnManager.S.EndTurn();
                        else
                        {
                            Pathfinder.S.CreatePath(this, target.position);
                            CostToChangePath();
                            NPCMovement();   
                        }
                        break;
                    case(AIState.attacking):
                        characterBattleManager.Attack(transform.forward);
                        isMoving = true;
                        break;
                }
                break;
            case CharacterType.Stationary:
                TurnManager.S.EndTurn();
                break;
        }
    }

    private void Refresh(Vector3 destination)
    {
        npcPath.Clear(); // We need to clear the path if we are wandering and have hit another ally
        Pathfinder.S.CreatePath(this, destination);  
        npcPath.AddRange(Pathfinder.S.pathPoints);
        hasPath = true;
    }
    
    private Vector3 PickAPoint()
    {
        
        // This needs to be rewritten to only grab points that are in rooms!
        float xPoint = UnityEngine.Random.Range(0, MazeGeneratorRooms.S.mazeWidth);
        float yPoint = UnityEngine.Random.Range(0, MazeGeneratorRooms.S.mazeHeight);
        
        return new Vector3(xPoint, 0.5f, yPoint);
    }


    // This is A* adjacent chicanery, we check if it is worth it to change the current path we are on
    //to the player
    private void CostToChangePath()
    {
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
    }
    
    private void NPCMovement()
    {
        
        // Checking that our current position is spot 0
        Vector3 transRouned = transform.position;
        transRouned.x = Mathf.RoundToInt(transform.position.x);
        transRouned.y = 0.5f;
        transRouned.z = Mathf.RoundToInt(transform.position.z);
        
        if (npcPath.Count > 0 && npcPath[0] == transRouned) npcPath.RemoveAt(0);
        
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
        if (Pathfinder.S.IsPositionOccupied(newPos))
        {
            // Need to avoid characters getting stuck on each other while wandering
            if(stateManager.currentAIState == AIState.wandering) hasPath = false;
            TurnManager.S.EndTurn();
            return;
        }
        StartCoroutine(MoveTowardsTarget(directionV3));
        
        isMoving = true;
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
        if (Pathfinder.S.IsPositionOccupied(newPos))
        {
            TurnManager.S.EndTurn();
            return;
        }
        
        
        
        // Checks for walls
        if (!Physics.Linecast(transform.position, newPos, playerLayerMask, QueryTriggerInteraction.Ignore))
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
        
        // We need the origin pos for Lerping. Otherwise our origin for the Lerp will be constantly moving!
        Vector3 originPos = transform.position;
        
        Vector3 newPos = new Vector3(originPos.x + dirX, 0.5f, originPos.z + dirz);
        
        if (!rend.isVisible) movementDelay = 0.05f;
        else movementDelay = defaultMovementDelay;
        
        // Because an Enumerator only updates when resources are available
        // We cannot use delta time! We instead need to make our own delta time
        float lastTime = Time.time;
        float elapsedTime = 0;
        
        while (elapsedTime < movementDelay)
        {
            float currTime = Time.time;
            float deltaTime = currTime - lastTime;
            
            float u = Mathf.Clamp01(elapsedTime / movementDelay);
            transform.position = Vector3.Lerp(originPos , newPos,(u));
            lastTime = currTime;
            elapsedTime += deltaTime;
                    
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

    public void OnExit()
    {
        InputManager.INPUTACTIONS.Player.Disable();
    }
}
