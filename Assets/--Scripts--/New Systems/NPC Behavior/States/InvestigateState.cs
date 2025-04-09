using UnityEngine;

public class InvestigateState : State
{
    public override State RunCurrentState()
    {
        if (canSeeEnemy())
        {
            thisState = AIState.chasing;
            return chaseState;
        }
        else
        {
            thisState = AIState.investigating;
            return this; 
        }
        
        // We will need to check if we have reached our destination
    }
}
