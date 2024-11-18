using UnityEngine;
/**
 * @class
 * @brief Reloj que cuenta los segundos transcurridos
 */
public class GameTimeWatch : MonoBehaviour
{
    #region Singleton
    /** @hidden*/
    private static GameTimeWatch SingletonGameManager;
    /** @hidden*/
    private GameTimeWatch()
    {
    }
    /** Aqui se crea el objeto singleton */
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
    /** Solo se puede crear un objeto de la clase GameTimeWatch, este metodo obtiene el objeto creado */
    public static GameTimeWatch GetSingleton()
    {
        return SingletonGameManager;
    }
    #endregion

    #region Variables
    /** @hidden*/
    private float _currentTime = 0.0f;
    /** Indica si el reloj ha iniciado */
    private bool Started = false;
    /** @hidden*/
    private MenuManager menuManager;
    /** Obtiene el tiempo actual del reloj */
    public float currentTime { get { return _currentTime; } }
    #endregion

    #region Start & Update
    /** Crea el singleton de la clase*/
    void Awake()
    {
        CreateSingleton();
    }
    /** Inicializacion de los objetos */
    public void Start()
    {
        menuManager = MenuManager.GetSingleton();
    }
    /** Agrega el tiempo transcurrido al reloj */
    public void Update()
    {
        if (Started)
        {
            _currentTime += 1 * Time.deltaTime;
        }
    }
    #endregion

    #region Start & Stop timer
    /** Inicia el reloj */
    public void StartTimer()
    {
        _currentTime = 0.0f;
        Started = true;
    }
    /** Detiene el reloj*/
    public void StopTimer()
    {
        Started = false;
    }
    #endregion

    #region Get & Set
    /** Devuelve el tiempo transcurrido del reloj en formato 0:00 como string */
    public string GetCurrentTimer()
    {
        var record = _currentTime;
        string ret = string.Empty;
        if (record >= 0)
        {
            ret = Mathf.Round(record / 60) + ":" + ((int)Mathf.Round(record % 60)).ToString("00");
        }
        return ret;
    }
    #endregion
}