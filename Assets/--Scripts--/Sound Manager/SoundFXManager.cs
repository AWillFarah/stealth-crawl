using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager S;

    [SerializeField] private AudioSource soundFXObject;
    
    public SoundFXSO soundFXSO;
    [SerializeField] Collider[] colliders = new Collider[50];
    [SerializeField] LayerMask soundLayer;
    private int count;
    public List<GameObject> objects = new List<GameObject>();
    
    private void Awake()
    {
        if (S == null)
        {
            S = this;
        }
    }

    public void PlaySoundFXClip(SoundFXSO audioClip, GameObject spawnGO)
    {
        //spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, new Vector3(spawnGO.transform.position.x, 0.5f, spawnGO.transform.position.z), 
            Quaternion.identity);
        
        
        //assign the audioClip              
        audioSource.clip = audioClip.sound; 
        
        //get length of sound FX clip                
        float clipLength = audioSource.clip.length;  
        
        if (audioClip.vfx != null)
        {
            ParticleSystem vfx = Instantiate(audioClip.vfx, spawnGO.transform.position + transform.TransformDirection(Vector3.forward),
                Quaternion.identity);
            Destroy(vfx.gameObject, clipLength);
        }
        
        
       
       

        //assign volume
        audioSource.volume = audioClip.volume;
        audioSource.pitch = Random.Range(audioClip.pitchMin, audioClip.pitchMax);

       
        //play sound
        audioSource.Play();
        
        // Creation of the noise sphere
        count = (Physics.OverlapSphereNonAlloc(audioSource.transform.position, audioClip.noiseRadius, colliders,
            soundLayer, QueryTriggerInteraction.Collide));
        
        CharacterBattleManager cBM1 = spawnGO.GetComponentInParent<CharacterBattleManager>();
        objects.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = colliders[i].gameObject;
            if (obj != spawnGO)
            {
                objects.Add(obj);  
                CharacterBattleManager cBM2 = obj.GetComponentInParent<CharacterBattleManager>();
                
                // We dont want team members hearing each other
                if(cBM1.teamNumber != cBM2.teamNumber)
                {
                    State npcState = obj.GetComponentInParent<StateManager>().currentState;
                    npcState.heardNoise(audioSource.transform.position);
                }
                
            }
            
        }
        
        
        

        //destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);
        
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume, bool changePitch)
    {
        //asign a random index
        int rand = Random.Range(0, audioClip.Length);
    
        //spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assign the audioClip
        if (rand > audioClip.Length || rand == 0) return;
        audioSource.clip = audioClip[rand];

        //assign volume
        audioSource.volume = volume;

        //assign pitch if set to change
        if(changePitch == true)
        {
            
        }
        else
        {
          audioSource.pitch = 1;
        }

        //play sound
        audioSource.Play();

        //get length of sound FX clip
        float clipLength = audioSource.clip.length;

        //destriy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);
    }
}
