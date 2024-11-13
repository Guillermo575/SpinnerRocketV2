using UnityEngine;
/**
 * @class
 * @brief GameObject del juego: Agujero negro que arrastrara al jugador hacia su centro si se acerca al area de colision
 */
namespace GameElement
{
    public class BlackHole : MonoBehaviour
    {
        #region Variables
        /** @hidden*/   [HideInInspector] public new Transform transform;
        /** @hidden*/   [HideInInspector] public GameManager gameManager;
        /** Fuerza de traccion que hara el objeto hacia el jugador*/
        public float SpeedTraction = 1;
        #endregion

        #region Start & update
        /** Inicializacion de los objetos*/
        void Start()
        {
            gameManager = GameManager.GetSingleton();
            transform = GetComponent<Transform>();
        }
        /** Metodo que hace que el objeto rote con el tiempo */
        void Update()
        {
            transform.Rotate(0, 0, -(5 * Time.deltaTime));
        }
        #endregion

        #region General
        /** Metodo de colision que arrastra al jugador hacia el de acuerdo a la variable de SpeedTraction*/
        void OnTriggerStay(Collider collision)
        {
            if (gameManager.IsGameActive)
            {
                if (collision.gameObject.tag == "Player")
                {
                    collision.gameObject.transform.position = Vector2.MoveTowards(collision.gameObject.transform.position, transform.position, SpeedTraction * Time.deltaTime);
                }
            }
        }
        #endregion
    }
}