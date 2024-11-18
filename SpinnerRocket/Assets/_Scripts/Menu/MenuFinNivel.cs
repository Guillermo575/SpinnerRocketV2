using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
/**
 * @class
 * @brief Clase que sirve para manejar los metodos de los menus que aparecen al finalizar el nivel (ganar o perder)
 */
public class MenuFinNivel : _Menu
{
    #region Variables
    /** @hidden*/ private GameManager gameManager;
    #endregion

    #region Awake
    /** Inicializacion de los objetos, es una clase override */
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
    /** Te manda al siguiente nivel de acuerdo al index actual, en caso de llegar a su final te manda a la pantalla de titulo */
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
    /** Carga el nivel actual */
    public void ReintentarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /** Carga la pantalla de inicio */
    public void RegresarAPantallaPrincipal()
    {
        MostrarPantallaConfirmar(EventoRegresarAPantallaPrincipal, "Do you want to return to the main menu?");
    }
    /** Evento que se manda a llamar desde el metodo RegresarAPantallaPrincipal */
    private void EventoRegresarAPantallaPrincipal()
    {
        SceneManager.LoadScene(0);
    }
    #endregion

    #region Panel Confirmar
    /** Muestra la ventana de confirmacion*/
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