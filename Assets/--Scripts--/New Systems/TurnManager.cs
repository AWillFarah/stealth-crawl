using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
    }

    void Update()
    {
        List<GameObject> removeList = new List<GameObject>();
        foreach (GameObject npc in turnOrder)
        {
            if (npc == null)
            {
                removeList.Add(npc);
            }
        }

        foreach (var obj in removeList)
        {   
            turnOrder.Remove(obj);   
        }
    }
    
    void QueueTurns()
    {
        GameObject[] tempArray = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject temp in tempArray)
        {
            CharacterMovement characterMovement = temp.GetComponent<CharacterMovement>();
            characterMovement.enabled = false;
            turnOrder.Add(temp.gameObject);
            if (temp == tempArray.Last())
            {
                StartTurn(turnOrder[0]); 
            }
        } 
        
    }
    void StartTurn(GameObject currentTurn)
    {
        
        currentTurn.GetComponent<CharacterMovement>().enabled = true;
    }
    
    public void EndTurn()
    {
        if (turnOrder.Count == 0) return;
        
        GameObject character = turnOrder[0];
        // I realllllyyy wanna be sure they're in the right spot because ive been getting some rounding errors
        if (character != null)
        {
            character.transform.position = new Vector3(Mathf.RoundToInt(character.transform.position.x), 0.5f, 
                Mathf.RoundToInt(character.transform.position.z));
        
            character.GetComponent<CharacterMovement>().enabled = false; 
        }
        

        turnOrder.RemoveAt(0);
        if (turnOrder.Count <= 0) QueueTurns();
        if(turnOrder[0] == null) QueueTurns();
        else if (turnOrder.Count > 0)
        {
            if (turnOrder[0] == null)
            {
                QueueTurns();
            }

            if (turnOrder.Count > 0 && turnOrder[0] != null)
            {
                StartTurn(turnOrder[0]);
            }
        }
    }
}
