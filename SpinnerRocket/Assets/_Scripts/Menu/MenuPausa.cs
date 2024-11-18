using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
/**
 * @class
 * @brief Menu que se abre al pausar el juego
 */
public class MenuPausa : _Menu
{
    #region Variables
    /** @hidden*/ private GameManager gameManager;
    #endregion

    #region Controles
    /** Objeto del input manager */
    private InputManager inputManager;
    /** InputAction que gestionara la pausa */
    private InputAction inputPause;
    #endregion

    #region Awake & Update
    /** Inicializacion de los objetos*/
    protected override void Start()
    {
        base.Start();
        gameManager = GameManager.GetSingleton();
        inputManager = InputManager.GetSingleton();
        gameManager.OnGamePause += delegate { MostrarMenuPausa(); } ;
        gameManager.OnGameResume += delegate { OcultarMenuPausa(); };
        inputPause = inputManager.GetAction("Pause");
        inputPause.performed += PauseAction;
    }
    #endregion

    #region Mostrar/Ocultar menu
    /** Muestra el menu de pausa */
    private void MostrarMenuPausa()
    {
        menuManager.ShowMenu(menuManager.GetMenu("CanvasMenuPausa"));
    }
    /** Oculta el menu de pausa */
    private void OcultarMenuPausa()
    {
        menuManager.DeleteMenuTree();
    }
    #endregion

    #region Botones Interfaz
    /** Muestra la pantalla de confirmacion para regresar a la pantalla principal*/
    public void RegresarAPantallaPrincipal()
    {
        MostrarPantallaConfirmar(EventoRegresarAPantallaPrincipal, "Do you want to return to the main menu?");
    }
    /** Evento que regresa a la pantalla principal */
    private void EventoRegresarAPantallaPrincipal()
    {
        SceneManager.LoadScene(0);
    }
    /** Muestra la pantalla de confirmacion para reiniciar de nivel */
    public void ReintentarNivel()
    {
        MostrarPantallaConfirmar(EventoReintentarNivel, "Do you want to restart the level?");
    }
    /** Evento que de reinicio de nivel */
    public void EventoReintentarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /** Muestra la pantalla de opciones */
    public void MostrarMenuOpciones()
    {
        menuManager.ShowMenu(menuManager.GetMenu("CanvasMenuOpciones"));
    }
    /** Reanuda el juego */
    public void ResumeGame()
    {
        gameManager.ResumeGame();
    }
    /** Regresa al menu anterior */
    public void BackMenu()
    {
        menuManager.BackMenu();
    }
    #endregion

    #region Panel Confirmar
    /** Metodo para mostrar la pantalla para confirmar */
    private void MostrarPantallaConfirmar(UnityAction evt, String msg)
    {
        var menuConfirmar = menuManager.menuConfirmar;
        if (menuConfirmar != null)
        {
            UnityEvent objEvent = new UnityEvent();
            objEvent.AddListener(evt);
            menuConfirmar.OpenWindow(objEvent, msg);
        }
        else
        {
            evt();
        }
    }
    #endregion

    #region InputAction
    /** Metodo de inputaction cuando oprimes el boton de pausa */
    private void PauseAction(InputAction.CallbackContext obj)
    {
        if (!gameManager.IsGameEnd)
        {
            if (gameManager.IsGamePause)
            {
                gameManager.ResumeGame();
            }
            else
            {
                gameManager.PauseGame();
            }
        }
    }
    #endregion
}