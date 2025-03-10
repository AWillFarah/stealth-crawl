using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class NPCAI : EntityMovement
{
    private Vector2[] directions = new Vector2[] { Vector2.left, Vector2.right, Vector2.up, Vector2.down };
    
    
    
    //[Header("Dynamic")]
  
    void Awake()
    {
        
    }

    void Start()
    {
      
    }

    void MakePath()
    {
        
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


