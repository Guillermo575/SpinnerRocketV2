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
    [HideInInspector] public float currentTime = 0.0f;
    [HideInInspector] public bool Started = false;
    #endregion

    #region Start & Update
    void Awake()
    {
        CreateSingleton();
    }
    public void Start()
    {
    }
    public void Update()
    {
        if (Started)
        {
            currentTime += 1 * Time.deltaTime;
        }
    }
    #endregion

    #region Start & Stop timer
    public void StartTimer()
    {
        currentTime = 0.0f;
        Started = true;
    }
    public void StopTimer()
    {
        Started = false;
    }
    #endregion

    #region Get & Set
    public bool SetRecord(string name)
    {
        var record = PlayerPrefs.GetFloat(name, -1);
        if(record < 0 || (record >= 0 && record > currentTime))
        {
            PlayerPrefs.SetFloat(name, currentTime);
            return true;
        }
        return false;
    }
    public string GetCurrentTimer()
    {
        var record = currentTime;
        string ret = string.Empty;
        if (record >= 0)
        {
            ret = Mathf.Round(record / 60) + ":" + ((int)Mathf.Round(record % 60)).ToString("00");
        }
        return ret;
    }
    public string GetRecord(string name)
    {
        var record = PlayerPrefs.GetFloat(name, -1);
        string ret = string.Empty;
        if (record >= 0)
        {
            ret = Mathf.Round(record / 60) + ":" + ((int)Mathf.Round(record % 60)).ToString("00");
        }
        return ret;
    }
    #endregion
}