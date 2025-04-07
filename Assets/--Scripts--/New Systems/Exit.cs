using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class Exit : MonoBehaviour
{
    
    
    void OnTriggerEnter(Collider other)
    {
        CharacterMovement c = other.gameObject.GetComponent<CharacterMovement>();
        print("Exit");
        if (c != null)
        {
            if(c.characterType != characterType.player) return;
            print("Exit");
            SceneManager.LoadScene("Maze");
            InputSystem_Actions.Player.Disable();
        }
    }
}
