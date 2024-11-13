using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @class
 * @brief Desactiva el objeto en caso de que el proyecto no sea de Android, fue creado para ocultar los botones tactiles
 */
public class ObjectHideNoAndroid : MonoBehaviour
{
    #region Start
    /** Desactiva los objetos al inicio en caso de que el proyecto no este ejecutadp para Android */
    void Start()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            this.gameObject.SetActive(false);
        }
    }
    #endregion
}