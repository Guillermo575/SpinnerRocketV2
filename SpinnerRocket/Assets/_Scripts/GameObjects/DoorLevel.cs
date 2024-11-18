using System.Linq;
using UnityEngine;
namespace GameElement
{
    /**
     * @class
     * @brief GameObject del juego: Puerta del nivel que debe de acceder el jugador para completar el nivel
     */
    public class DoorLevel : MonoBehaviour
    {
        #region Unity Variables
        /** @hidden */   private Animator animator;
        /** @hidden */   private GameManager GameManager;
        #endregion

        #region Start & Update
        /** Inicializacion de los objetos */
        void Start()
        {
            animator = GetComponent<Animator>();
            GameManager = GameManager.GetSingleton();
        }
        /** Metodo de actualizacion que revisa la cantidad de estrellas del nivel, en caso de no haber estrellas la puerta se abrira*/
        void Update()
        {
            var objects = GameObject.FindGameObjectsWithTag("StarLevel").Where(obj => obj.GetComponent<Renderer>().enabled).ToList();
            if(objects.Count() == 0 && !animator.GetBool("Opened") && !GameManager.IsLevelCleared)
            {
                //GameManager.objGameAudioBehavior.PlaySoundEffect(GameManager.objGameAudioBehavior.ClipDoorOpen);
                animator.SetBool("Opened", true);
            }
        }
        #endregion
    }
}