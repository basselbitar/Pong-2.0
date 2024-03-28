using Alteruna;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public TMP_Text p1ScoreText;
    public TMP_Text p2ScoreText;

    private BallSpawner _ballSpawner;

    private Paddle p1Paddle;
    private Paddle p2Paddle;
    private GameObject _ballGO;
    private Ball _ball;

    private int _p1Score;

    private int _p2Score;
    private Multiplayer _multiplayer;

    private bool _gameStarted;

    public void Start() {
        _multiplayer = FindObjectOfType<Multiplayer>();
        _gameStarted = false;
        _ballSpawner = GetComponent<BallSpawner>();
    }

    public void Update() {
        if (!IsHostAndReadyToPlay()) {
            return;
        }

        // spawn the ball
        if(!_gameStarted) {
            _gameStarted = true;
            ResetRound();
            //TODO: set starting health points depending on paddle choice
        }
    }

    private bool IsHostAndReadyToPlay() {
        //only the host should manage the game
        if (!AmITheHost()) {
            return false;
        }

        if (_multiplayer.CurrentRoom.Users.Count < 2) {
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

        return true;
    }

    private void InitializePaddles() {
        var playerList = GameObject.FindGameObjectsWithTag("Player");
        if(playerList.Length < 2) {
            return;
        }

        p1Paddle = playerList[0].GetComponent<Paddle>();
        p2Paddle = playerList[1].GetComponent<Paddle>();
     
    }

    private bool AmITheHost() {
        //if(_multiplayer.CurrentRoom != null) {
        //    foreach (var user in _multiplayer.CurrentRoom.Users)
        //    {
        //        if(_multiplayer.Me.Name == user.Name) {
        //            return user.IsHost;
        //        }
        //    }
        //}
        //return false;
        if (_multiplayer.CurrentRoom == null)
            return false;
        return _multiplayer.LowestUserIndex == _multiplayer.Me.Index;
    }

    public void Player1Scores() {
        _p1Score++;
        p1ScoreText.text = _p1Score.ToString();
        ResetRound();
    }

    public void Player2Scores() {
        _p2Score++;
        p2ScoreText.text = _p2Score.ToString();
        ResetRound();
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
    }

    public void PlayerConnected() {
        Multiplayer mp = FindObjectOfType<Multiplayer>();
        Debug.Log(mp);
        foreach (var user in mp.CurrentRoom.Users)
        {
        Debug.Log(user);
            
        }
        Debug.Log("Someone joined the room");
    }
}
