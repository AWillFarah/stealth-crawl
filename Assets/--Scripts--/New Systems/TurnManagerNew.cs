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
            CharacterMovement characterMovement = temp.GetComponent<CharacterMovement>();
            characterMovement.enabled = false;
            turnOrder.Enqueue(temp.gameObject);
        } 
        
    }
    void StartTurn(GameObject currentTurn)
    {
        print("it is " + currentTurn + "'s turn");
        currentTurn.GetComponent<CharacterMovement>().enabled = true;
    }
    
    public void EndTurn()
    {
        GameObject character = turnOrder.Peek();
        // I realllllyyy wanna be sure they're in the right spot because ive been getting some rounding errors
        character.transform.position = new Vector3(Mathf.RoundToInt(character.transform.position.x), 0.5f, 
        Mathf.RoundToInt(character.transform.position.z));
        character.GetComponent<CharacterMovement>().enabled = false;
        turnOrder.Dequeue();
        if (turnOrder.Count <= 0) QueueTurns();
        StartTurn(turnOrder.Peek());
    }
}
