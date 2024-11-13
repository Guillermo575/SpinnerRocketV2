using UnityEngine;
/**
 * @class
 * @brief Oculta el render de los objetos que tengan esta clase
 */
public class ObjectInvisibleAtStart : MonoBehaviour
{
    #region Variables
    [HideInInspector] public new Renderer renderer;
    #endregion

    #region Start
    /** Oculta el objeto al inicio del escenario */
    void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.enabled = false;
    }
    #endregion
}