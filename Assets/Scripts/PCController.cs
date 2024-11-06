using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCController : MonoBehaviour {
    Ball[] balls;
    Ball closestBall;

    private Paddle _paddle;

    private float _lastDrunkTime;
    private float _drunkResetThreshold = 6f;
    private float _drunkDuration = 0.4f;
    private bool _isDrunk = false;
    [SerializeField]
    private float _actionThreshold = 0.5f;

    private Vector2 _intendedDirection;

    enum Difficulty { Easy, Medium, Hard };
    private Difficulty _difficulty;

    // Start is called before the first frame update
    void Start() {
        _paddle = GetComponent<Paddle>();
        _lastDrunkTime = Time.realtimeSinceStartup;
        _difficulty = Difficulty.Easy;
        Debug.Log(PlayerPrefs.GetInt("Difficulty"));
        _difficulty = (Difficulty)PlayerPrefs.GetInt("Difficulty");
        Debug.Log(_difficulty);
    }

    // Update is called once per frame
    void Update() {
        balls = FindObjectsOfType<Ball>();
        if (balls.Length == 0)
            return;
        closestBall = balls[0];
        foreach (Ball ball in balls) {
            if (ball.GetComponent<Rigidbody2D>().velocity.x < 0) {
                continue;
            }

            if (ball.transform.position.x > closestBall.transform.position.x) {
                closestBall = ball;
            }
        }

        if (transform.position.y > closestBall.transform.position.y) {
            //Debug.Log("Moving down since " + transform.position.y + " > ball position = " + closestBall.transform.position.y);
            _intendedDirection = Vector2.down;
        }
        else if (transform.position.y < closestBall.transform.position.y) {
            //Debug.Log("Moving up since " + transform.position.y + " < ball position = " + closestBall.transform.position.y);
            _intendedDirection = Vector2.up;
        }
        // remove or reduce jitters
        if (Mathf.Abs(transform.position.y - closestBall.transform.position.y) < _actionThreshold) {
            _intendedDirection = Vector2.zero;
        }
        _paddle.SetDirection(_intendedDirection);

        //TODO: program failures at random ... and leeway so that the movement isn't jagged
        if (_paddle.isInvertedControls) {
            if (_lastDrunkTime + _drunkResetThreshold < Time.realtimeSinceStartup) {
                _isDrunk = true;
                _lastDrunkTime = Time.realtimeSinceStartup;
            }

            if (_lastDrunkTime + _drunkDuration < Time.realtimeSinceStartup) {
                _isDrunk = false;
            }
        }

        if (_isDrunk) {
            InvertDirection();
        }
    }

    private void InvertDirection() {
        //flip the intended direction and set that to the paddle
        if (_intendedDirection == Vector2.up) {
            _paddle.SetDirection(Vector2.down);
        }
        else if (_intendedDirection == Vector2.down) {
            _paddle.SetDirection(Vector2.up);
        }
    }
}
