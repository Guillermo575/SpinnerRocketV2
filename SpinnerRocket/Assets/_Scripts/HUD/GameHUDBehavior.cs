using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/**
 * @class
 * @brief Administra las funciones de la interfaz de usuario
 */
public class GameHUDBehavior : MonoBehaviour
{
    #region Singleton
    /** @hidden*/
    private static GameHUDBehavior SingletonGameManager;
    /** @hidden*/
    private GameHUDBehavior()
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
    /** Solo se puede crear un objeto de la clase GameHUDBehavior, este metodo obtiene el objeto creado */
    public static GameHUDBehavior GetSingleton()
    {
        return SingletonGameManager;
    }
    #endregion

    #region Variables
    /** @hidden*/ private GameManager objGameManager;
    /** @hidden*/ private GameTimeWatch objGameTimeWatch;
    /** @hidden*/ private AudioManager audioManager;
    /** @hidden*/ private MenuManager menuManager;
    /** Objeto que guarda el HUD */
    public GameObject HUD;
    /** @hidden*/ public Image btnSoundON;
    /** @hidden*/ public Image btnSoundOFF;
    /** Enum que indica cual es el tipo de puntuacion que tendra el nivel (por puntos o por tiempo) */
    public enum ScoreType
    {
        None = 0,
        Points = 1,
        Timer = 2,
    }
    /** Tipo de puntuacion que tiene el nivel */
    public ScoreType scoretype = ScoreType.None;
    /** objeto de texto de puntuacion */
    public TextMeshProUGUI txtScore;
    /** objeto de texto de puntuacion mas alta*/
    public TextMeshProUGUI txtHighScore;
    /** objeto de texto del nombre del nivel */
    public TextMeshProUGUI txtTitle;
    /** objeto de texto de las estrellas actuales del nivel */
    public TextMeshProUGUI txtStarCurrent;
    /** objeto de texto que se muestra al inicio del nivel indicando el tiempo de inicio del nivel*/
    public TextMeshProUGUI txtLaunch;
    #endregion

    #region Start & Update
    /** Metodo para crear el singleton */
    void Awake()
    {
        CreateSingleton();
    }
    /** Metodo de inicio de los objetos */
    void Start()
    {
        objGameManager = GameManager.GetSingleton();
        menuManager = MenuManager.GetSingleton();
        audioManager = AudioManager.GetSingleton();
        objGameTimeWatch = GameTimeWatch.GetSingleton();
        setTitle();
        switch (scoretype)
        {
            case ScoreType.Points:
                txtScore.alignment = TextAlignmentOptions.Left;
                txtHighScore.alignment = TextAlignmentOptions.Left;
                break;
            case ScoreType.Timer:
                txtScore.alignment = TextAlignmentOptions.Right;
                txtHighScore.alignment = TextAlignmentOptions.Right;
                break;
        }
        objGameManager.OnGameEnd += delegate { };
        objGameManager.OnGameLevelCleared += delegate {
            if (scoretype == ScoreType.Timer)
            {
                SetTimerRecord(SceneManager.GetActiveScene().name);
            }
        };
    }
    /** Metodo de actualizacion del objeto */
    void Update()
    {
        setScore();
        setStarCurrent();
        btnSoundON.enabled = !audioManager.IsMute;
        btnSoundOFF.enabled = audioManager.IsMute;
        if (objGameManager.GetActualGameState() == GameManager.GameState.Preparation)
        {
            txtLaunch.text = objGameManager.ConteLaunch;
        }
        if (!objGameManager.IsGameStart || objGameManager.IsGamePause)
        {
            HUD.SetActive(false);
        }
        if (objGameManager.IsGameStart && objGameManager.IsGameOver)
        {
            HUD.SetActive(false);
        }
        if (objGameManager.IsGameActive)
        {
            HUD.SetActive(true);
        }
    }
    #endregion

    #region General
    /** Actualiza las puntuaciones del HUD */
    public void setScore()
    {
        switch(scoretype)
        {
            case ScoreType.Points:
                if (txtScore != null)
                {
                    txtScore.text = "Score: " + objGameManager.GetScore;
                }
                var ScorePoints = menuManager.highScore.GetScore(SceneManager.GetActiveScene().name);
                if (ScorePoints < objGameManager.GetScore)
                {
                    menuManager.highScore.SetScore(new HighScore.Score { name = SceneManager.GetActiveScene().name, BestScore = ScorePoints });
                }
                if (txtHighScore != null)
                {
                    txtHighScore.text = "High: " + (ScorePoints < 0 ? 0 : ScorePoints);
                }
            break;
            case ScoreType.Timer:
                if (txtScore != null)
                {
                    txtScore.text = objGameTimeWatch.GetCurrentTimer();
                }
                if (txtHighScore != null)
                {
                    if (!string.IsNullOrEmpty(GetRecord(SceneManager.GetActiveScene().name)))
                    {
                        txtHighScore.text = "Score " + GetRecord(SceneManager.GetActiveScene().name);
                    }
                }
            break;
        }
    }
    /** Actualiza el nombre del nivel */
    public void setTitle()
    {
        if(txtTitle != null)
        {
            txtTitle.text = objGameManager.LevelName;
        }
    }
    /** Actualiza las estrellas del nivel, cuando el jugador obtenga todas las letras se tornaran en amarillo */
    public void setStarCurrent()
    {
        if (txtStarCurrent != null)
        {
            var TotalStars = GameObject.FindGameObjectsWithTag("StarLevel").ToList();
            var CurrentStars = TotalStars.Where(obj => !obj.GetComponent<Renderer>().enabled).ToList();
            txtStarCurrent.text = CurrentStars.Count + "/" + TotalStars.Count;
            if(CurrentStars.Count == TotalStars.Count)
            {
                txtStarCurrent.color = Color.yellow;
            }
            //txtStarCurrent.text = CurrentStars.Count == TotalStars.Count ? "*" + txtStarCurrent.text + "*" : txtStarCurrent.text;
        }
    }
    /** Actualiza el record actual 
     @param name nombre del nivel
     @return valor bool que indica si el record fue roto
     */
    public bool SetTimerRecord(string name)
    {
        var record = menuManager.highScore.GetScore(name);
        if (record < 0 || (record >= 0 && record > objGameTimeWatch.currentTime))
        {
            menuManager.highScore.SetScore(new HighScore.Score { name = name, BestScore = objGameTimeWatch.currentTime });
            return true;
        }
        return false;
    }
    /** Obtiene el record actual 
    @param name nombre del nivel
    @return string que devuelve el record en formato 0:00
    */
    public string GetRecord(string name)
    {
        var record = menuManager.highScore.GetScore(name);
        string ret = string.Empty;
        if (record >= 0)
        {
            ret = Mathf.Round(record / 60) + ":" + ((int)Mathf.Round(record % 60)).ToString("00");
        }
        return ret;
    }
    #endregion
}