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
    private float _messUpProbability = 0.05f;

    private Vector2 _intendedDirection;

    enum Difficulty { Easy, Medium, Hard };
    private Difficulty _difficulty;

    void Start() {
        _paddle = GetComponent<Paddle>();
        _lastDrunkTime = Time.realtimeSinceStartup;
        _difficulty = Difficulty.Easy;
        _difficulty = (Difficulty) PlayerPrefs.GetInt("Difficulty");
        SetParameters();
    }
    private void SetParameters() {
        switch (_difficulty) {
            case Difficulty.Easy:
                _actionThreshold = 0.7f;
                _drunkDuration = 2f;
                _drunkResetThreshold = 2f;
                _messUpProbability = 0.2f; // 20% chance to make the wrong decision
                break;
            case Difficulty.Medium:
                _actionThreshold = 0.2f;
                _drunkDuration = 1f;
                _drunkResetThreshold = 4f;
                _messUpProbability = 0.1f;
                break;
            case Difficulty.Hard:
                _actionThreshold = 0.08f;
                _drunkDuration = 0.5f;
                _drunkResetThreshold = 6f;
                _messUpProbability = 0.05f;
                break;
            default:
                Debug.LogError("Difficulty not set");
                break;
        }

        Debug.Log("Parameters have been set to: " + _actionThreshold + ", " + _drunkDuration + ", " + _drunkResetThreshold + ", " + _messUpProbability);
    }

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
        } else {
            _isDrunk = false;
        }

        // random chance to mess up (when not drunk)
        if (!_isDrunk && Random.Range(0f, 1f) < _messUpProbability) {
            if (_intendedDirection == Vector2.up) {
                _intendedDirection = Vector2.down;
            }
            else if (_intendedDirection == Vector2.down) {
                _intendedDirection = Vector2.up;
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
