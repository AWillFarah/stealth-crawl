using UnityEngine;

public class ProjectileItem : TemplateItem
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private SoundFXSO projectileSFXVFX;
    
  
    
    public override void Use()
    {
        Transform spawnTransform = CharacterBattleManager.PLAYER.gameObject.transform;
        Vector3 spawnPos = spawnTransform.position + spawnTransform.forward;
        GameObject proj = Instantiate(projectile, spawnPos, spawnTransform.rotation);
        Projectile p = proj.GetComponent<Projectile>();
        if(p != null) p.SetStats(projectileSFXVFX, spriteRenderer.sprite, 25f);
        DisableUI();
        // NOTE! End turn is called in the projectile and not in here! This is to avoid having enemies
        // Move while you fire
        Destroy(gameObject);
    }
    
}
