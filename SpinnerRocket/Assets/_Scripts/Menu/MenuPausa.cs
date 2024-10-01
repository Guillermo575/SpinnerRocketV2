using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
/**
 * @file
 * @brief Menu que se abre al pausar el juego
 */
public class MenuPausa : _Menu
{
    #region Variables
    private GameManager gameManager;
    #endregion

    #region Awake & Update
    protected override void Start()
    {
        base.Start();
        gameManager = GameManager.GetSingleton();
        gameManager.OnGamePause += delegate { MostrarMenuPausa(); } ;
        gameManager.OnGameResume += delegate { OcultarMenuPausa(); };
    }
    private void Update()
    {
        if (!gameManager.IsGameEnd)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
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
    }
    #endregion

    #region Mostrar/Ocultar menu
    private void MostrarMenuPausa()
    {
        menuManager.ShowMenu(menuManager.GetMenu("CanvasMenuPausa"));
    }
    private void OcultarMenuPausa()
    {
        menuManager.DeleteMenuTree();
    }
    #endregion

    #region Botones Interfaz
    public void RegresarAPantallaPrincipal()
    {
        MostrarPantallaConfirmar(EventoRegresarAPantallaPrincipal, "Do you want to return to the main menu?");
    }
    private void EventoRegresarAPantallaPrincipal()
    {
        SceneManager.LoadScene(0);
    }
    public void ReintentarNivel()
    {
        MostrarPantallaConfirmar(EventoReintentarNivel, "Do you want to restart the level?");
    }
    public void EventoReintentarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MostrarMenuOpciones()
    {
        menuManager.ShowMenu(menuManager.GetMenu("CanvasMenuOpciones"));
    }
    public void ResumeGame()
    {
        gameManager.ResumeGame();
    }
    public void BackMenu()
    {
        menuManager.BackMenu();
    }
    #endregion

    #region Panel Confirmar
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
}