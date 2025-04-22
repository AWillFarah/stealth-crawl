using System;
using JetBrains.Annotations;
using UnityEngine;

public class TemplateItem : MonoBehaviour
{
    [Header("Inscribed")]
    public ItemType typeOfItem;
    
    
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
               ItemManager.S.AddItem(this);
               Destroy(gameObject);
            }
        }
    }
}
