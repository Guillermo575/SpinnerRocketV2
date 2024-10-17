using System.Linq;
using UnityEngine;
namespace GameElement
{
    public class DoorLevel : MonoBehaviour
    {
        #region Unity Variables
        private Animator animator;
        private GameManager GameManager;
        private int ScoreLevel =0;
        #endregion

        #region Start & Update
        void Start()
        {
            animator = GetComponent<Animator>();
            GameManager = GameManager.GetSingleton();
        }
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