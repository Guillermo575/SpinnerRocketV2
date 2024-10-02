using UnityEngine;
using System.Linq;
using System.Collections;
public class Player : MonoBehaviour
{
    #region Unity Variables
    [HideInInspector] public new Transform transform;
    [HideInInspector] public new Renderer renderer;
    #endregion

    #region InGame Variables
    [HideInInspector] public bool InMovement = false;
    [HideInInspector] public bool IsDeath = false;
    [HideInInspector] public bool Stucked = false;
    [HideInInspector] public float SpeedObject = 0;
    [HideInInspector] public GameManager GameManager;
    [HideInInspector] public AudioManager audioManager;
    [HideInInspector] public float FraccAngle = 360 / 8;
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
        GameManager = GameManager.GetSingleton();
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
        if (!IsDeath && GameManager.IsGameStart && GameManager.IsGameActive)
        {
            var MinX = GameManager.MinValues.x;
            var MinY = GameManager.MinValues.y;
            var MaxX = GameManager.MaxValues.x;
            var MaxY = GameManager.MaxValues.y;
            var RenderWidth = (renderer.bounds.size.x / 2);
            var RenderHeight = (renderer.bounds.size.y / 2);
            var transformX = Mathf.Clamp(transform.position.x, MinX + RenderWidth, MaxX - RenderWidth);
            var transformY = Mathf.Clamp(transform.position.y, MinY + RenderHeight, MaxY - RenderHeight);
            Stucked = Stucked == false ? !(transformX == transform.position.x && transformY == transform.position.y) : Stucked;
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
            if(GameManager.IsGameOver || GameManager.IsLevelCleared)
            {
                StopArrow();
            }
        }
    }
    #endregion

    #region Collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.IsGameStart)
        {
            if (collision.gameObject.tag == "Door")
            {
                var DoorAnimator = collision.gameObject.GetComponent<Animator>();
                if(DoorAnimator.GetBool("Opened"))
                {
                    audioManager.PlaySound(ClipLevelCleared);
                    GameManager.GameLevelCleared();
                    DoorAnimator.SetBool("Opened", false);
                    renderer.enabled = false;
                    StopArrow();
                }
            }
            if (collision.gameObject.tag == "Star")
            {
                audioManager.PlaySound(ClipStarPick);
                ParticleBling.Play();
                collision.gameObject.transform.position = new Vector3(GameManager.mathRNG.NextValueFloat(-9, 9), GameManager.mathRNG.NextValueFloat(-5, 5), 0);
                GameManager.AddScore(1);
            }
            if (collision.gameObject.tag == "StarLevel")
            {
                var objRender = collision.gameObject.GetComponent<Renderer>();
                if(objRender.enabled)
                {
                    StartCoroutine(CoroutineStarPick(objRender));
                }
            }
            if (collision.gameObject.tag == "Obstaculo" && !GameManager.IsGameEnd && !GameManager.IsInvencibleMode)
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
        GameManager.AddScore(1);
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
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = .4f;
        audioManager.PlaySound(ClipExplosion);
        ParticleBurst.Play();
        renderer.enabled = false;
        yield return new WaitForSecondsRealtime(0.4f);
        GameManager.GameOver();
        yield return new WaitForSecondsRealtime(3);
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