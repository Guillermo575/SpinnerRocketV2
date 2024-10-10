using UnityEngine;

public class BlackHole : MonoBehaviour
{
    #region Variables
    [HideInInspector] public new Transform transform;
    [HideInInspector] public GameManager gameManager;
    public float SpeedTraction = 1;
    #endregion

    #region Start & update
    void Start()
    {
        gameManager = GameManager.GetSingleton();
        transform = GetComponent<Transform>();
    }
    void Update()
    {
        transform.Rotate(0, 0, -(5 * Time.deltaTime));
    }
    #endregion

    #region General
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
