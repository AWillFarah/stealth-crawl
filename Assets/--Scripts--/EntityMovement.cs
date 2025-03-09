using System.Collections;
using System.Collections.Generic;
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
    private int rayCastDistance = 1;
    [SerializeField] private float movementDelay = 0.1f;
    [SerializeField] private bool isPlayer;
    
    [Header("Dynamic")]
    
    private bool delayMovement = false;
    private InputSystem_Actions playerControls;
    void Awake()
    {
        
        if (isPlayer)
        {
            playerControls = new InputSystem_Actions();
            playerControls.Player.Enable();
        }
    }

    void FixedUpdate()
    {
        if (isPlayer)
        {
            Vector2 move = playerControls.Player.Move.ReadValue<Vector2>();
            Vector2 direction = move;
            
            print(direction);
            Vector2Int directionInt = Vector2Int.RoundToInt(direction);
            
            //We need a delay to the movement so we will need to use an IEnum
            IEnumerator moveSpace = MoveSpace(directionInt);
            StartCoroutine(moveSpace);
            
            
        }
    }
    
    // For player character only
    /* void OnMove(InputValue value)
    {
        Vector2 direction = value.Get<Vector2>();
        // I need this to always be a whole number (1 or -1)
        Vector2Int directionInt = Vector2Int.RoundToInt(direction);
        print(directionInt);
        MoveSpace(directionInt);
        
    } */

    private IEnumerator MoveSpace(Vector2 direction)
    {
        
        // We are getting the direction we want to move in + our current position
        float dirX = direction.x;
        float dirY = direction.y;
        
        
        float myX = transform.position.x;
        // We need the Z position but we will be modifying it with the input Y value
        float myZ = transform.position.z;
        
        // We need the direction to raycast
        Vector3 rayCastDir = new Vector3(dirX, 0, dirY);
        
        Vector3 newPos = new Vector3(myX + dirX, 0, myZ + dirY);
        Vector3 lookPoint = new Vector3(dirX, 0, dirY);
        transform.LookAt(newPos);
        if (!Physics.Raycast(transform.position, rayCastDir, out RaycastHit hit, rayCastDistance))
        {
            
            

            if (!delayMovement)
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
    }
}
