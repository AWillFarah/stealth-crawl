using System;
using JetBrains.Annotations;
using UnityEngine;

public class TemplateItem : MonoBehaviour
{
    [Header("Inscribed")]
    [HideInInspector] String itemName = "Test";
    public ItemSO itemSO;
    public SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        itemName = itemSO.name;
        spriteRenderer.sprite = itemSO.sprite;
        spriteRenderer.color = itemSO.spriteColor;
        
    }
    
    public virtual void Use()
    {
        CharacterBattleManager.PLAYER.ChangeHealth(-1, false);
        TurnManager.S.EndTurn();
        disableUI();
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
               
               
               MessageLogManager.S.AddMessageToLog("Found a " + itemSO.name + " !");
               if (InventoryManager.INVENTORY.Count < InventoryManager.INVENTORY.Capacity)
               {
                   InventoryManager.S.AddItem(itemSO);
                   InventoryManager.INVENTORY.Add(itemSO);
                   Destroy(gameObject);  
               }
               else
               {
                   MessageLogManager.S.AddMessageToLog("...Too bad your inventory was full!");  
               }
               
            }
        }
    }

    public void disableUI()
    {
        UIManager.S.TogglePauseMenu();
        InputManager.TOGGLEACTIONMAP(InputManager.INPUTACTIONS.Player);
    }
}
