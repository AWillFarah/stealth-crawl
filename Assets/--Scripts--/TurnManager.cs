using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager S;
    
    [Header("Dynamic")]
    [Tooltip("The order of turns for the scene")]
    public List<GameObject> turnOrder = new List<GameObject>();
    
    public int turnOrderIndex = 0; // The current turn
    void Start()
    {
        S = this;
        GameObject[] tempArray = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject temp in tempArray)
        {
            turnOrder.Add(temp);
            temp.GetComponent<EntityMovement>().enabled = false;
        }
        
        StartTurn(turnOrder[0]);
        
    }

    public void StartTurn(GameObject character)
    {
        character.GetComponent<EntityMovement>().enabled = true;
        NPCAI n = character.GetComponent<NPCAI>();
        if (n != null) n.Invoke("StartTurn", 0.02f);
        print("It is " + character.ToString() + "'s turn");
    }
    
    
    public void EndTurn(GameObject character)
    {
        if(character != turnOrder[turnOrderIndex]) print ("this doesnt seem to work");
        character.GetComponent<EntityMovement>().enabled = false;
        turnOrderIndex++;
        // We need the -1 because the Count of the turnOrder list
        // Is always going to be 1 larger than the last index
        if (turnOrderIndex > (turnOrder.Count -1)) 
        {
            turnOrderIndex = 0;
        }
        StartTurn(turnOrder[turnOrderIndex]);
    }
    
}
