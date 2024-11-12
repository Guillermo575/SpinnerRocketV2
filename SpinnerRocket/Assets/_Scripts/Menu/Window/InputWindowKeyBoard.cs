using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/**
 * @file
 * @brief Clase que muestra los botones de cada accion y los coloca en una ventana
 */
public class InputWindowKeyBoard : MonoBehaviour
{
    private InputManager inputManager;
    public TMP_Text txtLaunch;
    public TMP_Text txtRotate;
    void Start()
    {
        inputManager = InputManager.GetSingleton();
        var objLaunch = inputManager.GetActionKeys("Player", "Launch", "<Keyboard>");
        txtLaunch.text = objLaunch[0].KeyString.ToUpper();
        var objRotate = inputManager.GetActionKeys("Player", "Rotate", "<Keyboard>");
        txtRotate.text = objRotate[0].KeyString.ToUpper();
    }
}