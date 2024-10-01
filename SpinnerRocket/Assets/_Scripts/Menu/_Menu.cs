using UnityEngine;
/**
 * @file
 * @brief Clase padre que guarda
 */
public class _Menu : MonoBehaviour
{
    #region Variables
    protected MenuManager menuManager;
    #endregion

    #region Start
    protected virtual void Start()
    {
        menuManager = MenuManager.GetSingleton();
    }
    #endregion
}