using UnityEngine;
/**
 * @file
 * @brief Clase padre que guarda las caracteristicas principales del menu
 */
public class _Menu : MonoBehaviour
{
    #region Variables
    internal MenuManager menuManager;
    #endregion

    #region Start
    protected virtual void Start()
    {
        menuManager = MenuManager.GetSingleton();
    }
    #endregion
}