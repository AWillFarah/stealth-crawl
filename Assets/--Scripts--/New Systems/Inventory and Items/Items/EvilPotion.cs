using UnityEngine;

public class EvilPotion : TemplateItem
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Use()
    {
        CharacterBattleManager.PLAYER.ChangeHealth(1, true);
        Destroy(gameObject);
        TurnManager.S.EndTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
