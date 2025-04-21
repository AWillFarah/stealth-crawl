using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Inscribed")]
    public static UIManager S;
    public GameObject pauseMenuUI;
    
    [Header("Dynamic")]
    private bool pauseEnabled = false;
    
    void Start()
    {
        if(S != null) Debug.LogError("PauseMenuUIManager already exists!");
        S = this;
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        bool cancel = InputManager.INPUTACTIONS.UI.Cancel.ReadValue<float>() > 0;
        if (cancel)
        {
            TogglePauseMenu(); 
        }
    }
    
    // Update is called once per frame
    public void TogglePauseMenu()
    {
        pauseEnabled = !pauseEnabled;
        // If we are disabling the pause menu then turn the player controls on
        if(!pauseEnabled) InputManager.TOGGLEACTIONMAP(InputManager.INPUTACTIONS.Player);
        pauseMenuUI.SetActive(pauseEnabled);
    }
}
