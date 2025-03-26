using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCharacterMovement : MonoBehaviour
{
    public float movementDelay = 0.5f;


    public IEnumerator MoveTowardsTarget(Vector3 newPos)
    {
        
        float elapsedTime = 0;
        while (elapsedTime < movementDelay)
        {
            transform.position=Vector3.Slerp(transform.position , newPos,(elapsedTime / movementDelay));
            elapsedTime += Time.deltaTime;
                    
                    
            yield return null;
        }

        if (elapsedTime >= movementDelay)
        {
            TurnManagerNew.S.EndTurn();
        }
    }
}
