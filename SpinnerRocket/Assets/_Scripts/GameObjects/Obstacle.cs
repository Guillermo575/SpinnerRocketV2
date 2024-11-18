using System.Linq;
using UnityEngine;
namespace GameElement
{
    /**
     * @class
     * @brief GameObject del juego: Objetos moviles cuya funcion es la de colisionar con el jugador
     */
    public class Obstacle : MonoBehaviour
    {
        #region Variables privada
        /** @hidden */  private new Transform transform;
        /** @hidden */  private new Rigidbody rigidbody;
        /** @hidden */  private GameManager gameManager;
        /** Objetivo del obstaculo, el obstaculo perseguira o intentara moverse hacia la direccion donde se encuentra */ 
        private GameObject objTarget;
        /** Variable que indica si ya tiene fijado al objetivo */
        private bool targetLock = false;
        /** Indica la direccion de velocidad del objeto */
        private Vector3 direction = Vector3.zero;
        #endregion

        #region Variables Editor
        /** Velocidad de movimiento del objeto*/
        public float SpeedMovement = 3;
        /** Indica como se comportara el objeto, si es verdadero semovera siempre hacia el objetivo, si es falso se movera de forma constante a una direccion*/
        public bool IsStalker = false;
        /** Rango de deteccion para que el objeto persiga al objetivo en caso de que la variable IsStalker sea verdadera */
        public float StalkRange = 12;
        #endregion

        #region General
        /** Inicializacion de los objetos */
        void Start()
        {
            transform = GetComponent<Transform>();
            rigidbody = GetComponent<Rigidbody>();
            gameManager = GameManager.GetSingleton();
            var objmain = GameObject.FindObjectsByType<Player>(FindObjectsSortMode.InstanceID);
            objTarget = objmain.Length == 0 ? null : objmain[0].gameObject;
        }
        /** Metodo que actualiza la direccion a la que se movera el objeto*/
        void Update()
        {
            if (gameManager.IsGameActive)
            {
                var MinX = gameManager.MinValues.x;
                var MinY = gameManager.MinValues.y;
                var MaxX = gameManager.MaxValues.x;
                var MaxY = gameManager.MaxValues.y;
                if (!targetLock)
                {
                    RotateTowards(objTarget == null ? new Vector3(0f, 0f, 0f) : objTarget.transform.position);
                    targetLock = true;
                }
                if (transform.position.x < MinX - 1 || transform.position.x > MaxX + 1 || 
                    transform.position.y < MinY - 1 || transform.position.y > MaxY + 1)
                {
                    transform.position = gameManager.mathRNG.getRandomSpawnPoint(gameManager.MinValues, gameManager.MaxValues);
                    RotateTowards(objTarget == null ? new Vector3(0f, 0f, 0f) : objTarget.transform.position);
                    direction = Vector3.zero;
                }
                if(!IsStalker)
                {
                    if (direction == Vector3.zero)
                        setSpeed(SpeedMovement);
                    transform.position += direction * SpeedMovement * Time.deltaTime;
                }
                else
                {
                    var distance = Vector3.Distance(objTarget.transform.position, transform.position);
                    if(distance < StalkRange)
                    {
                        RotateTowards(objTarget == null ? new Vector3(0f, 0f, 0f) : objTarget.transform.position);
                        setSpeed(gameManager.IsGameEnd? 0 : SpeedMovement);
                    }
                    else
                    {
                        setSpeed(0);
                    }
                }
            }
            else
            {
                setSpeed(0);
            }
        }
        #endregion

        #region setDirection
        /** Metodo que ajusta la velocidad del objeto */
        public void setSpeed(float Speed)
        {
            direction = objTarget.transform.position - transform.position;
            direction.z = 0;
            direction.Normalize();
            transform.position += direction * Speed * Time.deltaTime;
        }
        /** Metodo que actualiza la direccion a la que se movera el objeto */
        public void RotateTowards(Vector2 target)
        {
            var targetPosition = target == null ? new Vector2(0f, 0f) : target;
            var offset = 180f;
            Vector2 direction = (Vector2)transform.position - targetPosition;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        }
        #endregion
    }
    //newVelocity.x = rigidbody.rotation > 90.0f || rigidbody.rotation < -90.0f ? -newVelocity.x : newVelocity.x;
    //newVelocity.y = rigidbody.rotation > 0 ? -newVelocity.y : newVelocity.y
}