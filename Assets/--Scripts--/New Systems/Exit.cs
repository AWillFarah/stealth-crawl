using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


/// <summary>
/// This will handle whether we load another floor or if we have reached the bottom/top of the dungeon
/// </summary>
public class Exit : MonoBehaviour
{

    public static int floorsRemaining = 1;
    void OnTriggerEnter(Collider other)
    {
        // Get rid of the ground tile below you
        if(other.tag == "Ground") Destroy(other.gameObject);
        
        CharacterMovement c = other.gameObject.GetComponent<CharacterMovement>();
        if (c != null)
        {
            if(c.characterType != characterType.player) return;
            c.OnExit();
            
            // We need to check if there is another floor after us
            floorsRemaining--;
            if (floorsRemaining < 1)
            {
                switch (MazeGeneratorWithRooms.S.dungeon.endFloor)
                {
                    case(EndFloor.treasureRoom):
                    {
                        SceneManager.LoadScene("TreasureRoom");
                        break;
                    }
                    case (EndFloor.bossRoom):
                    {
                        SceneManager.LoadScene("BossRoom");
                        break;
                    }
                }
                
            }
            else
            {
                if(Camera.main != null) Camera.main.gameObject.SetActive(false);
                SceneManager.LoadScene("Maze");
            }
            
            
        }
        
        
    }
}
