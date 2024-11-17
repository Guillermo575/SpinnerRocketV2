using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.InputSystem;
/**
 * @class
 * @brief GameObject del juego: Controla las funciones de la nave que controla el jugador
 */
namespace GameElement
{
    public class Player : MonoBehaviour
    {
        #region Unity Variables
        /** @hidden*/ private new Transform transform;
        /** @hidden*/ private new Renderer renderer;
        /** @hidden*/ private GameManager gameManager;
        /** @hidden*/ private AudioManager audioManager;
        /** Indica si el objeto se encuentra en movimiento */
        private bool InMovement = false;
        /** Indica si el objeto colisiono y por tanto ocurrira un game over*/
        private bool IsDeath = false;
        /** Variable que sirve para evitar que el objeto se salga de los limites del escenario y este quede atorado en los bordes*/
        private bool Stucked = false;
        /** Velocidad actual del objeto */
        private float SpeedObject = 0;
        /** Ajuste constante de rotacion a la cual la nave va girando */
        private float FraccAngle = 360 / 8;
        /** Fuente de audio que usara el objeto para reproducir los sonidos */
        internal AudioSource SourceDisparo;
        #endregion

        #region Controles
        /** Objeto del input manager */
        private InputManager inputManager;
        /** InputAction que gestionara la propulsion de la nave */
        private InputAction inputLaunch;
        /** InputAction que gestionara el cambio de rotacion */
        private InputAction inputRotate;
        #endregion

        #region Editor Variables
        /** Particulas utilizadas al momento de propulsionar la nave */
        public ParticleSystem ParticleLaunch;
        /** Particulas utilizadas al momento de que la nave se estrelle */
        public ParticleSystem ParticleBurst;
        /** Particulas utilizadas al momento de que la nave obtenga una estrella */
        public ParticleSystem ParticleBling;
        /** Clip de sonido que se reproducira al momento de superar un nivel */
        public AudioClip ClipLevelCleared;
        /** Clip de Sonido que se reproducira al momento de obtener una estrella */
        public AudioClip ClipStarPick;
        /** Clip de Sonido que se reproducira al momento de que la nave se estrelle */
        public AudioClip ClipExplosion;
        /** Velocidad que indica cuantas veces rotara la nave por minuto */
        public int RotationXMin = 180;
        /** Velocidad maxima de la nave al momento de hacer propulsion */
        public int SpeedMovement = 15;
        /** Velocidad de perdida al momento de dejar de hacer propulsion por cada Update */
        public double DecreaseSpeed = 0.2;
        /** Velocidad de incremento durante la propulsion por cada Update */
        public double IncreaseSpeed = 0.8;
        #endregion

        #region General
        /** Metodo de inicio del objeto */
        void Start()
        {
            gameManager = GameManager.GetSingleton();
            audioManager = AudioManager.GetSingleton();
            inputManager = InputManager.GetSingleton();
            SourceDisparo = this.GetComponent<AudioSource>();
            transform = GetComponent<Transform>();
            renderer = GetComponent<Renderer>();
            SpeedObject = 0;
            var lstParticle = this.gameObject.GetComponentsInChildren<ParticleSystem>();
            var lstLaunch = (from x in lstParticle where x.gameObject.name == ParticleLaunch.gameObject.name select x).ToList();
            ParticleLaunch = lstLaunch.Count > 0 ? lstLaunch[0] : ParticleLaunch;
            var lstBurst = (from x in lstParticle where x.gameObject.name == ParticleBurst.gameObject.name select x).ToList();
            ParticleBurst = lstBurst.Count > 0 ? lstBurst[0] : ParticleBurst;
            var lstBling = (from x in lstParticle where x.gameObject.name == ParticleBling.gameObject.name select x).ToList();
            ParticleBling = lstBling.Count > 0 ? lstBling[0] : ParticleBling;
            inputLaunch = inputManager.GetAction("Launch");
            inputRotate = inputManager.GetAction("Rotate");
            inputLaunch.Enable();
            inputRotate.Enable();
            inputLaunch.performed += LaunchMove;
            inputLaunch.canceled += LaunchStop;
            inputRotate.performed += RotateMove;
        }
        /** Metodo de actualizacion del objeto, revisa si el objeto se encuentra en movimiento, activo o en proceso de Game Over */
        void Update()
        {
            if (!IsDeath && gameManager.IsGameStart && gameManager.IsGameActive)
            {
                var MinX = gameManager.MinValues.x;
                var MinY = gameManager.MinValues.y;
                var MaxX = gameManager.MaxValues.x;
                var MaxY = gameManager.MaxValues.y;
                var RenderWidth = (renderer.bounds.size.x / 2);
                var RenderHeight = (renderer.bounds.size.y / 2);
                var transformX = Mathf.Clamp(transform.position.x, MinX + RenderWidth, MaxX - RenderWidth);
                var transformY = Mathf.Clamp(transform.position.y, MinY + RenderHeight, MaxY - RenderHeight);
                Stucked = Stucked == false ? !(transformX == transform.position.x && transformY == transform.position.y) : Stucked;
                transform.position = new Vector3(transformX, transformY, transform.position.z);
                if (!InMovement)
                {
                    Stucked = false;
                    if(SpeedObject <= 0)
                    {
                        transform.Rotate(0, 0, -(RotationXMin * Time.deltaTime));
                    }
                    MoveArrow((float)-DecreaseSpeed);
                    if (ParticleLaunch != null && ParticleLaunch.isPlaying)
                    {
                        ParticleLaunch.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    }
                }
                else
                {
                    var ogAngle = transform.rotation.eulerAngles.z;
                    transform.rotation = Quaternion.Euler(0, 0, ogAngle - (ogAngle % 9));
                    if (ParticleLaunch != null && !ParticleLaunch.isPlaying)
                    {
                        ParticleLaunch.Play(true);
                    }
                    MoveArrow((float)IncreaseSpeed);
                }
                if(gameManager.IsGameOver || gameManager.IsLevelCleared)
                {
                    StopArrow();
                }
            }
        }
        /** Oculta los renders de la nave */
        public void HideRenderers()
        {
            renderer.enabled = false;
            var lstRenderer = this.gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach(var obj in lstRenderer)
            {
                obj.enabled = false;
            }
        }
        #endregion

