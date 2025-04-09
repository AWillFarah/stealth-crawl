using UnityEngine;

public class npcAI : MonoBehaviour
{
    public Transform target;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    // void FixedUpdate()
    // {
    //     if(target == null)
    //     {
    //         if(!hasPath) Pathfinder.S.CreatePath(this, PickAPoint());
    //         else Pathfinder.S.CreatePath(this, pathDestination);
    //     }
    //         
    //         
    //     // Let's check if our target is within reach
    //     // else if (target != null &&
    //     //          Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
    //     //              out RaycastHit hit, 1.41f))
    //     // {
    //     //     print("attacking!");
    //     //     transform.LookAt(hit.point);
    //     //     characterBattleManager.Attack();
    //     //     isMoving = true;
    //     // }
    //     else
    //     {
    //         Pathfinder.S.CreatePath(this, target.position);
    //     }
    //     if(!isMoving) NPCMovement();
    // }
}
