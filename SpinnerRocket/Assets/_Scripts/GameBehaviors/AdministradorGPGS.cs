using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine.SceneManagement;
/**
 * @class
 * @brief Aqui se gestiona las funciones principales de Google Plays
 */
public class AdministradorGPGS : MonoBehaviour
{
    #region Variables
    /** @hidden*/
    private GameManager gameManager;
    /** Objeto que se usa para el debug */
    public TMPro.TMP_Text GPGSText;
    #endregion

    #region Start
    /** Inicializacion de los objetos */
    void Start()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcesarAutenticacion);
        gameManager = GameManager.GetSingleton();
        gameManager.OnGameLevelCleared += delegate
        {
            var scene = SceneManager.GetActiveScene();
            switch (scene.name)
            {
                case "Scene_0001": DesbloquearLogro(GPGSIds.achievement_level_1_finished); break;
                case "Scene_0002": DesbloquearLogro(GPGSIds.achievement_level_2_finished); break;
                case "Scene_0003": DesbloquearLogro(GPGSIds.achievement_level_3_finished); break;
                case "Scene_0004": DesbloquearLogro(GPGSIds.achievement_level_4_finished); break;
                case "Scene_0005": DesbloquearLogro(GPGSIds.achievement_level_5_finished); break;
                case "Scene_0006": DesbloquearLogro(GPGSIds.achievement_level_6_finished); break;
                case "Scene_0007": DesbloquearLogro(GPGSIds.achievement_level_7_finished); break;
                case "Scene_0008": DesbloquearLogro(GPGSIds.achievement_level_8_finished); break;
            }
        };
    }
    #endregion

    #region General
    /**
     * Metodo de pruebas para probar la autentificacion
     * @param status: Objeto SignInStatus que contiene los datos de autentificacion
     */
    internal void ProcesarAutenticacion(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            if (GPGSText != null)
            {
                GPGSText.text = $"Good Auth \n {Social.localUser.userName} \n {Social.localUser.id}";
            }
        }
        else
        {
            if (GPGSText != null)
            {
                GPGSText.text = $"Bad Auth";
            }
        }
    }
    /**
     * Sirve para activar un logro\n 
     * @param Logro: Id del logro de Google Plays
     */
    internal void DesbloquearLogro(string Logro)
    {
        string mStatus;
        Social.ReportProgress(
            Logro,
            100.0f,
            (bool success) =>
            {
                mStatus = success ? "Logro desbloqueado" : "Fallo en el desbloqueo delo logro";
                if (GPGSText != null)
                {
                    GPGSText.text = mStatus;
                }
            }
        );
    }
    #endregion
}