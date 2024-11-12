using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @file
 * @brief Clase que muestra alguna de la ventana de atajos de botones y la ventana mostrada dependera de que control se este usando (teclado o gamepad)
 */
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