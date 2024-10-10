using UnityEngine;
using System.Linq;
using System.Collections;
public class Player : MonoBehaviour
{
    #region Unity Variables
    private new Transform transform;
    private new Renderer renderer;
    private bool InMovement = false;
    private bool IsDeath = false;
    private bool Stucked = false;
    private float SpeedObject = 0;
    private GameManager gameManager;
    private AudioManager audioManager;
    private float FraccAngle = 360 / 8;
    #endregion

    #region Editor Variables
    public ParticleSystem ParticleLaunch;
    public ParticleSystem ParticleBurst;
    public ParticleSystem ParticleBling;
    public AudioClip ClipLevelCleared;
    public AudioClip ClipStarPick;
    public AudioClip ClipExplosion;
    public int RotationXMin = 180;
    public int SpeedMovement = 15;
    public double DecreaseSpeed = 0.2;
    public double IncreaseSpeed = 0.8;
    #endregion

    #region General
    void Start()
    {
        gameManager = GameManager.GetSingleton();
        audioManager = AudioManager.GetSingleton();
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
    }
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
            if (Input.GetKeyDown(KeyCode.Z))
            {
                RotationXMin = -RotationXMin;
            }
            if (!Input.GetKey(KeyCode.Space))
            {
                Stucked = false;
                if(SpeedObject <= 0)
                {
                    transform.Rotate(0, 0, -(RotationXMin * Time.deltaTime));
                }
                MoveArrow((float)-DecreaseSpeed);
                InMovement = false;
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
                if (!InMovement)
                {
                    InMovement = true;
                }
                MoveArrow((float)IncreaseSpeed);
            }
            if(gameManager.IsGameOver || gameManager.IsLevelCleared)
            {
                StopArrow();
            }
        }
    }
    #endregion

    #region Collider
    private void OnCollisionEnter(Collision collision)
    {
        if (gameManager.IsGameStart)
        {
            if (collision.gameObject.tag == "Door")
            {
                var DoorAnimator = collision.gameObject.GetComponent<Animator>();
                if(DoorAnimator.GetBool("Opened"))
                {
                    audioManager.PlaySound(ClipLevelCleared);
                    gameManager.GameLevelCleared();
                    DoorAnimator.SetBool("Opened", false);
                    renderer.enabled = false;
                    StopArrow();
                }
            }
            if (collision.gameObject.tag == "Star")
            {
                audioManager.PlaySound(ClipStarPick);
                ParticleBling.Play();
                collision.gameObject.transform.position = new Vector3(gameManager.mathRNG.NextValueFloat(-9, 9), gameManager.mathRNG.NextValueFloat(-5, 5), 0);
                gameManager.AddScore(1);
            }
            if (collision.gameObject.tag == "StarLevel")
            {
                var objRender = collision.gameObject.GetComponent<Renderer>();
                if(objRender.enabled)
                {
                    StartCoroutine(CoroutineStarPick(objRender));
                }
            }
            if (collision.gameObject.tag == "Obstaculo" && !gameManager.IsGameEnd && !gameManager.IsInvencibleMode)
            {
                StartCoroutine(CoroutineDeath());
            }
        }
    }
    IEnumerator CoroutineStarPick(Renderer objRender)
    {
        audioManager.PlaySound(ClipStarPick);
        ParticleBling.Play();
        objRender.enabled = false;
        gameManager.AddScore(1);
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1;
    }
    IEnumerator CoroutineDeath()
    {
        IsDeath = true;
        //animator.SetBool("Death", true);
        StopArrow();
        ParticleLaunch.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        Time.timeScale = .0f;
        yield return new WaitForSecondsRealtime(0.3f);
        audioManager.PlaySound(ClipExplosion);
        ParticleBurst.Play();
        renderer.enabled = false;
        Time.timeScale = .2f;
        yield return new WaitForSecondsRealtime(0.5f);
        gameManager.GameOver();
        yield return new WaitForSecondsRealtime(4);
        Time.timeScale = 1;
    }
    #endregion

    #region Set Speed
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
    private void StopArrow()
    {
        SpeedObject = 0;
        Vector3 direction = transform.up;
        transform.position += direction * SpeedObject * Time.deltaTime;
    }
    #endregion
}