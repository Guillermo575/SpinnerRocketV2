using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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