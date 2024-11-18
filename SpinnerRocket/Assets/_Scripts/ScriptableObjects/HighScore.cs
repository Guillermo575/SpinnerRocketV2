using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/**
 * @class
 * @brief Aqui se gestiona las puntuaciones mas altas
 */
[CreateAssetMenu(fileName = "Opciones", menuName = "Tools/HighScore", order = 1)]
public class HighScore : ObjetoPersistente
{
    /** Objeto persistente que contiene las puntuaciones mas altas */
    public List<Score> lstscores = new List<Score>();

    /**
     * Obtiene la puntuacion del nivel
     * @param name: nombre del escenario
     * @return float
     */
    public float GetScore(string name)
    {
        var score = (from x in lstscores where x.name == name select x).ToList();
        return score.Count == 0 ? -1 : score[0].BestScore;
    }
    /**
     * Establece la puntuacion actual
     * @param newscore: Objeto que guarda el nombre del escenario y la puntuacion
     */
    public void SetScore(Score newscore)
    {
        var score = (from x in lstscores where x.name == newscore.name select x).ToList();
        if (score.Count == 0)
        {
            lstscores.Add(newscore);
        }
        else
        {
            score.First().BestScore = newscore.BestScore;
        }
    }
    [Serializable]
    public class Score
    {
        public string name;
        public float BestScore = 0;
    }
}