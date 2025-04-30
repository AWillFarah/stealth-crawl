using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
   
    public abstract State RunCurrentState();
    
    [Header("Inscribed")]
    [HideInInspector] public LineOfSight lOS;
    [HideInInspector] public CharacterBattleManager cBM;
    public CharacterMovement cM;
    
    [HideInInspector] public AIState thisState;
    public ChaseState chaseState;
    public AttackState attackState;
    public WanderState wanderState;
    public InvestigateState investigateState;
    
    [Header("Dyanmic")] 
    public bool heardSomething;
    public Vector3 investigatePos;
    
    public GameObject target;
    
    void Start()
    {
        lOS = GetComponentInParent<LineOfSight>();
        cBM = GetComponentInParent<CharacterBattleManager>();
    }
    
    public bool canSeeEnemy()
    {
        if(lOS.objects == null) return false;
        foreach (GameObject gO in lOS.objects)
        {
            CharacterBattleManager c = gO.GetComponent<CharacterBattleManager>();
            if (c.teamNumber != cBM.teamNumber)
            {
                cM.target = c.transform;
                target = gO;
                return true;
            }
            
        }
        return false;
        
    }

    public void heardNoise(Vector3 pos)
    {
      heardSomething = true;  
      investigatePos = new Vector3(pos.x, 0.5f, pos.z);
    }
    
    public bool isInRange()
    {
        for (int i = 0; i <= 8; i++)
        {
            if (Physics.Raycast(transform.position, (transform.forward) , out RaycastHit hit, 1.41f))
            {
                
                CharacterBattleManager cBM = hit.collider.gameObject.GetComponent<CharacterBattleManager>();
                if (cBM != null )
                {
                    
                    if (hit.transform.gameObject == target)
                    {
                        return true; 
                    }
                    
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }
}
