using System.Linq;
using UnityEngine;
/**
 * @file
 * @brief Administra atajos para agilizar las pruebas durante el desarrollo (se recomienda desactivar despues de hacer pruebas)
 */
public class GameCheatMode : MonoBehaviour
{
    #region Variables
    GameManager GameManager;
    AudioManager audioManager;
    bool KeyPress = false;
    #endregion

    #region Start & Update
    void Start()
    {
        GameManager = GameManager.GetSingleton();
        audioManager = AudioManager.GetSingleton();
    }
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
    public void SwitchInvincibleMode()
    {
        GameManager.setInvencibleMode(!GameManager.IsInvencibleMode);
        Debug.Log($"SwitchInvincibleMode = {GameManager.IsInvencibleMode}" );
    }
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
    public void MuteGame()
    {
        audioManager.ToogleMute();
    }
    #endregion
}