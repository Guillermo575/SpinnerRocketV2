using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @class
 * @brief Desactiva el objeto en caso de que el proyecto sea de Android
 */
public class ObjectHideAndroid : MonoBehaviour
{
    #region Start
    /** Desactiva los objetos al inicio en caso de que el proyecto este ejecutadp para Android */
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            this.gameObject.SetActive(false);
        }
    }
    #endregion
}