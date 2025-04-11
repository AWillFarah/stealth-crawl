using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum AIState
{
    wandering, chasing, investigating, attacking
}

public class StateManager : MonoBehaviour
{
    public State currentState;
    public AIState currentAIState;
    private CharacterMovement character;
    
   public void CheckState()
    {
        character = gameObject.GetComponent<CharacterMovement>();
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState(); // If current state does not = null, run it
        currentAIState = nextState.thisState;
        if (nextState != null)
        {
            SwitchToNextState(nextState);
        }
        
        
    }

    private void SwitchToNextState(State nextState)
    {
        // We should only clear our path if our current state != our next state
        if (currentState != nextState)
        {
            character.hasPath = false;  
        }
        currentState = nextState;
        
    }
}
