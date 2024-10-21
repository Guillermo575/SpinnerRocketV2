using UnityEngine;
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
    private float _currentTime = 0.0f;
    private bool Started = false;
    private MenuManager menuManager;
    public float currentTime { get { return _currentTime; } }
    #endregion

    #region Start & Update
    void Awake()
    {
        CreateSingleton();
    }
    public void Start()
    {
        menuManager = MenuManager.GetSingleton();
    }
    public void Update()
    {
        if (Started)
        {
            _currentTime += 1 * Time.deltaTime;
        }
    }
    #endregion

    #region Start & Stop timer
    public void StartTimer()
    {
        _currentTime = 0.0f;
        Started = true;
    }
    public void StopTimer()
    {
        Started = false;
    }
    #endregion

    #region Get & Set
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