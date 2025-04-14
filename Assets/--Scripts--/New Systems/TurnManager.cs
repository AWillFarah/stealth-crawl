using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager S;
    
    [Header("Dynamic")]
    [Tooltip("The order of turns for the scene")]
    public List<GameObject> turnOrder = new List<GameObject>();
    
    void Start()
    {
        S = this;
        QueueTurns();
        StartTurn(turnOrder[0]);
    }

    void QueueTurns()
    {
        GameObject[] tempArray = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject temp in tempArray)
        {
            CharacterMovement characterMovement = temp.GetComponent<CharacterMovement>();
            characterMovement.enabled = false;
            turnOrder.Add(temp.gameObject);
        } 
        
    }
    void StartTurn(GameObject currentTurn)
    {
        
        currentTurn.GetComponent<CharacterMovement>().enabled = true;
    }
    
    public void EndTurn()
    {
        GameObject character = turnOrder[0];
        // I realllllyyy wanna be sure they're in the right spot because ive been getting some rounding errors
        character.transform.position = new Vector3(Mathf.RoundToInt(character.transform.position.x), 0.5f, 
        Mathf.RoundToInt(character.transform.position.z));
        
        character.GetComponent<CharacterMovement>().enabled = false;

        turnOrder.RemoveAt(0);
        if (turnOrder.Count <= 0) QueueTurns();
        if(turnOrder[0] == null) QueueTurns();
        else StartTurn(turnOrder[0]);
    }
}
