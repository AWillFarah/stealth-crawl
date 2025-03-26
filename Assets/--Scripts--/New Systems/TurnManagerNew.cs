using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManagerNew : MonoBehaviour
{
    public static TurnManagerNew S;
    
    [Header("Dynamic")]
    [Tooltip("The order of turns for the scene")]
    public Queue<GameObject> turnOrder = new Queue<GameObject>();
    
    void Start()
    {
        S = this;
        QueueTurns();
        StartTurn(turnOrder.Peek());
    }

    void QueueTurns()
    {
        GameObject[] tempArray = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject temp in tempArray)
        {
            turnOrder.Enqueue(temp.gameObject);
        } 
        
    }
    void StartTurn(GameObject currentTurn)
    {
        print("it is " + currentTurn + "'s turn");
        NewCharacterMovement newCharacterMovement = currentTurn.GetComponent<NewCharacterMovement>();
        
        IEnumerator moveSpace = newCharacterMovement.MoveTowardsTarget(new Vector3(1,0,1));
        StartCoroutine(moveSpace);
    }

 

    public void EndTurn()
    {
        turnOrder.Dequeue();
        if (turnOrder.Count <= 0) QueueTurns();
        StartTurn(turnOrder.Peek());
        
        
    }
}
