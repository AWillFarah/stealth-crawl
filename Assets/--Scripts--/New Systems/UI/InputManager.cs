using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputSystem_Actions INPUTACTIONS;
    public static event Action<InputActionMap> ACTIONMAPCHANGE;
    
    void Start()
    {
        INPUTACTIONS = new InputSystem_Actions();
        TOGGLEACTIONMAP(INPUTACTIONS.Player);
    }

    // Update is called once per frame
    public static void TOGGLEACTIONMAP(InputActionMap actionMap)
    {
        if(actionMap.enabled) return;
        
        INPUTACTIONS.Disable();
        ACTIONMAPCHANGE?.Invoke(actionMap);
        actionMap.Enable();
    }
}
