using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @file
 * @brief Desactiva el objeto en caso de que el proyecto no sea de Android, fue creado para ocultar los botones tactiles
 */
public class ObjectHideNoAndroid : MonoBehaviour
{
    void Start()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            this.gameObject.SetActive(false);
        }
    }
    void Update()
    {
    }
}