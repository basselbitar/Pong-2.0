using Alteruna;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : AttributesSync
{
    [SynchronizableField]
    private int _p1Score;
    [SynchronizableField]
    private int _p2Score;

    [SerializeField]
    private TMP_Text p1ScoreText;
    [SerializeField]
    private TMP_Text p2ScoreText;

    [SynchronizableMethod]
    public void UpdateP1Score(int score) {
        _p1Score = score;
        p1ScoreText.text = _p1Score.ToString();
    }

    [SynchronizableMethod]
    public void UpdateP2Score(int score) {
        _p2Score = score;
        p2ScoreText.text = _p2Score.ToString();
    }
}
