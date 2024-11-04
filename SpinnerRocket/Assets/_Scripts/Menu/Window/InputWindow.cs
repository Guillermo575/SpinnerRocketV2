using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InputWindow : MonoBehaviour
{
    public GameObject WindowKeyBoard;
    public GameObject WindowGamePad;
    void Start()
    {
        EnableControllers();
    }
    void Update()
    {
        EnableControllers();
    }
    private void EnableControllers()
    {
        var controllers = Input.GetJoystickNames();
        WindowKeyBoard.SetActive(controllers.Length == 0);
        WindowGamePad.SetActive(controllers.Length > 0);
    }
}