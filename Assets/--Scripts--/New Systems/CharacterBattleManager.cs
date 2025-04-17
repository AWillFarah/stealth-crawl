using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattleManager : MonoBehaviour
{
    [Header("Inscribed")]
    public StatSO stats;
    public int teamNumber = 0;
    
    [Header("Dynamic")]
    private int maxHealth = 1;
    private int currentHealth;

    [SerializeField] private SoundFXSO attackSFX;
    
    void Start()
    {
        if (stats != null) currentHealth = maxHealth = stats.health;
        else Debug.LogError("Error! Stats are not set for: " + gameObject.name);
    }
    
    public void Attack(Vector3 direction)
    {
        SoundFXManager.S.PlaySoundFXClip(attackSFX, gameObject);
        if(Physics.Raycast(transform.position, (transform.forward), out RaycastHit hit, 1.41f) )
        {
            CharacterBattleManager cBM = hit.collider.gameObject.GetComponent<CharacterBattleManager>();
            if(cBM != null) cBM.currentHealth -= stats.attack;
        }
        
        Debug.DrawRay(transform.position, direction, Color.yellow, 2f);
        TurnManager.S.Invoke("EndTurn", 0.1f);
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
