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
        if (transform.position == investigatePos)
        {
            print("returning to wander");
            thisState = AIState.wandering;
            wanderState.heardSomething = false;
            return wanderState; 
        }
        else
        {
            thisState = AIState.investigating;
            return this; 
        }
        
        // We will need to check if we have reached our destination
    }
}
