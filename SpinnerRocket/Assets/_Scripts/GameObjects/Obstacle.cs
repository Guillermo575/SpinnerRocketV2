using System.Linq;
using UnityEngine;
/**
 * @file
 * @brief GameObject del juego: Objetos moviles cuya funcion es la de colisionar con el jugador
 */
namespace GameElement
{
    public class Obstacle : MonoBehaviour
    {
        #region Variables
        private new Transform transform;
        private new Rigidbody rigidbody;
        private GameManager gameManager;
        private GameObject objTarget;
        private bool targetLock = false;
        private Vector3 direction = Vector3.zero;
        public float SpeedMovement = 3;
        public bool IsStalker = false;
        public float StalkRange = 12;
        #endregion

        #region General
        void Start()
        {
            transform = GetComponent<Transform>();
            rigidbody = GetComponent<Rigidbody>();
            gameManager = GameManager.GetSingleton();
            var objmain = GameObject.FindObjectsByType<Player>(FindObjectsSortMode.InstanceID);
            objTarget = objmain.Length == 0 ? null : objmain[0].gameObject;
        }
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
        public void setSpeed(float Speed)
        {
            direction = objTarget.transform.position - transform.position;
            direction.z = 0;
            direction.Normalize();
            transform.position += direction * Speed * Time.deltaTime;
        }
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