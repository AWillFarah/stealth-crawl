using System;
using JetBrains.Annotations;
using UnityEngine;

public class TemplateItem : MonoBehaviour
{
    [Header("Inscribed")]
    public String itemName = "Test";
    public ItemSO itemSO;

    public void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        itemName = itemSO.name;
        spriteRenderer.sprite = itemSO.sprite;
        spriteRenderer.color = itemSO.spriteColor;
    }
    
    public virtual void Use()
    {
        CharacterBattleManager.PLAYER.ChangeHealth(-1, false);
        TurnManager.S.EndTurn();
        Destroy(gameObject);
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
