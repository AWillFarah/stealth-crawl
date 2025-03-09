using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public GameObject camPoint;
    public float movementDelay;
    void Start()
    {
        if (player == null) Debug.LogError("Player is null");
        this.transform.position = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);
    }

    
    void FixedUpdate()
    {
        Follow();
        
    }

    private void Follow()
    {
        float playerX = player.transform.position.x;
        float camY = this.transform.position.y;
        float playerZ = player.transform.position.z;
        Vector3 targetPos = new Vector3(playerX, camY, playerZ);
        if (this.transform.position != targetPos)
        {
            Vector3 p0 = camPoint.transform.position;
            Vector3 p1 = targetPos;
            Vector3 p01 = (1- movementDelay) * p0 + movementDelay * p1;
            this.transform.position = p01;
        }
    }
}
