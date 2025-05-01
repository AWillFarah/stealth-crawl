using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// For Demo use ONLY
/// </summary>
public class DemoReturnToMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        // Get rid of the ground tile below you
        if(other.tag == "Ground") Destroy(other.gameObject);
        CharacterMovement c = other.gameObject.GetComponent<CharacterMovement>();
        if (c != null)
        {
            SceneManager.LoadScene("DungeonPicker");
        }
    }
}
