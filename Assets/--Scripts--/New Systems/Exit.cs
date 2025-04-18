using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class Exit : MonoBehaviour
{
    
    
    void OnTriggerEnter(Collider other)
    {
        // Get rid of the ground tile below you
        if(other.tag == "Ground") Destroy(other.gameObject);
        
        CharacterMovement c = other.gameObject.GetComponent<CharacterMovement>();
        if (c != null)
        {
            if(c.characterType != characterType.player) return;
            c.OnExit();
            SceneManager.LoadScene("Maze");
            
        }
        
        
    }
}
