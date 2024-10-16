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

    [SerializeField]
    private ScoreAudioManager _scoreAudioManager;

    [SerializeField]
    private Multiplayer _multiplayer;

    private bool _isVsPC;
    private bool _isLocalMultiplayer;
    private bool _isOnline;
    private int _myIndex;

    public void Start() {
        _multiplayer = FindObjectOfType<Multiplayer>();
    }

    [SynchronizableMethod]
    public void UpdateP1Score(int score, bool withSound) {
        _p1Score = score;
        _tweenUIManager.TweenScoreUI(p1ScoreText, _p1Score.ToString());

        if(withSound) {
            PlayAppropriateScoreSound(1);
        }

        if (_p1Score <= 0 ) {
            _tweenUIManager.ActivateGameOverPanel(1);
        }

    }

    [SynchronizableMethod]
    public void UpdateP2Score(int score, bool withSound) {
        _p2Score = score;
        _tweenUIManager.TweenScoreUI(p2ScoreText, _p2Score.ToString());
        
        if(withSound) {
            PlayAppropriateScoreSound(0);
        }

        if (_p2Score <= 0) {
            _tweenUIManager.ActivateGameOverPanel(0);
        }
    }

    private void GetPlayMode() {
        _isVsPC = PlayMode.selectedPlayMode == PlayMode.PlayModeType.PlayVsPC;
        _isLocalMultiplayer = PlayMode.selectedPlayMode == PlayMode.PlayModeType.PlayLocal;
        _isOnline = PlayMode.selectedPlayMode == PlayMode.PlayModeType.PlayOnline;
        if (_isVsPC) {
            _myIndex = 0;
        }
        if ( _isOnline) {
            //get the index of our player
            _myIndex = _multiplayer.Me.Index;
            Debug.LogError("My index: " +  _myIndex);
        }
    }

    private void PlayAppropriateScoreSound(int indexOfScorer) {
        GetPlayMode();
        if (_isLocalMultiplayer) {
            _scoreAudioManager.PlayScoreSound();
        }
        else {
            // same functionality if vs PC or vs another player
            // index will be either 0 or 1
            if (_myIndex == indexOfScorer) {
                _scoreAudioManager.PlayScoreSound();
            }
            else {
                _scoreAudioManager.PlayScoredAtSound();
            }
        }
    }
}
