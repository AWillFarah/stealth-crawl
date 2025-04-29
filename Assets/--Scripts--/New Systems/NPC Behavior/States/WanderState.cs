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
            MessageLogManager.S.AddMessageToLog((cBM.stats.name + " heard something"));
            return investigateState;
        }
        else
        {
            thisState = AIState.wandering;
            return this;
        }
    }

    
}
