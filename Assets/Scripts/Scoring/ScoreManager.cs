using Alteruna;
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
    [SerializeField]
    private TweenUIManager _tweenUIManager;

    [SynchronizableMethod]
    public void UpdateP1Score(int score) {
        _p1Score = score;
        _tweenUIManager.TweenScoreUI(p1ScoreText, _p1Score.ToString());
        
        if(_p1Score <= 0 ) {
            _tweenUIManager.ActivateGameOverPanel(1);
        }
    }

    [SynchronizableMethod]
    public void UpdateP2Score(int score) {
        _p2Score = score;
        _tweenUIManager.TweenScoreUI(p2ScoreText, _p2Score.ToString());
        if (_p2Score <= 0) {
            _tweenUIManager.ActivateGameOverPanel(0);
        }
    }
}
