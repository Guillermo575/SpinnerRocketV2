using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
/**
 * @class
 * @brief Administrador principal del juegol, indica las fases en que se ordena el ciclo de juego
 */
public class GameManager : MonoBehaviour
{
    #region Singleton
    /** @hidden*/
    private static GameManager SingletonGameManager;
    /** @hidden*/
    private GameManager()
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
    /** Solo se puede crear un objeto de la clase GameManager, este metodo obtiene el objeto creado */
    public static GameManager GetSingleton()
    {
        return SingletonGameManager;
    }
    #endregion

    #region Privados
    /** @hidden*/ private AudioManager audioManager;
    /** @hidden*/ private MenuManager menuManager;
    /** @hidden*/ private GameState ActualGameState;
    /** @hidden*/ private float intervaloTiempoLaunch = 0.6f;
    /** @hidden*/ private GameTimeWatch objGameTimeWatch;
    /** Objeto que crea numeros psudoaleatorios */
    public MathRNG mathRNG = new MathRNG(3241);
    /** Objeto alterno que crea numeros psudoaleatorios para propositos adicionales */
    public MathRNG mathRNGOther = new MathRNG(3241);
    #endregion

    #region Getters & Setters
    /** Obtiene el estado dela ctual del juego*/
    public GameState GetActualGameState()
    {
        return ActualGameState;
    }
    #endregion

    #region Level Game Variables

    #region GameStart, GameEnd & GameActive
    /** @hidden*/
    private bool _GameStart = false;
    /** Indica si el juego empezo (Y ya se puede contar los procesos de GameOver y LevelCleared)*/
    public bool IsGameStart { get { return _GameStart; } }
    /** @hidden*/
    private bool _GameEnd = false;
    /** Indica si el juego termino (nivel completado o es Game Over)*/
    public bool IsGameEnd { get { return _GameEnd; } }
    /** Indica si el juego no esta pausado ni terminado*/
    public bool IsGameActive { get { return _GameStart && !_GameEnd && !_GamePause && !LevelCleared; } }
    #endregion

    #region GamePause
    /** @hidden*/
    private bool _GamePause = false;
    /** Indica si el juego esta pausado*/
    public bool IsGamePause { get { return _GamePause; } }
    #endregion

    #region LevelCleared 
    /** @hidden*/
    private bool LevelCleared = false;
    /** Indica si el nivel del juego fue completado*/
    public bool IsLevelCleared { get { return LevelCleared; } }
    #endregion

    #region GameOver
    /** @hidden*/
    private bool _GameOver = false;
    /** Indica si perdiste en el juego*/
    public bool IsGameOver { get { return _GameOver; } }
    #endregion

    #region InvencibleMode
    /** @hidden*/
    private bool _InvencibleMode = false;
    /** Indica si el juego se encuentra en modo de invencibilidad, al estar asi no puedes perder*/
    public bool IsInvencibleMode { get { return _InvencibleMode; } }
    public void setInvencibleMode(bool _InvencibleMode) { this._InvencibleMode = _InvencibleMode; }
    #endregion

    #region Score
    /** @hidden*/
    private int _Score;
    /** Regresa la puntuacion del nivel*/
    public int GetScore { get { return _Score; } }
    /** Agrega puntos al nivel*/
    public void AddScore (int Score) { _Score += Score; }
    #endregion

    #region Min & Max Values
    /** @hidden*/
    private Vector3 _minValues;
    public Vector3 MinValues { get { return _minValues; } }
    /** @hidden*/
    private Vector3 _maxValues;
    public Vector3 MaxValues { get { return _maxValues; } }
    #endregion

    #region ConteoLaunch
    /** @hidden*/ private string _ConteLaunch = String.Empty;
    public string ConteLaunch { get { return _ConteLaunch; } }
    #endregion

    #endregion

    #region GameState
    /** Estados del juego*/
    public enum GameState
    {
        Preparation = 0,
        Action = 1,
        Ended = 2,
    }
    #endregion

    #region Editor Variables
    /** Clip musical que se reproduce en el juego*/
    public AudioClip ClipBGM;
    /** Clip musical que se reproduce cuando se completa el nivel*/
    public AudioClip ClipLevelCleared;
    /** Clip musical que se reproduce cuando pierdes el nivel*/
    public AudioClip ClipGameOver;
    /** Objeto que funciona como delimitador interno de los limites del escenario*/
    public GameObject objLimites;
    /** Nombre del nivel que vera el jugador*/
    public String LevelName;
    #endregion

