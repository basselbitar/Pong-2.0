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
    private UIManager _uiManager;

    [SynchronizableMethod]
    public void UpdateP1Score(int score) {
        _p1Score = score;
        p1ScoreText.text = _p1Score.ToString();
        if(_p1Score <= 0 ) {
            _uiManager.ActivateGameOverPanel(1);
        }
    }

    [SynchronizableMethod]
    public void UpdateP2Score(int score) {
        _p2Score = score;
        p2ScoreText.text = _p2Score.ToString();
        if (_p2Score <= 0) {
            _uiManager.ActivateGameOverPanel(0);
        }
    }
}
