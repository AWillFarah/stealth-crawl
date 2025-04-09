using System;
using UnityEngine;

public enum AIState
{
    wandering, chasing, investigating, attacking
}

public class StateManager : MonoBehaviour
{
    public State currentState;
    public AIState currentAIState;
    
   public void CheckState()
    {
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
        currentState = nextState;
    }
}
