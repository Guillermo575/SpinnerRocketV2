using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
 * @file
 * @brief Clase que maneja el desvanecimiento de un objeto, sirve para hacer transiciones entre escenario
 */
public class SceneLoadManager : MonoBehaviour
{
    #region Variables
    /** Imagen que se usará para el desvanecimiento */
    public Image fadeImage;
    /** Duración del desvanecimiento */
    public float fadeDuration = 0.8f;
    #endregion

    #region Start
    /** Inicializacion de los objetos */
    void Start()
    {
        StartCoroutine(FadeOut());
    }
    #endregion

    #region General
    /** Corutina que desvanece el objeto */
    private IEnumerator FadeOut()
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime / fadeDuration;
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
    }
    #endregion
}