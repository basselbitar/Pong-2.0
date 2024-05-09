using Alteruna;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using static UpgradeData;

public class GameManager : MonoBehaviour {
    private BallSpawner _ballSpawner;
    private ScoreManager _scoreManager;

    private Paddle p1Paddle;
    private Paddle p2Paddle;
    private GameObject _ballGO;
    private Ball _ball;

    private int _p1Score;
    private int _p2Score;
    private Multiplayer _multiplayer;

    private bool _gameStarted;
    private bool _gameFinished;
    private bool _initialValuesAssigned;

    private int _ballTouchedBy;
    public bool debugMode;
    [SerializeField]
    private GameObject[] walls;
    public void Start() {
        _multiplayer = FindObjectOfType<Multiplayer>();
        _gameStarted = false;
        _ballSpawner = GetComponent<BallSpawner>();
        _scoreManager = FindObjectOfType<ScoreManager>();
        debugMode = true;
    }

    public void Update() {
        if (!IsHostAndReadyToPlay()) {
            return;
        }

        if (!_initialValuesAssigned)
        {
            AssignInitialValues();
        }

        // spawn the ball
        if (!_gameStarted) {
            _gameStarted = true;
            ResetRound();
        }

        if (Input.GetKeyDown(KeyCode.U)) {
            debugMode = !debugMode;
            if(walls.Length == 0) {
                walls = GameObject.FindGameObjectsWithTag("Debug");

            }
            foreach (GameObject wall in walls) {
                wall.SetActive(debugMode);
            }
        }
    }
    public void Player1Scores() {
        if (!IsHostAndReadyToPlay()) {
            return;
        }
        _p2Score--;
        _scoreManager.BroadcastRemoteMethod("UpdateP2Score", _p2Score);
        if (_p2Score <= 0) {
            _gameFinished = true;
        }
        ResetRound();
    }

    public void Player2Scores() {
        if (!IsHostAndReadyToPlay()) {
            return;
        }
        _p1Score--;
        _scoreManager.BroadcastRemoteMethod("UpdateP1Score", _p1Score);
        if (_p1Score <= 0) {
            _gameFinished = true;
        }
        ResetRound();
    }

    public void Player1GainsLife() {
        _p1Score++;
        _scoreManager.BroadcastRemoteMethod("UpdateP1Score", _p1Score);
    }

    public void Player2GainsLife() {
        _p2Score++;
        _scoreManager.BroadcastRemoteMethod("UpdateP2Score", _p2Score);
    }

    public void SetTouchedBy(int index) {
        _ballTouchedBy = index;
    }

    private bool IsHostAndReadyToPlay() {
        //only the host should manage the game
        if (!AmITheHost()) {
            return false;
        }

        if (_multiplayer.CurrentRoom.Users.Count < 2) {
            DestroyRemainingBalls();
            return false;
        }

        if (p1Paddle == null || p2Paddle == null) {
            InitializePaddles();
            return false; //skip a frame to make sure the paddles are initialized
        }

        // check if both players are ready
        if (!p1Paddle.IsReady() || !p2Paddle.IsReady()) {
            return false;
        }

        // when game finishes, reset players Readiness
        if(_gameFinished) {
            DestroyRemainingBalls();
            ResetGame();
            return false;
        }

        return true;
    }

    private void DestroyRemainingBalls() {
        Ball remainingBall = FindObjectOfType<Ball>();
        if (remainingBall != null) {
            remainingBall.DestroyBall();
        }
    }

    private void InitializePaddles() {
        var playerList = GameObject.FindGameObjectsWithTag("Player");
        if(playerList.Length < 2) {
            return;
        }

        p1Paddle = playerList[0].GetComponent<Paddle>();
        p2Paddle = playerList[1].GetComponent<Paddle>();

        p1Paddle.id = 0;
        p2Paddle.id = 1;
    }

    private bool AmITheHost() {
        if (_multiplayer.CurrentRoom == null)
            return false;
        return _multiplayer.LowestUserIndex == _multiplayer.Me.Index;
    }

    private void AssignInitialValues() {
        if (_initialValuesAssigned || !IsHostAndReadyToPlay())
            return;

        //depending on which paddle the players chooses, they get buffs/nerfs to their stats
        SetPaddleValues(p1Paddle, p1Paddle.GetPaddleTypeIndex());
        SetPaddleValues(p2Paddle, p2Paddle.GetPaddleTypeIndex());

        SetStartingLives();
        _initialValuesAssigned = true;
    }

    private void SetPaddleValues(Paddle p, int paddleTypeIndex) {
        int defaultLives = 3;
        float defaultSpeed = 10f;
        float defaultLength = 0.1429687f;

        p.startingSpeed = defaultSpeed;
        p.startingLives = defaultLives;
        p.length = defaultLength;

        switch (paddleTypeIndex) {
            case 0: // default values
                // keep the default values of speed, length, and lives
                break;
            case 1: // speed
                p.startingSpeed = defaultSpeed * 5f;
                break;
            case 2: // length
                p.length = defaultLength * 3f;
                break;
            case 3: // lives
                p.startingLives = defaultLives * 2;
                break;
            default:
                Debug.LogError("Unknown paddle selected!");
                break;
        }
        p.BroadcastRemoteMethod("ModifyLength");
        p.speed = p.startingSpeed;
    }

    private void SetStartingLives() {
        _p1Score = p1Paddle.startingLives;
        _p2Score = p2Paddle.startingLives;

        _scoreManager.BroadcastRemoteMethod("UpdateP1Score", _p1Score);
        _scoreManager.BroadcastRemoteMethod("UpdateP2Score", _p2Score);
    }
   
    private void ResetRound() {
        if (!IsHostAndReadyToPlay()) {
            return;
        }
        p1Paddle.BroadcastRemoteMethod("ResetPosition");
        p2Paddle.BroadcastRemoteMethod("ResetPosition");
        _ballGO = _ballSpawner.SpawnBall();
        _ball = _ballGO.GetComponent<Ball>();
        _ball.ResetPosition();
        _ball.AddStartingForce();
        // turn on the ball's Rigidbody2DSynchronizable script on the host's side ONLY
        _ball.GetComponent<Rigidbody2DSynchronizable>().enabled = enabled;

    }

    private void ResetGame() {
        _gameStarted = false;
        _gameFinished = false;
        _initialValuesAssigned = false;
        p1Paddle.SetReady(false);
        p2Paddle.SetReady(false);
    }

public int GetBallTouchedBy() { return _ballTouchedBy; }

    public Paddle GetPaddle1() { return p1Paddle; }
    public Paddle GetPaddle2() {  return p2Paddle; }

    //public void PlayerConnected() {
    //    Multiplayer mp = FindObjectOfType<Multiplayer>();
    //    Debug.Log(mp);
    //    foreach (var user in mp.CurrentRoom.Users)
    //    {
    //    Debug.Log(user);

    //    }
    //    Debug.Log("Someone joined the room");
    //}
}
