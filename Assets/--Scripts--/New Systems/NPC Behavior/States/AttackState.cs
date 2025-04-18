using UnityEngine;

public class AttackState : State
{
   
    
    public override State RunCurrentState()
    {
        if (!isInRange())
        {
            thisState = AIState.chasing;
            return chaseState;
        }
        if (cM.target == null)
        {
            thisState = AIState.wandering;
            return wanderState;
        }
        else
        {
            thisState = AIState.attacking;
            return this;
        }
    }
}
