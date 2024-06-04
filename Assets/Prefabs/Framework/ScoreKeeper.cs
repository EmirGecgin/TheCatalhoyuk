using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public delegate void OnScoreChanged(int newVal);

    public event OnScoreChanged onScoreChanged;
    private int score;

    public void ChangeScore(int amt)
    {
        score += amt;
        if (score < 0)
        {
            score = 0;
        }
        //Debug.Log($"the score has changed to {score}");
        onScoreChanged?.Invoke(score);
    }
}
