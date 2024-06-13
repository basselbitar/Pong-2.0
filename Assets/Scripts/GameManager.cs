using Alteruna;
using System.Collections;
using System.Collections.Generic;
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

    public bool debugMode;
    [SerializeField]
    private GameObject[] walls;

    public static GameMode gameMode;

    public void Start() {
        _multiplayer = FindObjectOfType<Multiplayer>();
        _gameStarted = false;
        _ballSpawner = GetComponent<BallSpawner>();
        _scoreManager = FindObjectOfType<ScoreManager>();
        debugMode = true;
        gameMode = null;
    }

    public void Update() {
        if (!IsHostAndReadyToPlay()) {
            return;
        }
        if (!_initialValuesAssigned) {
            AssignInitialValues();
        }

        // spawn the ball
        if (!_gameStarted) {
            _gameStarted = true;
            StartCoroutine(ResetRound());
        }

        if (Input.GetKeyDown(KeyCode.U)) {
            debugMode = !debugMode;
            if (walls.Length == 0) {
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
        StartCoroutine(ResetRound());
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
        StartCoroutine(ResetRound());
    }

    public void Player1GainsLife() {
        _p1Score++;
        _scoreManager.BroadcastRemoteMethod("UpdateP1Score", _p1Score);
    }

    public void Player2GainsLife() {
        _p2Score++;
        _scoreManager.BroadcastRemoteMethod("UpdateP2Score", _p2Score);
    }

    private bool IsHostAndReadyToPlay() {
        //only the host should manage the game
        if (!AmITheHost()) {
            return false;
        }

        if (_multiplayer.CurrentRoom.Users.Count < 2) {
            if (_gameStarted) {
                ResetGame();
            }
            else {
                DestroyRemainingBalls();
                DestroyRemainingUpgrades();
            }
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
        if (_gameFinished) {
            ResetGame();
            return false;
        }

        return true;
    }

    private void DestroyRemainingBalls() {
        Ball[] remainingBalls = FindObjectsOfType<Ball>();
        foreach (var ball in remainingBalls) {
            ball.DestroyBall();
        }
    }

    private void DestroyRemainingUpgrades() {
        Upgrade[] remainingUpgrades = FindObjectsOfType<Upgrade>();
        foreach (var upgrade in remainingUpgrades) {
            upgrade.DestroyUpgrade();
        }
        FindObjectOfType<UpgradeManager>().spawnedUpgrades = new List<GameObject>();
    }

    private void InitializePaddles() {
        var playerList = GameObject.FindGameObjectsWithTag("Player");
        if (playerList.Length < 2) {
            return;
        }

        p1Paddle = playerList[0].GetComponent<Paddle>();
        p2Paddle = playerList[1].GetComponent<Paddle>();

        p1Paddle.id = 0;
        p2Paddle.id = 1;
    }

    public bool AmITheHost() {
        if (_multiplayer.CurrentRoom == null)
            return false;
        return _multiplayer.LowestUserIndex == _multiplayer.Me.Index;
    }

    private void AssignInitialValues() {
        if (_initialValuesAssigned || !IsHostAndReadyToPlay())
            return;

        //TODO: assign the values for the upgrades based on the chosen game mode 
        
        //depending on which paddle the players chooses, they get buffs/nerfs to their stats
        SetPaddleValues(p1Paddle, p1Paddle.GetPaddleTypeIndex());
        SetPaddleValues(p2Paddle, p2Paddle.GetPaddleTypeIndex());

        SetStartingLives();


        _initialValuesAssigned = true;
    }

    private void SetPaddleValues(Paddle p, int paddleTypeIndex) {
        int defaultLives = 3;
        float defaultSpeed = 10f;
        float defaultLength = 0.3f;

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
        p.BroadcastRemoteMethod(nameof(p.ModifyLength));
        p.speed = p.startingSpeed;
    }

    private void SetStartingLives() {
        _p1Score = p1Paddle.startingLives;
        _p2Score = p2Paddle.startingLives;

        _scoreManager.BroadcastRemoteMethod(nameof(_scoreManager.UpdateP1Score), _p1Score);
        _scoreManager.BroadcastRemoteMethod(nameof(_scoreManager.UpdateP2Score), _p2Score);
    }

    private IEnumerator ResetRound() {
        if (!IsHostAndReadyToPlay()) {
            StopCoroutine(ResetRound());
            yield return null;
        }

        yield return new WaitForSeconds(0.4f);
        //if there are more balls, no need to reset the round
        bool outOfBalls = FindObjectsOfType<Ball>().Length < 1; //destroying a ball is slow
        if (outOfBalls) {

            p1Paddle.BroadcastRemoteMethod("ResetPosition");
            p2Paddle.BroadcastRemoteMethod("ResetPosition");

            yield return new WaitForSeconds(0.8f);
            if (IsHostAndReadyToPlay()) {

                _ballGO = _ballSpawner.SpawnBall();
                _ball = _ballGO.GetComponent<Ball>();
                _ball.ResetPosition();
                _ball.AddStartingForce();
                _ball.GetComponent<Rigidbody2DSynchronizable>().enabled = enabled;
            }
        }
    }

    private void ResetGame() {
        DestroyRemainingBalls();
        DestroyRemainingUpgrades();
        _gameStarted = false;
        _gameFinished = false;
        _initialValuesAssigned = false;
        p1Paddle.SetReady(false);
        p2Paddle.SetReady(false);
        p1Paddle = null;
        p2Paddle = null;
    }

    public Paddle GetPaddle1() { return p1Paddle; }
    public Paddle GetPaddle2() { return p2Paddle; }

    //public void PlayerConnected() {
    //    Multiplayer mp = FindObjectOfType<Multiplayer>();
    //    Debug.Log(mp);
    //    foreach (var user in mp.CurrentRoom.Users)
    //    {
    //    Debug.Log(user);

    //    }
    //    Debug.Log("Someone joined the room");
    //}

    public bool IsGamePlaying() {
        return _gameStarted && !_gameFinished;
    }

    public void SetGameFinished(bool gameFinished) {
        _gameFinished = gameFinished;
    }

    public void OnLeaveRoom() {
        if (!AmITheHost()) {
            return;
        }
        ResetGame();
    }
}


