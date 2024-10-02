using UnityEngine;
public class GameCameraBehavior : MonoBehaviour
{
    #region Variables
    [HideInInspector] public Vector3 minValues;
    [HideInInspector] public Vector3 maxValues;
    public Camera Camera;
    public Transform target;
    public Vector3 offset;
    [Range(0, 10)] public float smoothFactor;
    private GameManager gameManager;
    #endregion

    #region Start & Update
    void Start()
    {
        gameManager = GameManager.GetSingleton();
        minValues = gameManager.MinValues;
        maxValues = gameManager.MaxValues;
    }
    void Update()
    {
        if(Camera != null)
        {
            FollowCamera();
        }
    }
    #endregion

    #region General
    public void FollowCamera()
    {
        float height = 2f * Camera.orthographicSize;
        float width = height * Camera.aspect;
        Vector3 targetPosition = (target == null ? new Vector3(0,0) : target.position) + offset;
        Vector3 boundPosition = new Vector3(Mathf.Clamp(targetPosition.x, minValues.x + (width / 2), maxValues.x - (width / 2)), Mathf.Clamp(targetPosition.y, minValues.y + (height / 2), maxValues.y - (height / 2)), Camera.transform.position.z);
        Vector3 smoothPosition = Vector3.Lerp(Camera.transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        Camera.transform.position = (smoothFactor == 0) ? boundPosition : smoothPosition;
    }
    #endregion
}