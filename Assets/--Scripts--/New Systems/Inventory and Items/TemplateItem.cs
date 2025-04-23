using System;
using JetBrains.Annotations;
using UnityEngine;

public abstract class TemplateItem : MonoBehaviour
{
    [Header("Inscribed")]
    public String itemName = "Test";
    public ItemSO itemSO;
    
    
    public virtual void Use()
    {
        CharacterBattleManager.PLAYER.ChangeHealth(-1, false);
        Destroy(gameObject);
        TurnManager.S.EndTurn();
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterMovement>() != null)
        {
            CharacterMovement characterMovement = other.GetComponent<CharacterMovement>();

            if (characterMovement.characterType == CharacterType.Player)
            {
               // We need a copy of the templateitem, not this one
               
               InventoryManager.S.AddItem(itemSO);
               InventoryManager.INVENTORY.Add(itemSO);
               Destroy(gameObject);
            }
        }
    }
}
