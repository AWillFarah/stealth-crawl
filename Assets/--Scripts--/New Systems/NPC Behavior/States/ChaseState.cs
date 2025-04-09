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
        else
        {
            thisState = AIState.chasing;
            return this;  
        }
        
        
    }
}
