using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Math = UnityEngine.Mathf;

public class EntityMovement : MonoBehaviour
{
    /// <summary>
    /// When an entity wants to move, the game is going to do a ray cast and see if there is a wall in front of them.
    /// If there isn’t they will move to the space
    /// </summary>
    
    // How far we want to raycast
    [Header("Inscribed")]
    public int rayCastDistance = 1;
    [SerializeField] private float movementDelay = 0.1f;
    [SerializeField] private bool isPlayer;
    
    [Header("Dynamic")]
    
    private bool delayMovement = false;
    private bool canMove = true;
    private InputSystem_Actions playerControls;
    void Awake()
    {
        
        if (isPlayer)
        {
            playerControls = new InputSystem_Actions();
            playerControls.Player.Enable();
        }
    }

    public virtual void FixedUpdate()
    {
        if (isPlayer)
        {
            Vector2 move = playerControls.Player.Move.ReadValue<Vector2>();
            Vector2 direction = move;
            Vector2Int directionInt = Vector2Int.RoundToInt(direction);
            
            //We need a delay to the movement so we will need to use an IEnum
            IEnumerator moveSpace = MoveSpace(directionInt);
            // Make sure we actually get input!
            if (direction != Vector2.zero)
            {
                StartCoroutine(moveSpace);
                TurnManager.S.EndTurn(gameObject);
            }
            
            
            
        }
    }

    public IEnumerator MoveSpace(Vector2 direction)
    {
        canMove = true;
        // We need to let the game know we are occupying a space
        Pathfinder.S.CheckOccupiedPositions();
        
        // We are getting the direction we want to move in + our current position
        float dirX = direction.x;
        float dirY = direction.y;
        
        
        float myX = transform.position.x;
        float myY = transform.position.y;
        // We need the Z position but we will be modifying it with the input Y value
        float myZ = transform.position.z;
        
        // We need the direction to raycast
        Vector3 rayCastDir = new Vector3(dirX, myY, dirY);
        
        Vector3 newPos = new Vector3(myX + dirX, myY, myZ + dirY);
        Vector3 lookPoint = new Vector3(dirX, myY, dirY);
        transform.LookAt(newPos);
        // I hate having to use large trees of if statements but no way around it in an IEnum
        // We need (the player only)
        // check if what we hit isnt a wall, this will come in handy later with things such as treasures
        if (!Physics.Raycast(transform.position, rayCastDir, out RaycastHit hit, rayCastDistance))
        {
            // If we can't move our turn will just end
            foreach (Vector3 pos in Pathfinder.S.occupiedPositions)
            {
                if (pos == newPos)
                {
                    if(!isPlayer) print("cant move");
                    canMove = false;
                }
            }
            Debug.DrawRay(transform.position, rayCastDir, Color.red);
            
            if (!delayMovement && canMove)
            {
                delayMovement = true;
                
                
                float elapsedTime = 0;
                while (elapsedTime < movementDelay)
                {
                    transform.position=Vector3.Slerp(transform.position , newPos,(elapsedTime / movementDelay));
                    elapsedTime += Time.deltaTime;
                    
                    
                    // Yield here
                    yield return null;
                }
                
             
                delayMovement = false;
            }

            if (direction == Vector2.zero)
            {
                float posX = Mathf.Round(transform.position.x);
                float posZ = Mathf.Round(transform.position.z);
            
                transform.position = new Vector3(posX, transform.position.y,posZ);
            }  
            

        }
        
        
        yield break;
    }
}
