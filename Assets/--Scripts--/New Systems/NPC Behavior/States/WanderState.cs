using UnityEngine;
using UnityEngine.Serialization;

public class WanderState : State
{
    
    public override State RunCurrentState()
    {
        if (canSeeEnemy())
        {
            thisState = AIState.chasing;
            return chaseState;
        }

        if (heardSomething)
        {
            thisState = AIState.investigating;
            investigateState.investigatePos = investigatePos;
            return investigateState;
        }
        else
        {
            thisState = AIState.wandering;
            return this;
        }
    }

    
}
