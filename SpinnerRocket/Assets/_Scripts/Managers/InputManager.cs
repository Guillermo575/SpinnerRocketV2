using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Windows;
using static InputManager.ActionMap.Action;
/**
 * @class
 * @brief maneja los inputs del proyecto
 */
public class InputManager : MonoBehaviour
{
    #region Singleton
    /** @hidden */
    private static InputManager SingletonGameManager;
    /** @hidden */
    private InputManager()
    {
    }
    /** @hidden */
    private void CreateSingleton()
    {
        if (SingletonGameManager == null)
        {
            SingletonGameManager = this;
        }
        else
        {
            Debug.LogError("Ya existe una instancia de esta clase");
        }
    }
    /** Solo se puede crear un objeto de la clase InputManager, este metodo obtiene el objeto creado */
    public static InputManager GetSingleton()
    {
        return SingletonGameManager;
    }
    #endregion

    #region Variables
    /** Input Principal */
    public InputActionAsset MainInput;
    /** Lista de InputActionAsset disponibles */
    public List<InputActionAsset> lstInputs;
    /** @hidden*/
    private List<ActionMap> lstActionMaps;
    #endregion

    #region Awake & Start & Update
    /** Inicializacion de los objetos */
    void Awake()
    {
        CreateSingleton();
        MainInput = MainInput == null ? lstInputs.First() : MainInput;
        ParseActionMaps();
    }
    /** @hidden*/
    void Start()
    {
    }
    /** @hidden*/
    void Update()
    {
    }
    /** Inicializacion de los objetos */
    private void OnEnable()
    {
        MainInput.Enable();
        TouchSimulation.Enable();
    }
    /** Inicializacion de los objetos */
    private void OnDisable()
    {
        MainInput.Disable();
        TouchSimulation.Disable();
    }
    #endregion

    #region General
    /** Parsea la lista de acciones en un listado */
    public void ParseActionMaps()
    {
        lstActionMaps = new List<ActionMap>();
        foreach (var objActionMaps in MainInput.actionMaps)
        {
            lstActionMaps.Add(new ActionMap
            {
                name = objActionMaps.name,
            });
            var objActionLast = lstActionMaps.Last();
            foreach (var obj in objActionMaps.actions)
            {
                objActionLast.lstActions.Add(
                    new ActionMap.Action
                    {
                        name = obj.name,
                    }
                );
                var objInputLast = objActionLast.lstActions.Last();
                var lstActions = (from x in obj.bindings where x.action == obj.name select x).ToList();
                string CompositeName = "";
                foreach (var objAction in lstActions)
                {
                    if (objAction.isComposite)
                    {
                        CompositeName = objAction.name;
                        objInputLast.lstInputComposite.Add
                        (
                          new ActionMap.Action.ActionInput
                          {
                              name = objAction.name,
                              inputAction = objAction
                          }
                        );
                    }
                    else
                    {
                        var lstTrimAction = objAction.path.Split('/');
                        objInputLast.lstInputAction.Add
                        (
                          new ActionMap.Action.ActionInput
                          {
                              name = objAction.name,
                              composite = CompositeName,
                              inputAction = objAction,
                              OutPutDevice = lstTrimAction[0].Trim(),
                              KeyString = (lstTrimAction.Length > 1) ? lstTrimAction[1].Trim() : lstTrimAction[0].Trim(),
                          }
                        );
                    }
                }
            }
        }
    }
    /**
     * Obtiene la accion pedida
     * @param actionName: Nombre de la accion
     * @return InputAction
     */
    public InputAction GetAction(string actionName)
    {
        return MainInput.FindAction(actionName);
    }
    /**
     * Obtiene la lista de teclas de cada accion
     * @param actionroot: Nombre del mapa de acciones
     * @param actionName: Nombre de la accion
     * @param OutputDevice: Nombre del dispositivo de entrada (Keyboard, gamepad, etc)
     * @param compositeName: Nombre del decorador adicional de la accion
     * @return List<ActionInput>
     */
    public List<ActionInput> GetActionKeys(string actionroot, string actionName, string OutputDevice = "", string compositeName = "")
    {
        var objActionMaps = (from x in lstActionMaps where x.name == actionroot select x).First();
        var objActions = (from x in objActionMaps.lstActions where x.name == actionName select x).First();
        var objInputBind = (from x in objActions.lstInputAction where x.OutPutDevice == OutputDevice || string.IsNullOrEmpty(OutputDevice) select x).ToList();
        var obj = (from x in objActions.lstInputAction where x.composite == compositeName || string.IsNullOrEmpty(compositeName) select x).ToList();
        return obj;
    }
    #endregion

    #region ActionMap
    /** Clase que parsea los objetos de un inputaction con tal de que se pueda obtener un arbol de clases */
    public class ActionMap
    {
        public string name;
        public List<Action> lstActions;
        public ActionMap()
        {
            lstActions = new List<Action>();
        }
        /** @hidden*/
        public class Action
        {
            public string name;
            public List<ActionInput> lstInputAction;
            public List<ActionInput> lstInputComposite;
            public Action()
            {
                lstInputAction = new List<ActionInput>();
                lstInputComposite = new List<ActionInput>();
            }
            /** @hidden*/
            public class ActionInput
            {
                public string name;
                public string composite;
                public string OutPutDevice;
                public string KeyString;
                public InputBinding inputAction;
            }
        }
    }
    #endregion
}