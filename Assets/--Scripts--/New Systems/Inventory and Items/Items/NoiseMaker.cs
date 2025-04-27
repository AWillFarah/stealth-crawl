using UnityEngine;

public class NoiseMaker : TemplateItem
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private SoundFXSO projectileSFXVFX;
    public SpriteRenderer spriteRenderer;
  
    
    public override void Use()
    {
        Transform spawnTransform = CharacterBattleManager.PLAYER.gameObject.transform;
        Vector3 spawnPos = spawnTransform.position + spawnTransform.forward;
        GameObject proj = Instantiate(projectile, spawnPos, spawnTransform.rotation);
        Projectile p = proj.GetComponent<Projectile>();
        if(p != null) p.SetStats(projectileSFXVFX, spriteRenderer.sprite, 25f);
        TurnManager.S.EndTurn();
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
