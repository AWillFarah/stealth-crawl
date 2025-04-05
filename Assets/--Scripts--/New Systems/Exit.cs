using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Exit : MonoBehaviour
{
    
    void Start()
    {
        
    }
    
    void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.name == "Player")
        {
            print("Exit");
            SceneManager.LoadScene("Maze");
        }
    }
}
