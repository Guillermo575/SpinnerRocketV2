using UnityEngine;
/**
 * @file
 * @brief Oculta el render de los objetos que tengan esta clase
 */
public class ObjectInvisibleAtStart : MonoBehaviour
{
    [HideInInspector] public new Renderer renderer;
    void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.enabled = false;
    }
}