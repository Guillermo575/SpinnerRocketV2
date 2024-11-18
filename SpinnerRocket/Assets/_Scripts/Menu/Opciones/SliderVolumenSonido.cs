using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/**
 * @class
 * @brief Interfaz que cambia el volumen de los efectos de sonidos
 */
public class SliderVolumenSonido : MonoBehaviour
{
    #region Variables
    /** @hidden*/ private MenuManager menuManager;
    /** @hidden*/ private Opciones opciones;
    /** Objeto slider que usara la clase */
    Slider slider;
    #endregion

    #region Awake & Start
    /** Metodo de inicio para crear el singleton */
    public void Start()
    {
        menuManager = MenuManager.GetSingleton();
        opciones = menuManager.opciones;
        slider = this.GetComponent<Slider>();
        slider.value = (int)opciones.VolumenSonido;
        slider.onValueChanged.AddListener(delegate { ControlarCambios(); });
    }
    #endregion

    #region General
    /** Metodo de que responde al cambio de sonido */
    public void ControlarCambios()
    {
        opciones.CambiarVolumenSonido(slider.value);
    }
    #endregion
}