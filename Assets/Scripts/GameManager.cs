using Alteruna;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public Ball ball;

    public Paddle p1Paddle;
    public Paddle p2Paddle;

    public TMP_Text p1ScoreText;
    public TMP_Text p2ScoreText;

    private int _p1Score;

    private int _p2Score;

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
        p1Paddle.ResetPosition();
        p2Paddle.ResetPosition();
        ball.ResetPosition();
        ball.AddStartingForce();
    }

    public void PlayerConnected() {
        Multiplayer mp = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Multiplayer>();
        Debug.Log(mp);
        Debug.Log("Someone joined the room");
    }
}
