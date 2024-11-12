using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/**
 * @file
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
    private GameManager objGameManager;
    private GameTimeWatch objGameTimeWatch;
    private AudioManager audioManager;
    private MenuManager menuManager;
    public GameObject HUD;
    public Image btnSoundON;
    public Image btnSoundOFF;
    public enum ScoreType
    {
        None = 0,
        Points = 1,
        Timer = 2,
    }
    public ScoreType scoretype = ScoreType.None;
    public TextMeshProUGUI txtScore;
    public TextMeshProUGUI txtHighScore;
    public TextMeshProUGUI txtTitle;
    public TextMeshProUGUI txtStarCurrent;
    public TextMeshProUGUI txtLaunch;
    #endregion

    #region Start & Update
    void Awake()
    {
        CreateSingleton();
    }
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
    public void setTitle()
    {
        if(txtTitle != null)
        {
            txtTitle.text = objGameManager.LevelName;
        }
    }
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