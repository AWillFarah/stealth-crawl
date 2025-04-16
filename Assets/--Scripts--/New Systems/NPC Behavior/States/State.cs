using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
   
    public abstract State RunCurrentState();
    
    [Header("Inscribed")]
    [HideInInspector] public LineOfSight lOS;
    private CharacterBattleManager cBM;
    public CharacterMovement cM;
    
    [HideInInspector] public AIState thisState;
    public ChaseState chaseState;
    public AttackState attackState;
    public WanderState wanderState;
    public InvestigateState investigateState;
    
    [Header("Dyanmic")] 
    public bool heardSomething;
    public Vector3 investigatePos;
    
    void Start()
    {
        lOS = GetComponentInParent<LineOfSight>();
        cBM = GetComponentInParent<CharacterBattleManager>();
    }
    
    public bool canSeeEnemy()
    {
        
        foreach (GameObject gO in lOS.objects)
        {
            CharacterBattleManager c = gO.GetComponent<CharacterBattleManager>();
            if (c.teamNumber != cBM.teamNumber)
            {
                cM.target = c.transform;
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
    
    
}
