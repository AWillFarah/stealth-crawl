using UnityEngine;
using System.Collections.Generic;
public class WarpItem : TemplateItem
{

    public override void Use()
    {
        List<Vector2Int> roomTiles = new List<Vector2Int>();
        foreach (Room r in MazeGeneratorRooms.S.rooms)
        {
            foreach (Vector2Int t in r.tilesInRoom)
            {
                roomTiles.Add(t);
            }
        }
        
        Vector2Int tile = roomTiles[Random.Range(0, roomTiles.Count - 1)];
        
        CharacterBattleManager.PLAYER.transform.position = new Vector3(tile.x, 0.5f, tile.y);
        TurnManager.S.EndTurn();
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
