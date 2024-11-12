using System.Collections.Generic;
using UnityEngine;
/**
 * @file
 * @brief Genera objetos aleatorios al inicio del escenario
 */
public class SpawnObject : MonoBehaviour
{
    #region Hidden
    private List<GameObject> lstObj;
    private GameManager gameManager;
    #endregion

    #region Editor Variables
    [Header("General")]
    public GameObject obj;
    public int Quantity;
    public enum SpawnType
    {
        Border = 1,
        InScene = 2,
    }
    public SpawnType type;
    [Header("Scene Bounds")]
    public float OffSetUnits;
    #endregion

    #region Methods
    void Start()
    {
        gameManager = GameManager.GetSingleton();
        Spawn();
    }
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