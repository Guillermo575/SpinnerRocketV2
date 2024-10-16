using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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