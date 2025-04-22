using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Handles combat and taking damage
/// </summary>
public class CharacterBattleManager : MonoBehaviour
{
    [Header("Inscribed")]
    public StatSO stats;
    public int teamNumber = 0;
    [HideInInspector] public bool isPlayer;
    public static CharacterBattleManager PLAYER;
    
    [Header("Dynamic")]
    private int maxHealth;
    private int currentHealth;
    

    [SerializeField] private SoundFXSO attackSFX;
    
    void Start()
    {
        if (stats != null) currentHealth = maxHealth = stats.health;
        else Debug.LogError("Error! Stats are not set for: " + gameObject.name);
        if (isPlayer)
        {
            UIManager.S.UpdateHealth(currentHealth, maxHealth);
            if (PLAYER != null)
            {
                Debug.Log("PLAYER already exists!");
                return;
            }
            PLAYER = this;
        }
    }
    
    
    
    public void Attack(Vector3 direction)
    {
        
        if(Physics.Raycast(transform.position, (transform.forward), out RaycastHit hit, 1.41f) )
        {
            SoundFXManager.S.PlaySoundFXClip(attackSFX, gameObject);
            CharacterBattleManager cBM = hit.collider.gameObject.GetComponent<CharacterBattleManager>();
            if(cBM != null) cBM.ChangeHealth(stats.attack - cBM.stats.defense);
        }
        
        Debug.DrawRay(transform.position, direction, Color.yellow, 2f);
        TurnManager.S.Invoke("EndTurn", 0.1f);
    }

    public void ChangeHealth(int damage, bool takingDamage = true)
    {
        if(damage <= 0 && takingDamage) damage = 1;
        currentHealth -= damage;
        if(isPlayer) UIManager.S.UpdateHealth(currentHealth, maxHealth);
    }

    
    
    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
