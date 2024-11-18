using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/**
 * @class
 * @brief Clase que muestra los botones de cada accion y los coloca en una ventana
 */
public class InputWindowKeyBoard : MonoBehaviour
{
    /** @hidden*/ private InputManager inputManager;
    /** Texto que muestra el boton para la accion de propulsion */
    public TMP_Text txtLaunch;
    /** Texto que muestra el boton para la accion de cambio de rotacion */
    public TMP_Text txtRotate;
    /** Inicializacion de los objetos */
    void Start()
    {
        inputManager = InputManager.GetSingleton();
        var objLaunch = inputManager.GetActionKeys("Player", "Launch", "<Keyboard>");
        txtLaunch.text = objLaunch[0].KeyString.ToUpper();
        var objRotate = inputManager.GetActionKeys("Player", "Rotate", "<Keyboard>");
        txtRotate.text = objRotate[0].KeyString.ToUpper();
    }
}