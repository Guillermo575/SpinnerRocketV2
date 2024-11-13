using System.Collections.Generic;
using UnityEngine;
/**
 * @class
 * @brief Genera objetos aleatorios al inicio del escenario
 */
public class SpawnObject : MonoBehaviour
{
    #region Hidden
    private List<GameObject> lstObj;
    private GameManager gameManager;
    #endregion

    #region Editor Variables
    /** Objeto que replicara la clase */
    [Header("General")]
    public GameObject obj;
    /** Cantidad de objetos que se va a crear */
    public int Quantity;
    /** 1: El objeto se creara en los bordes, 2: Se creara dentro del scenario */
    public enum SpawnType
    {
        Border = 1,
        InScene = 2,
    }
    /** 1: Indica donde se creara el objeto */
    public SpawnType type;
    /** Unidades de margen donde se creara en caso de elegir que el objeto se creara por los bordes */
    [Header("Scene Bounds")]
    public float OffSetUnits;
    #endregion

    #region Methods
    /** Metodo de inicio del escenario */
    void Start()
    {
        gameManager = GameManager.GetSingleton();
        Spawn();
    }
    /** Metodo que creara los objetos */
    public void Spawn()
    {
        lstObj = new List<GameObject>();
        var min = new Vector2(gameManager.MinValues.x + OffSetUnits, gameManager.MinValues.y + OffSetUnits);
        var max = new Vector2(gameManager.MaxValues.x - OffSetUnits, gameManager.MaxValues.y - OffSetUnits);
        for (int i = 0; i < Quantity; i++)
        {
            Vector2 position = new Vector2();
            switch(type)
            {
                case SpawnType.Border: position = gameManager.mathRNG.getRandomSpawnPoint(min, max); break;
                case SpawnType.InScene: position = new Vector2(gameManager.mathRNG.NextValueFloat(min.x, max.x), gameManager.mathRNG.NextValueFloat(min.y, max.y)); break;
            }
            lstObj.Add(Instantiate(obj, position, Quaternion.identity));
        }
    }
    #endregion
}