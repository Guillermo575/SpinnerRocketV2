using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/**
 * @file
 * @brief Aqui se gestiona las puntuaciones mas altas
 */
[CreateAssetMenu(fileName = "Opciones", menuName = "Tools/HighScore", order = 1)]
public class HighScore : ObjetoPersistente
{
    public List<Score> lstscores = new List<Score>();

    public float GetScore(string name)
    {
        var score = (from x in lstscores where x.name == name select x).ToList();
        return score.Count == 0 ? -1 : score[0].BestScore;
    }
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