using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/**
 * @file
 * @brief Interfaz que cambia el volumen de la musica de fondo
 */
public class SliderVolumenMusica : MonoBehaviour
{
    #region Variables
    /** @hidden */ private MenuManager menuManager;
    /** @hidden */ private Opciones opciones;
    /** Objeto slider que usara la clase */
    Slider slider;
    #endregion

    #region Awake & Start
    /** Metodo de inicio del singleton */
    public void Start()
    {
        menuManager = MenuManager.GetSingleton();
        opciones = menuManager.opciones;
        slider = this.GetComponent<Slider>();
        slider.value = (int)opciones.VolumenMusica;
        slider.onValueChanged.AddListener(delegate { ControlarCambios(); });
    }
    #endregion

    #region General
    /** Metodo de que responde al cambio de musica */
    public void ControlarCambios()
    {
        opciones.CambiarVolumenMusica(slider.value);
    }
    #endregion
}