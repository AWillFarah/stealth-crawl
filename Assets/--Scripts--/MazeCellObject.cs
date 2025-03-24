using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCellObject : MonoBehaviour
{
    [SerializeField] private GameObject topWall;
    [SerializeField] private GameObject bottomWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    
    [Header("Tiling")]
    [Tooltip("For when walls are adjacent on both sides")]
    [SerializeField] private Mesh adjacentMesh;
    [Tooltip("For when there is one wall adjacent")]
    [SerializeField] private Mesh oneAdjacentMesh;

    public bool leftActive;
    public bool topActive;
    public bool rightActive;
    public bool bottomActive;
   
    public void Init(bool top, bool bottom, bool left, bool right)
    {
        topWall.SetActive(top);
        topActive = top;
        bottomWall.SetActive(bottom);
        bottomActive = bottom;
        bottomActive = bottom;
        leftWall.SetActive(left);
        leftActive = left;
        rightWall.SetActive(right);
        rightActive = right;
    }

    public void ChangeMesh(bool changingLeft, bool flipWall, int wallPoints)
    {
        
        GameObject gameObjectToChange = changingLeft ? leftWall : topWall;
        MeshFilter meshFilter = gameObjectToChange.GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        
        // We need to check if we are flipping the tile or not
        
        
        switch (wallPoints)
        {
            case 1:
                meshFilter.mesh = oneAdjacentMesh;
                if (flipWall)
                {
                    gameObjectToChange.transform.rotation *= Quaternion.Euler(0, 180, 0);
                }
                break;
            case 2:
                meshFilter.mesh = adjacentMesh;
                break;
        }
        
      
    }
}
