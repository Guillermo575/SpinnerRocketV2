using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
/**
 * @class
 * @brief Menu que se abre al iniciar la aplicacion
 */
public class MenuPrincipal : _Menu
{
    #region Awake
    /** Inicializacion de los objetos */
    protected override void Start()
    {
        base.Start();
        Time.timeScale = 1;
        if (btnContinue != null)
        {
            var lastLevel = menuManager.opciones.LastLevel;
            btnContinue.SetActive(!string.IsNullOrEmpty(lastLevel) && lastLevel != "Scene_0001");
        }
    }
    #endregion

    #region Interfaz
    /** Boton de continuacion que se activa o desactiva si el ultimo nivel jugado no es el Scene_0*/
    public GameObject btnContinue;
    /** Carga el primer nivel del juego (index 1) */
    public void IniciarJuego()
    {
        SceneManager.LoadScene(1);
    }
    /** Recarga la escena actual */
    public void ContinuarJuego()
    {
        SceneManager.LoadScene(menuManager.opciones.LastLevel);
    }
    /** Regresa la primera pantalla del juego (index 0) */
    public void BackToTitle()
    {
        SceneManager.LoadScene(0);
    }
    /** Carga el menu de opciones */
    public void MostrarOpciones()
    {
        menuManager.ShowMenu(menuManager.GetMenu("CanvasMenuOpciones"));
    }
    /** Cierra el juego */
    public void FinalizarJuego()
    {
        Application.Quit();
    }
    /** Regresa al menu anterior */
    public void BackMenu()
    {
        menuManager.BackMenu();
    }
    /** Muestra el menu de creditos */
    public void MostrarCreditos()
    {
        menuManager.ShowMenu(menuManager.GetMenu("CanvasMenuCreditos"));
    }
    #endregion
}