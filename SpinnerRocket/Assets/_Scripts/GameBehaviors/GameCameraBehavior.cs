using UnityEngine;
/**
 * @class
 * @brief Maneja el control de la camara, hace que no se mueva de manera rigida
 */
public class GameCameraBehavior : MonoBehaviour
{
    #region Variables
    [HideInInspector]   private GameManager gameManager;

    #region minValues
    /** @hidden*/   private Vector3 _minValues;
    /** @hidden*/   public Vector3 minValues { get { return _minValues; } }
    #endregion

    #region maxValues
    /** @hidden*/   private Vector3 _maxValues;
    /** @hidden*/   public Vector3 maxValues { get { return _maxValues; } }
    #endregion

    /** Camara que sera manipulada */ 
    public Camera Camera;
    /** Objeto que seguira la camara */ 
    public Transform target;
    /** Tamaño de margen */ 
    public Vector3 offset;
    /** Escala de suavidad de movimiento de la camara */ 
    [Range(0, 10)] public float smoothFactor;
    #endregion

    #region Start & Update
    /** Carga los valores iniciales */
    void Start()
    {
        gameManager = GameManager.GetSingleton();
        _minValues = gameManager.MinValues;
        _maxValues = gameManager.MaxValues;
    }
    /** Actualiza la camara del objetivo */
    void Update()
    {
        if(Camera != null)
        {
            FollowCamera();
        }
    }
    #endregion

    #region General
    /** Metodo que maneja el movimiento de la camara */
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