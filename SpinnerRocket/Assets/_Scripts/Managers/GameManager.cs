using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
/**
 * @file
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
    /** @hidden*/ private GameState ActualGameState;
    public MathRNG mathRNG = new MathRNG(3241);
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
    /** @hidden*/
    private bool _GameEnd = false;
    /** Indica si el juego termino (nivel completado o es Game Over)*/
    public bool IsGameEnd { get { return _GameEnd; } }
    /** @hidden*/
    private bool GamePause = false;
    /** Indica si el juego esta pausado*/
    public bool IsGamePause { get { return GamePause; } }
    /** @hidden*/
    private bool LevelCleared = false;
    /** Indica si el nivel del juego fue completado*/
    public bool IsLevelCleared { get { return LevelCleared; } }
    /** Indica si el juego no esta pausado ni terminado*/
    public bool IsGameActive { get { return !_GameEnd && !GamePause && !LevelCleared; } }
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

    #region EventHandlers
    /** @hidden*/ public delegate void GameEvent();
    /** @hidden*/ public event GameEvent OnGameStart;
    /** @hidden*/ public event GameEvent OnGamePause;
    /** @hidden*/ public event GameEvent OnGameResume;
    /** @hidden*/ public event GameEvent OnGameEnd;
    /** @hidden*/ public event GameEvent OnGameOver;
    /** @hidden*/ public event GameEvent OnGameExit;
    /** @hidden*/ public event GameEvent OnGameLevelCleared;
    /** Clip musical que se reproduce en el juego*/
    public AudioClip ClipBGM;
    /** Clip musical que se reproduce cuando se completa el nivel*/
    public AudioClip ClipLevelCleared;
    /** Clip musical que se reproduce cuando pierdes el nivel*/
    public AudioClip ClipGameOver;

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
            GamePause = true;
            OnGamePause();
        }
    }
    /** Reanuda el juego si estuvo pausado*/
    public void ResumeGame()
    {
        GamePause = false;
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
    /** @hidden*/
    private void Awake()
    {
        OnGameStart += delegate { Time.timeScale = 1; };
        OnGamePause += delegate { Time.timeScale = 0; };
        OnGameResume += delegate { Time.timeScale = 1; };
        OnGameEnd += delegate { Time.timeScale = 0; };
        OnGameOver += delegate { OnGameEnd(); };
        OnGameLevelCleared += delegate { OnGameEnd(); };
        OnGameExit += delegate { Time.timeScale = 1; };
    }
    /** @hidden*/
    private void Start()
    {
        audioManager = AudioManager.GetSingleton();
        if (audioManager != null)
        {
            OnGameLevelCleared += delegate { audioManager.PlaySound(ClipLevelCleared); };
            OnGameOver += delegate { audioManager.PlaySound(ClipGameOver); };
        }
        OnGameStart();
    }
    /** @hidden*/
    private void Update()
    {
        if (IsGameEnd) return;
        if (ActualGameState == GameState.Action)
        {
        }
    }
    /** @hidden*/
    private void OnEnable()
    {
        CreateSingleton();
    }
    #endregion
}