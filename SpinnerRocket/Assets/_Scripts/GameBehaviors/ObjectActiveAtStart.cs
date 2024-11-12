using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @file
 * @brief Hace que los objetos hijos de quien tengan esta clase se activen al inicio, fue hecho para que el objeto de transicion solo sea visible al abrir el juego
 */
public class ObjectActiveAtStart : MonoBehaviour
{
    void Awake()
    {
        for (int l = 0; l < this.transform.childCount; l++)
        {
            var obj = this.transform.GetChild(l);
            obj.gameObject.SetActive(true);
        }
    }
}