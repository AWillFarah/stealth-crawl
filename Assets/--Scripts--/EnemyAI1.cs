using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI1 : EntityMovement
{
    private Vector2[] directions = new Vector2[] { Vector2.left, Vector2.right, Vector2.up, Vector2.down };
    
    [Header("Dynamic")]
    [SerializeField] int gridHeight = 10;
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private float cellHeight = 1f;
    [SerializeField] private float cellWidth = 1f;

    [SerializeField] private bool generatePath;
    [SerializeField] private bool pathGenerated;
    
    private Dictionary<Vector3, Cell> cells;

    [SerializeField] private List<Vector3> cellsToSearch;
    [SerializeField] private List<Vector3> searchedCells;
    [SerializeField] private List<Vector3> finalPath;
    void Awake()
    {
        cells = new Dictionary<Vector3, Cell>();
    }

    // Generates grid for pathfinding
    private void GenerateGrid()
    {
        if(MazeGenerator.S == null) print("no singleton");
        //gridHeight = MazeGenerator.S.mazeHeight;
        //gridWidth = MazeGenerator.S.mazeWidth;
        
        for (float x = 0; x < gridWidth; x += cellWidth)
        {
            for (float y = 0; y < gridHeight; y += cellHeight)
            {
                Vector3 pos = new Vector3(x, 0, y);
                if (!cells.ContainsKey(pos))  // Prevent duplicate key error
                {
                    cells.Add(pos, new Cell(pos));
                    Debug.Log($"Added cell at: {pos}");
                }
            }
        }
    }


    private void FindPath(Vector3 startPos, Vector3 endPos)
    {
        searchedCells = new List<Vector3>();
        cellsToSearch = new List<Vector3>{startPos};
        finalPath = new List<Vector3>();
        
        
        cells[startPos].gCost = 0;
        cells[startPos].hCost = GetDistance(startPos, endPos);
        cells[startPos].fCost = GetDistance(startPos, endPos);
        //Cell startCell = cells[startPos];
        print("do we run this");

        while (cellsToSearch.Count > 0)
        {
            Vector3 cellToSearch = cellsToSearch[0];

            foreach (Vector3 pos in cellsToSearch)
            {
                Cell c = cells[pos];
                if (c.fCost < cells[cellToSearch].fCost ||
                    c.fCost == cells[cellToSearch].fCost && c.hCost == cells[cellToSearch].hCost)
                {
                    cellToSearch = pos;
                }
            }
            
            cellsToSearch.Remove(cellToSearch);
            searchedCells.Add(cellToSearch);

            if (cellToSearch == endPos)
            {
                //Find Path
                return;
            }
            
            SearchCellNeighbors(cellToSearch, endPos);
        }
    }

    private void SearchCellNeighbors(Vector3 cellPos, Vector3 endPos)
    {
        for (float x = cellPos.x - cellWidth; x <= cellWidth + cellPos.x; x += cellWidth)
        {
            for (float y = cellPos.y - cellHeight; y <= cellHeight + cellPos.y; y += cellHeight)
            {
                float cellPosX = cellPos.x;
                float cellPosY = cellPos.y;
                
                // I need to check for walls here by doing a raycast
                Vector3 cellPosDir = new Vector3(cellPosX, 0.5f, cellPosY);
                Vector3 neighborPos = new Vector3(x, 0.5f, y);
                Vector3 rayCastDir = neighborPos - cellPosDir; 
                //if(Physics.Raycast(transform.position, rayCastDir, out RaycastHit hit, cellHeight) &&
                //    hit.transform.CompareTag("Wall")) cells[neighborPos].isWall = true;
                if (cells.TryGetValue(neighborPos, out Cell c) && !searchedCells.Contains(neighborPos))
                {
                    int GcostToNeighbour = cells[cellPos].gCost + GetDistance(cellPos, neighborPos);

                    if (GcostToNeighbour < cells[neighborPos].gCost)
                    {
                        Cell neighbourNode = cells[neighborPos];

                        neighbourNode.connection = cellPos;
                        neighbourNode.gCost = GcostToNeighbour;
                        neighbourNode.hCost = GetDistance(neighborPos, endPos);
                        neighbourNode.fCost = neighbourNode.gCost + neighbourNode.hCost;

                        if (!cellsToSearch.Contains(neighborPos))
                        {
                            cellsToSearch.Add(neighborPos);
                        }
                    }
                }
            }
        }
    }

    private int GetDistance(Vector3 pos1, Vector3 pos2)
    {
        Vector3Int dist = new Vector3Int(Mathf.Abs((int)pos1.x - (int)pos2.x), 1, Mathf.Abs((int)pos1.y - (int)pos2.y));
        
        int lowest = Mathf.Min(dist.x, dist.y);
        int highest = Mathf.Max(dist.x, dist.y);
        
        int horizontalMovesRequired = highest - lowest;
        
        // We are multiplying by 14 here because diagonal spaces are 1.4 units away (we are using ints)
        return lowest * 14 + horizontalMovesRequired * 10;
    }
    
    public override void FixedUpdate()
    {
        /*
        int random = Random.Range(0, directions.Length);
        Vector2 direction = directions[random];
        
        // Everything pass here is pretty identical
        Vector2Int directionInt = Vector2Int.RoundToInt(direction);
            
        //We need a delay to the movement so we will need to use an IEnum
        IEnumerator moveSpace = MoveSpace(directionInt);
        StartCoroutine(moveSpace); 
        */
        
        if (generatePath && !pathGenerated)
        {
            GenerateGrid();
            FindPath(new Vector3(1, 0, 1), new Vector3(5, 0, 5));  
            pathGenerated = true;
        }
        
        
        //TurnManager.S.EndTurn(gameObject);
    }
    
    private class Cell
    {
        public Vector3 position;
        public int fCost = int.MaxValue;
        public int gCost  = int.MaxValue; // Distances from the nodes to the start
        public int hCost  = int.MaxValue; // Distance from the cell to the end position
        public Vector3 connection;
        public bool isWall;

        public Cell(Vector3 pos)
        {
            position = pos;
        }
    }
}


