using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @class
 * @brief Clase que muestra alguna de la ventana de atajos de botones y la ventana mostrada dependera de que control se este usando (teclado o gamepad)
 */
public class InputWindow : MonoBehaviour
{
    #region Variables
    /** Objeto que representa la ventana de teclado */
    public GameObject WindowKeyBoard;
    /** Objeto que representa la ventana de gamepad */
    public GameObject WindowGamePad;
    #endregion

    #region Start & Update
    /** Inicializacion de los objetos */
    void Start()
    {
        EnableControllers();
    }
    /** Metodo de actualizacion de objetos */
    void Update()
    {
        EnableControllers();
    }
    #endregion

    #region General
    /** Inactiva o desactiva las ventanas de acuerdo a los controles conectados */
    private void EnableControllers()
    {
        var controllers = Input.GetJoystickNames();
        WindowKeyBoard.SetActive(controllers.Length == 0);
        WindowGamePad.SetActive(controllers.Length > 0);
    }
    #endregion
}