    #region EventHandlers
    /** @hidden*/
    public delegate void GameEvent();
    /** @hidden*/ public event GameEvent OnGamePreparation;
    /** @hidden*/ public event GameEvent OnGameStart;
    /** @hidden*/ public event GameEvent OnGamePause;
    /** @hidden*/ public event GameEvent OnGameResume;
    /** @hidden*/ public event GameEvent OnGameEnd;
    /** @hidden*/ public event GameEvent OnGameOver;
    /** @hidden*/ public event GameEvent OnGameExit;
    /** @hidden*/ public event GameEvent OnGameLevelCleared;

    /** Inicia el juego*/
    public void PreparationGame()
    {
        _GameStart = false;
        ActualGameState = GameState.Preparation;
        OnGamePreparation();
    }
    /** Inicia el juego*/
    public void StartGame()
    {
        _GameEnd = false;
        OnGameStart();
    }
    /** Pausa el juego*/
    public void PauseGame()
    {
        if (ActualGameState == GameState.Action || ActualGameState == GameState.Preparation)
        {
            _GamePause = true;
            OnGamePause();
        }
    }
    /** Reanuda el juego si estuvo pausado*/
    public void ResumeGame()
    {
        _GamePause = false;
        OnGameResume();
    }
    /** Finaliza el nivel*/
    public void GameEnd()
    {
        _GameEnd = true;
        OnGameEnd();
    }
    /** Pierdes el nivel*/
    public void GameOver()
    {
        OnGameOver();
    }
    /** Completas el nivel*/
    public void GameLevelCleared()
    {
        LevelCleared = true;
        OnGameLevelCleared();
    }
    #endregion

    #region Awake, Start & Update
    /** Inicializa la mayoria de los objetos y metodos principales */
    private void Awake()
    {
        if (objLimites != null)
        {
            var SizeTile = new Vector3(objLimites.transform.localScale.x/2, objLimites.transform.localScale.y / 2, 0);
            _minValues = new Vector3(objLimites.transform.position.x - SizeTile.x, objLimites.transform.position.y - SizeTile.y, 0);
            _maxValues = new Vector3(objLimites.transform.position.x + SizeTile.x, objLimites.transform.position.y + SizeTile.y, 0);
        }
        OnGamePreparation += delegate { StartCoroutine(CoroutinePrepareLaunch()); };
        OnGameStart += delegate { _GameStart = true; Time.timeScale = 1; objGameTimeWatch.StartTimer(); };
        OnGamePause += delegate { Time.timeScale = 0; };
        OnGameResume += delegate { Time.timeScale = 1; };
        OnGameEnd += delegate { _GameStart = false; ActualGameState = GameState.Ended; };
        OnGameOver += delegate { OnGameEnd(); };
        OnGameLevelCleared += delegate { OnGameEnd(); SiguienteNivel(); };
        OnGameExit += delegate { Time.timeScale = 1; };
    }
    /** Inicializacion de los objetos */
    private void Start()
    {
        audioManager = AudioManager.GetSingleton();
        objGameTimeWatch = GameTimeWatch.GetSingleton();
        menuManager = MenuManager.GetSingleton();
        if (audioManager != null)
        {
            OnGameLevelCleared += delegate { audioManager.PlaySound(ClipLevelCleared); };
            OnGameOver += delegate { audioManager.PlaySound(ClipGameOver); };
        }
        menuManager.opciones.LastLevel = SceneManager.GetActiveScene().name;
        //OnGameStart();
        PreparationGame();
    }
    /** Actualizacion de los objetos */
    private void Update()
    {
        if (IsGameEnd) return;
        if (ActualGameState == GameState.Action)
        {
        }
    }
    /** Crea el singleton */
    private void OnEnable()
    {
        CreateSingleton();
    }
    /** Corutina que crea un intervalo de espera al inicio del nivel, muestra un texto que cuenta del 3 al 0 para dar inicio al juego */
    IEnumerator CoroutinePrepareLaunch()
    {
        for (int l = 3; l > 0 && !_GameStart; l--)
        {
            _ConteLaunch = l.ToString();
            yield return new WaitForSeconds(intervaloTiempoLaunch);
        }
        _ConteLaunch = String.Empty;
        StartGame();
    }
    /** Actualiza el siguiente nivel y guarda dicho escenario en caso de querer continuar donde te quedaste */
    public void SiguienteNivel()
    {
        var siguienteNivel = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > siguienteNivel)
        {
            var sceneLastName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(siguienteNivel));
            if (sceneLastName.StartsWith("Scene_"))
            {
                menuManager.opciones.LastLevel = sceneLastName;
            }
            else
            {
                //menuManager.opciones.LastLevel = "Scene_0001";
            }
        }
        else
        {
            menuManager.opciones.LastLevel = "Scene_0001";
        }
    }
    #endregion
}