using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattleManager : MonoBehaviour
{
    [Header("Inscribed")]
    public StatSO stats;
    public int teamNumber = 0;
    
    [Header("Dynamic")]
    private int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        if (stats != null) currentHealth = maxHealth = stats.health;
    }
    
    public void Attack()
    {
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 1.41f) )
        {
            print("attacking");
            CharacterBattleManager cBM = hit.collider.gameObject.GetComponent<CharacterBattleManager>();
            cBM.currentHealth -= 10;
        }
        
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.yellow, 2f);
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
