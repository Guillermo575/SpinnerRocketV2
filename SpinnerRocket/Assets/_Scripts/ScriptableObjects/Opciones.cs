using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @class
 * @brief Aqui se gestiona los valores de musica y sonido
 */
[CreateAssetMenu(fileName = "Opciones", menuName = "Tools/Opciones", order = 1)]
public class Opciones : ObjetoPersistente
{
    /** Volumen de audio minimo */
    [HideInInspector] public float MinVolume = -60f;
    /** Volumen de audio maximo */
    [HideInInspector] public float MaxVolume = -60f;
    /** Volumen de sonido */
    public float VolumenSonido = 0;
    /** Volumen de musica*/
    public float VolumenMusica = 0;
    /** Ultimo Nivel jugado */
    public string LastLevel = "Scene_0001";
    /** @hidden*/
    public enum dificultad
    {
        facil,
        normal,
        dificil
    }
    /** Cambia el volumen de la musica
      * @param nuevaVolumen: Volumen actual
      */
    public void CambiarVolumenMusica(float nuevaVolumen)
    {
        VolumenMusica = nuevaVolumen;
        AudioManager.GetSingleton().SetBGMVolume(VolumenMusica);
    }
    /** Cambia el volumen del sonido
      * @param nuevaVolumen: Volumen actual
      */
    public void CambiarVolumenSonido(float nuevaVolumen)
    {
        VolumenSonido = nuevaVolumen;
        AudioManager.GetSingleton().SetSFXVolume(VolumenSonido);
    }
}