        #region Collider
        /** Metodo que se activa cuando colisiona con un objeto 
         @param collision objeto de colision */
        private void OnCollisionEnter(Collision collision)
        {
            CollisionDetection(collision.gameObject, collision.gameObject.tag);
        }
        /** Metodo que se activa cuando colisiona con un objeto (en caso de que este activado el IsTrigger)
         @param collision objeto de colision
        */
        private void OnTriggerEnter(Collider collision)
        {
            CollisionDetection(collision.gameObject, collision.gameObject.tag);
        }
        /** Metodo de colision que se activa con OnCollisionEnter y OnTriggerEnter
        @param gameObject objeto de colision 
        @param CollisionTag tag del objeto de colision
         */
        private void CollisionDetection(GameObject gameObject, string CollisionTag)
        {
            if (gameManager.IsGameStart)
            {
                if (CollisionTag == "Door")
                {
                    var DoorAnimator = gameObject.GetComponent<Animator>();
                    if (DoorAnimator.GetBool("Opened"))
                    {
                        ParticleLaunch.Stop();
                        audioManager.StopSound();
                        PlayClip(ClipLevelCleared);
                        gameManager.GameLevelCleared();
                        DoorAnimator.SetBool("Opened", false);
                        HideRenderers();
                        StopArrow();
                    }
                }
                if (CollisionTag == "Star")
                {
                    PlayClip(ClipStarPick);
                    ParticleBling.Play();
                    gameObject.transform.position = new Vector3(gameManager.mathRNG.NextValueFloat(-9, 9), gameManager.mathRNG.NextValueFloat(-5, 5), 0);
                    gameManager.AddScore(1);
                }
                if (CollisionTag == "StarLevel")
                {
                    var objRender = gameObject.GetComponent<Renderer>();
                    if (objRender.enabled)
                    {
                        StartCoroutine(CoroutineStarPick(objRender));
                    }
                }
                if (gameObject.tag == "Obstaculo" && !gameManager.IsGameEnd && !gameManager.IsInvencibleMode)
                {
                    StartCoroutine(CoroutineDeath());
                }
            }
        }
        /** Courutina que se activa al obtener una estrella 
         @param objRender render de la estrella */
        IEnumerator CoroutineStarPick(Renderer objRender)
        {
            PlayClip(ClipStarPick);
            ParticleBling.Play();
            objRender.enabled = false;
            gameManager.AddScore(1);
            Time.timeScale = 0.1f;
            yield return new WaitForSecondsRealtime(0.1f);
            Time.timeScale = 1;
        }
        /** Courutina que se activa al morir a causa de una colision */
        IEnumerator CoroutineDeath()
        {
            IsDeath = true;
            //animator.SetBool("Death", true);
            StopArrow();
            ParticleLaunch.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            Time.timeScale = .0f;
            yield return new WaitForSecondsRealtime(0.3f);
            audioManager.StopSound();
            PlayClip(ClipExplosion);
            ParticleBurst.Play();
            HideRenderers();
            Time.timeScale = .2f;
            yield return new WaitForSecondsRealtime(0.5f);
            gameManager.GameOver();
            yield return new WaitForSecondsRealtime(4);
            Time.timeScale = 1;
        }
        /** Metodo que reproduce un efecto de sonido 
         @param clip audio que se reproducira*/
        private void PlayClip(AudioClip clip)
        {
            SourceDisparo.clip = clip;
            SourceDisparo.Play();
        }
        #endregion

        #region Set Speed
        /** Metodo de movimiento del objeto 
         @param Speed Velocidad de incremento de la nave
         */
        private void MoveArrow(float Speed)
        {
            if (Stucked)
            {
                StopArrow();
                return;
            }
            Vector3 direction = transform.up;
            transform.position += direction * SpeedObject * Time.deltaTime;
            SpeedObject += Speed;
            SpeedObject = SpeedObject >= SpeedMovement ? SpeedMovement : SpeedObject;
            SpeedObject = SpeedObject <= 0 ? 0 : SpeedObject;
        }
        /** Metodo para que se detenga del objeto */
        private void StopArrow()
        {
            SpeedObject = 0;
            Vector3 direction = transform.up;
            transform.position += direction * SpeedObject * Time.deltaTime;
        }
        #endregion

        #region InputAction
        /** Metodo de inputaction cuando oprimes el boton de propulsion */
        private void LaunchMove(InputAction.CallbackContext obj)
        {
            InMovement = true;
        }
        /** Metodo de inputaction cuando sueltas el boton de propulsion */
        private void LaunchStop(InputAction.CallbackContext obj)
        {
            InMovement = false;
        }
        /** Metodo de inputaction cuando oprimes el boton de rotacion */
        private void RotateMove(InputAction.CallbackContext obj)
        {
            RotationXMin = -RotationXMin;
        }
        #endregion
    }
}