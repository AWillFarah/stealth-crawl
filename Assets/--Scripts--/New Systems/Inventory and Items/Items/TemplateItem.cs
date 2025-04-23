using System;
using JetBrains.Annotations;
using UnityEngine;

public class TemplateItem : MonoBehaviour
{
    [Header("Inscribed")]
    public ItemSO itemSO;
    public String itemName = "Test";
    
    public void Use()
    {
        CharacterBattleManager.PLAYER.ChangeHealth(-1, false);
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterMovement>() != null)
        {
            CharacterMovement characterMovement = other.GetComponent<CharacterMovement>();

            if (characterMovement.characterType == CharacterType.Player)
            {
               
               Destroy(gameObject);
            }
        }
    }
}
