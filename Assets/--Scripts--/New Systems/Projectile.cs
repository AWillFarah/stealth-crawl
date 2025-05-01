using UnityEngine;

/// <summary>
/// Projectiles will all behave fairly similar, so we can handle it all through a script like this
/// </summary>

public class Projectile : MonoBehaviour
{
    private SoundFXSO sfxVfx;
    private Rigidbody rb;
    private SpriteRenderer sr;
    [SerializeField] CharacterBattleManager characterBattleManager; // This needs a cBM for damage calculations!

    private enum projectileT
    {
        damage, noise, hypno
    }
    [SerializeField] private projectileT projectileType;
   
    public void SetStats(SoundFXSO soundFX, Sprite sprite, float speed)
    {
        Rigidbody rb = GetComponent<Rigidbody>(); 
        sr = GetComponentInChildren<SpriteRenderer>();
        sfxVfx = soundFX;
        sr.sprite = sprite;
        rb.linearVelocity = transform.forward * speed;
        // We only want to check for collisions after all of this info is set!
        Collider collider = gameObject.GetComponent<Collider>();
        collider.enabled = true;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
       SoundFXManager.S.PlaySoundFXClip(sfxVfx, gameObject);
       
       // If we dont have a cBM set this will be ignored! Useful for projectiles such as noise makers
       CharacterBattleManager cBMOther = other.gameObject.GetComponent<CharacterBattleManager>();
       switch (projectileType)
       {
           
           case projectileT.damage:
               
               if (characterBattleManager != null && cBMOther != null)
               {
                   cBMOther.ChangeHealth(characterBattleManager.stats.attack - cBMOther.stats.defense);
                   MessageLogManager.S.AddMessageToLog(cBMOther.stats.name + " takes " + 
                                                       (characterBattleManager.stats.attack - cBMOther.stats.defense) + 
                                                       " damage!");
               }
               break;
           case projectileT.hypno:
               if (cBMOther != null)
               {
                   // This is the player's team number!
                   cBMOther.teamNumber = 0;
               }

               break;
       }
       
       TurnManager.S.EndTurn();
       Destroy(gameObject);
    }
}
