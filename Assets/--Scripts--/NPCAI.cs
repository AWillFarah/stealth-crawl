using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAI : EntityMovement
{
    private Vector2[] directions = new Vector2[] { Vector2.left, Vector2.right, Vector2.up, Vector2.down };
    
    public NavMeshAgent agent;
    public NavMeshPath path;
    
    //[Header("Dynamic")]
  
    void Awake()
    {
        GameObject player = GameObject.Find("Player");
        agent.SetDestination(player.transform.position);
    }

    
    
    public override void FixedUpdate()
    {
        /*
        int random = Random.Range(0, directions.Length);
        Vector2 direction = directions[random];
        
        // Everything pass here is pretty identical
        Vector2Int directionInt = Vector2Int.RoundToInt(direction);
            
        //We need a delay to the movement so we will need to use an IEnum
        IEnumerator moveSpace = MoveSpace(directionInt);
        StartCoroutine(moveSpace); 
        */
        
       
        
        
        //TurnManager.S.EndTurn(gameObject);
    }
    
    
}


