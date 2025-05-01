using UnityEngine;


public class DummyItem : TemplateItem
{
    [SerializeField] GameObject dummyPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Use()
    {
        Vector3 newPos = CharacterBattleManager.PLAYER.transform.position +
                         (CharacterBattleManager.PLAYER.transform.forward).normalized;
        if(!Pathfinder.S.IsPositionOccupied(newPos))
        {
            
            Instantiate(dummyPrefab, newPos , transform.rotation);
            TurnManager.S.EndTurn();
            DisableUI();
            Destroy(this.gameObject);
        }
    }
    
}
