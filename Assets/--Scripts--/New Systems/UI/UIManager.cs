using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Inscribed")]
    public static UIManager S;
    // We will need to access the player's stats and only theirs
    
    public GameObject pauseMenuUI;
    public TMP_Text healthText;
    public Image healthBar;
    
    
    [Header("Dynamic")]
    private bool pauseEnabled;
    
    void Start()
    {
        if(S != null) Debug.LogError("PauseMenuUIManager already exists!");
        S = this;
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        // This is used for unpausing the game. Since it's turn based we don't need to worry about pausing any objects
        // or anything
        bool cancel = InputManager.INPUTACTIONS.UI.Cancel.ReadValue<float>() > 0;
        if (cancel)
        {
            TogglePauseMenu(); 
        }
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
      healthText.text = currentHealth.ToString();
      healthBar.fillAmount = (float)currentHealth / maxHealth;
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
