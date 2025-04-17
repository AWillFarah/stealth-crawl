using NUnit.Framework.Constraints;
using UnityEngine;

public class ChaseState : State
{

    

    public override State RunCurrentState()
    {
        if (isInRange())
        {
            thisState = AIState.attacking;
            attackState.target = target;
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
