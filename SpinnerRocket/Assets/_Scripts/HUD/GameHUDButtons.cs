using UnityEngine;
using UnityEngine.InputSystem;
public class GameHUDButtons : MonoBehaviour
{
    InputManager inputManager;
    InputAction inputLaunch;
    InputAction inputRotate;
    void Start()
    {
        inputManager = InputManager.GetSingleton();
        inputLaunch = inputManager.GetAction("Launch");
        inputRotate = inputManager.GetAction("Rotate");
    }
    public void PressLaunchButton()
    {
        Debug.Log("PressLaunchButton");
    }
    public void ReleaseLaunchButton()
    {
        Debug.Log("ReleaseLaunchButton");
    }
    public void PressRotateButton()
    {
        Debug.Log("PressRotateButton");
    }
}