using System.Linq;
using UnityEngine;
/**
 * @file
 * @brief Administra atajos para agilizar las pruebas durante el desarrollo (se recomienda desactivar despues de hacer pruebas)
 */
public class GameCheatMode : MonoBehaviour
{
    #region Variables
    /** @hidden*/ GameManager GameManager;
    /** @hidden*/ AudioManager audioManager;
    /** @hidden*/ bool KeyPress = false;
    #endregion

    #region Start & Update
    /** Inicializacion de los objetos */
    void Start()
    {
        GameManager = GameManager.GetSingleton();
        audioManager = AudioManager.GetSingleton();
    }
    /** Revisa cual teclado esta siendo oprimido */
    void Update()
    {
        if(!KeyPress)
        {
            switch(Input.inputString)
            {
                case "1": SwitchInvincibleMode(); break;
                case "2": ClearStars(); break;
                case "3": TelePortDoor(); break;
                case "4": MuteGame(); break;
            }
        }
        KeyPress = Input.anyKey;
    }
    #endregion

    #region Metodos
    /** Te vuelves invencible al oprimir la tecla 1 */
    public void SwitchInvincibleMode()
    {
        GameManager.setInvencibleMode(!GameManager.IsInvencibleMode);
        Debug.Log($"SwitchInvincibleMode = {GameManager.IsInvencibleMode}" );
    }
    /** Tomas las estrellas al oprimir la tecla 2 */
    public void ClearStars()
    {
        Debug.Log("ClearStars");
        var lstObjects = GameObject.FindGameObjectsWithTag("StarLevel").Where(obj => obj.GetComponent<Renderer>().enabled).ToList();
        foreach (var x in lstObjects)
        {
            var objRender = x.gameObject.GetComponent<Renderer>();
            objRender.enabled = false;
            GameManager.AddScore(1);
        }
    }
    /** Te transportas a la puerta al oprimir la tecla 3 */
    public void TelePortDoor()
    {
        Debug.Log("TeleportDoor");
        var lstMainPlayer = GameObject.FindGameObjectsWithTag("Player").ToList();
        var lstDoorLevel = GameObject.FindGameObjectsWithTag("Door").ToList();
        if(lstMainPlayer.Count > 0 && lstDoorLevel.Count > 0)
        {
            var objMainPlayer = lstMainPlayer.First().gameObject.GetComponent<Transform>();
            var objDoorLevel = lstDoorLevel.First().gameObject.GetComponent<Transform>();
            objMainPlayer.position = objDoorLevel.position;
        }
    }
    /** Silencia o reactiva el audio */
    public void MuteGame()
    {
        audioManager.ToogleMute();
    }
    #endregion
}