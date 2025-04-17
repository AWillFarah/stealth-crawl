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
        else
        {
            thisState = AIState.attacking;
            return this;
        }
    }
}
