using UnityEngine;

/// <summary>
/// Projectiles will all behave fairly similar, so we can handle it all through a script like this
/// </summary>
public class Projectile : MonoBehaviour
{
    private SoundFXSO sfxVfx;
    private Rigidbody rb;
    private SpriteRenderer sr;
    
    
    void Awake()
    {
       
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
       Destroy(this.gameObject);
    }
}
