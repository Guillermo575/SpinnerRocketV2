using UnityEngine;
public class ObjectInvisibleAtStart : MonoBehaviour
{
    [HideInInspector] public new Renderer renderer;
    void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.enabled = false;
    }
}