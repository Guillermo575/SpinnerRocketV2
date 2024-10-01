using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
/**
 * @file
 * @brief Archivo principal que maneja los menus del juego
 */
public class MenuManager : MonoBehaviour
{
    #region Singleton
    private static MenuManager SingletonMenuManager;
    /** @hidden */
    private MenuManager()
    {
    }
    /** @hidden */
    private void CreateSingleton()
    {
        if (SingletonMenuManager == null)
        {
            SingletonMenuManager = this;
        }
        else
        {
            Debug.LogError("Ya existe una instancia de esta clase");
        }
    }
    /** Solo se puede crear un objeto de la clase MenuManager, este metodo obtiene el objeto creado */
    public static MenuManager GetSingleton()
    {
        return SingletonMenuManager;
    }
    #endregion

    #region Variables
    /**
     * Para navegar entre menus cada vez que se abre uno nueva se agrega a esta lista y en caso de salir
     * y regresar al anterior se elimina el mas reciente para desplegar el anterior
     */
    [HideInInspector] public List<GameObject> lstMenuTree;
    public List<GameObject> lstMenus;
    public Opciones opciones;
    public MenuConfirmar menuConfirmar;
    #endregion

    #region Start
    /** @hidden */
    private void Awake()
    {
        CreateSingleton();
    }
    /** @hidden */
    void Start()
    {
        lstMenuTree = new List<GameObject>();
        var lstActivos = (from x in lstMenus where x.gameObject.activeSelf select x).ToList();
        if (lstActivos.Count > 0) lstMenuTree.Add(lstActivos.First().gameObject);
    }
    /** @hidden */
    void Update()
    {
    }
    #endregion

    #region Menus
    /** Elimina el menu actual y despliega el anterior de este */
    public void BackMenu()
    {
        if (lstMenuTree.Count > 1)
        {
            var objBack = lstMenuTree[lstMenuTree.Count - 2];
            var objActual = lstMenuTree.Last();
            lstMenuTree.Remove(objActual);
            objActual.SetActive(false);
            objBack.SetActive(true);
        }
    }

    /**
     * Desactiva el menu actual y despliega el mostrado \n
     * @param objMenu: objeto que se mostrara como menu
     */
    public void ShowMenu(GameObject objMenu)
    {
        if (objMenu != null)
        {
            SetActiveCanvas();
            lstMenuTree.Add(objMenu);
            objMenu.SetActive(true);
        }
    }

    /**
     * Activa/Desactiva todos los menus activos \n
     * @param value: valor para activar los menus
     */
    public void SetActiveCanvas(bool value = false)
    {
        var lst = lstMenuTree;
        foreach (var x in lst)
        {
            if (x.gameObject != null && x.gameObject.activeSelf)
            {
                x.gameObject.SetActive(value);
            }
        }
    }

    /** Desactiva los menus activos y borra la lista de menus actual */
    public void DeleteMenuTree()
    {
        SetActiveCanvas(false);
        lstMenuTree = new List<GameObject>();
    }
    #endregion

    #region General
    public GameObject GetMenu(string name)
    {
        var lst = (from x in lstMenus where x.name.ToUpper() == name.ToUpper() select x).ToList();
        return lst.Count > 0 ? lst.First().gameObject : null;
    }
    #endregion
}