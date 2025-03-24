using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class Pathfinder : MonoBehaviour
{
    
    /// <summary>
    /// This script is used for when NPCs are requesting a path to a target transform
    /// It finds a path betweem them and the target, rounds out the numbers to whole, and then passes the list
    /// back to them
    /// </summary>
    
    public static Pathfinder S;
    
    [Header("Inscribed")]
    [SerializeField] private LineRenderer pathRenderer;
    public bool drawPath = false;
    [SerializeField]
    private float PathHeightOffset = 1.25f;
    [SerializeField]
    private float SpawnHeightOffset = 1.5f;
    [SerializeField]
    private float PathUpdateSpeed = 0.25f;
    
    [Header("Dynamic")]
    [SerializeField]
    private NPCAI npc;
    [SerializeField] private Transform targetTransform;
    private NPCAI activeInstance;
    private NavMeshTriangulation Triangulation;
    private Coroutine DrawPathCoroutine;
    
    public List<Vector3> pathPoints = new List<Vector3>();
    public List<Vector3> occupiedPositions = new List<Vector3>();
    
    
    private void Awake()
    {
        S = this;
        Triangulation = NavMesh.CalculateTriangulation();
        GameObject[] tempArray = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject temp in tempArray)
        {
            occupiedPositions.Add(temp.transform.position);
        }
    }

   
    public void CreatePath(NPCAI npcObj, npcState currentState)
    {
        
        activeInstance = npcObj;
        Triangulation = NavMesh.CalculateTriangulation();
        switch (currentState)
        {
            case npcState.patrol:
                GameObject[] tempArray = GameObject.FindGameObjectsWithTag("Ground");
                int randomIndex = Random.Range(0, tempArray.Length);
                targetTransform = tempArray[randomIndex].transform;
                break;
            case npcState.chasing:
                targetTransform = GameObject.Find("Player").transform;
                break;
        }
        /*activeInstance = Instantiate(npc,
            Triangulation.vertices[Random.Range(0, Triangulation.vertices.Length)] + Vector3.up * SpawnHeightOffset,
            Quaternion.Euler(90, 0, 0));*/
        //activeInstance.pathfinder = this;
        
        if (DrawPathCoroutine != null)
        {
            StopCoroutine(DrawPathCoroutine);
        }

        DrawPathCoroutine = StartCoroutine(FindPath());
    }

    private IEnumerator FindPath()
    {
        WaitForSeconds Wait = new WaitForSeconds(PathUpdateSpeed);
        NavMeshPath path = new NavMeshPath();

        while (activeInstance != null)
        {
            if (NavMesh.CalculatePath(activeInstance.transform.position, targetTransform.position,  NavMesh.AllAreas, path))
            {
                pathRenderer.positionCount = path.corners.Length;
                pathPoints.Clear();
                for (int i = 0; i < path.corners.Length; i++)
                {
                    // We need whole numbers here so we need to round our coordinates
                    // We also always want Y to by 0.5 because thats where all our entities are at
                    float pathX = Mathf.Round(path.corners[i].x);
                    float pathY = 0.5f;
                    float pathZ = Mathf.Round(path.corners[i].z);
                    Vector3 pathRounded = new Vector3(pathX, pathY, pathZ);
                    pathPoints.Add(pathRounded);
                    // Draws line renderer
                    if(drawPath) pathRenderer.SetPosition(i, pathRounded + Vector3.up * PathHeightOffset);
                }
                if(activeInstance.gameObject == TurnManager.S.turnOrder[TurnManager.S.turnOrderIndex]) activeInstance.ChoosePath(pathPoints);
            }
            else
            {
                Debug.LogError($"Unable to calculate a path on the NavMesh between {targetTransform.position} and {activeInstance.transform.position}!");
            }

            yield return Wait;
        }
    }

    public void CheckOccupiedPositions()
    {
        // Start off by wiping out the previous list
        GetOccupiedPositions().Clear();
        GetOccupiedPositions();
    }
    
    public Dictionary<GameObject, Vector3> GetOccupiedPositions()
    {
        
        GameObject[] tempArray = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject temp in tempArray)
        {
            GetOccupiedPositions().Add(temp, temp.transform.position);
        }

        return positionsOccupied;
    }
}
