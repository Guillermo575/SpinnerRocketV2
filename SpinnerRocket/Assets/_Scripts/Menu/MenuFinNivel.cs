using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
/**
 * @file
 * @brief Clase que sirve para manejar los metodos de los menus que aparecen al finalizar el nivel (ganar o perder)
 */
public class MenuFinNivel : _Menu
{
    #region Variables
    private GameManager gameManager;
    #endregion

    #region Awake
    protected override void Start()
    {
        base.Start();
        if (menuManager != null)
        {
            gameManager = GameManager.GetSingleton();
            if (gameManager != null)
            {
                gameManager.OnGameLevelCleared += delegate { menuManager.ShowMenu(menuManager.GetMenu("CanvasMenuFinNivel")); };
                gameManager.OnGameOver += delegate { menuManager.ShowMenu(menuManager.GetMenu("CanvasMenuFinJuego")); };
            }
        }
    }
    #endregion

    #region Eventos
    public void SiguienteNivel()
    {
        var siguienteNivel = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > siguienteNivel)
        {
            SceneManager.LoadScene(siguienteNivel);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
    public void ReintentarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void RegresarAPantallaPrincipal()
    {
        MostrarPantallaConfirmar(EventoRegresarAPantallaPrincipal, "Do you want to return to the main menu?");
    }
    private void EventoRegresarAPantallaPrincipal()
    {
        SceneManager.LoadScene(0);
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