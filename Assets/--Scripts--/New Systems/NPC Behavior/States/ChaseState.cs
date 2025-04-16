using UnityEngine;

public class ChaseState : State
{

    public bool isInRange;
    
    public override State RunCurrentState()
    {
        if (isInRange)
        {
            thisState = AIState.attacking;
            return attackState; 
        }

        if (cM.target == null)
        {
            thisState = AIState.wandering;
            return wanderState;
        }
        else
        {
            thisState = AIState.chasing;
            return this;  
        }
        
        
    }
}
