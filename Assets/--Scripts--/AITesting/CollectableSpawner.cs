using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollectableSpawner : MonoBehaviour
{
    
    
    [SerializeField]
    private Collectable Prefab;
    [SerializeField]
    private Transform Player;
    [SerializeField]
    private LineRenderer Path;
    [SerializeField]
    private float PathHeightOffset = 1.25f;
    [SerializeField]
    private float SpawnHeightOffset = 1.5f;
    [SerializeField]
    private float PathUpdateSpeed = 0.25f;

    private Collectable ActiveInstance;
    private NavMeshTriangulation Triangulation;
    private Coroutine DrawPathCoroutine;
    public List<Vector3> pathPoints = new List<Vector3>();

    private void Awake()
    {
        Triangulation = NavMesh.CalculateTriangulation();
    }

   
    public void SpawnNewObject()
    {
        Triangulation = NavMesh.CalculateTriangulation();
        ActiveInstance = Instantiate(Prefab,
            Triangulation.vertices[Random.Range(0, Triangulation.vertices.Length)] + Vector3.up * SpawnHeightOffset,
            Quaternion.Euler(90, 0, 0));
        ActiveInstance.Spawner = this;

        if (DrawPathCoroutine != null)
        {
            StopCoroutine(DrawPathCoroutine);
        }

        DrawPathCoroutine = StartCoroutine(DrawPathToCollectable());
    }

    private IEnumerator DrawPathToCollectable()
    {
        WaitForSeconds Wait = new WaitForSeconds(PathUpdateSpeed);
        NavMeshPath path = new NavMeshPath();

        while (ActiveInstance != null)
        {
            if (NavMesh.CalculatePath(Player.position, ActiveInstance.transform.position, NavMesh.AllAreas, path))
            {
                Path.positionCount = path.corners.Length;
                pathPoints.Clear();
                for (int i = 0; i < path.corners.Length; i++)
                {
                    float pathX = Mathf.Round(path.corners[i].x);
                    float pathY = Mathf.Round(path.corners[i].y);
                    float pathZ = Mathf.Round(path.corners[i].z);
                    Vector3 pathRounded = new Vector3(pathX, pathY, pathZ);
                    pathPoints.Add(pathRounded);
                    Path.SetPosition(i, pathRounded + Vector3.up * PathHeightOffset);
                }
            }
            else
            {
                Debug.LogError($"Unable to calculate a path on the NavMesh between {Player.position} and {ActiveInstance.transform.position}!");
            }

            yield return Wait;
        }
    }
}
