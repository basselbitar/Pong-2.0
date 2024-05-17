using Alteruna;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : AttributesSync {
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;

    [SynchronizableField]
    private bool _isPlayerReady;
    [SynchronizableField]
    private int _paddleTypeIndex;

    [SynchronizableField]
    public int startingLives;
    [SynchronizableField]
    public float startingSpeed;
    [SynchronizableField]
    public float speed;
    [SynchronizableField]
    public float length;
    [SynchronizableField]
    public bool isInvertedControls;


    public int id;

    private Alteruna.Avatar _avatar;
    private Multiplayer _multiplayer;
    private GameManager _gameManager;

    private int _lastTouchedBy;

    public bool debug = false;

    void Start() {
        _avatar = GetComponent<Alteruna.Avatar>();

        //debug code
        if (debug) {
            length = 0.2f;
            transform.localScale = new Vector3(transform.localScale.x, length, 1f);
        }


        Initialize();
        //Debug.Log("Am I the host? " + _multiplayer.Me.IsHost);

    }

    void Update() {
        if (!_avatar.IsMe && !debug) {
            return;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            _direction = isInvertedControls ? Vector2.down : Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            _direction = isInvertedControls ? Vector2.up : Vector2.down;
        }
        else {
            _direction = Vector2.zero;
        }

        //transform.localScale = new Vector3(transform.localScale.x, length, 1f);

    }

    private void FixedUpdate() {
        if ((speed == -1)) {
            speed = startingSpeed; //TODO: plus any buffs and minus any nerfs
        }

        if (_direction.sqrMagnitude != 0) {
            _rigidbody.AddForce(_direction * speed);
        }

        if (this.transform.position.y >= 6f || this.transform.position.y <= -6f) {
            Debug.LogError("Paddle has left the building");
            this.BroadcastRemoteMethod("ResetPosition");
        }

    }

    private void Initialize() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _multiplayer = FindObjectOfType<Multiplayer>();
        _gameManager = FindObjectOfType<GameManager>();
        _lastTouchedBy = -1;
        //id = _multiplayer.Me.Index;
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Ball")) {
            Transform ballTransform = collision.collider.transform;
            Rigidbody2D ballRigidBody = ballTransform.GetComponent<Rigidbody2D>();
            float velocityX = ballRigidBody.velocity.x;
            float velocityY = ballRigidBody.velocity.y;
            float velocityBefore = new Vector2(velocityX, velocityY).magnitude;
            float dist = ballTransform.position.y - transform.position.y;
            float normalizedDist = (dist / (length * 10f / 2f)); //due to number of pixels
            Vector2 direction = new(0f, normalizedDist);
            collision.collider.GetComponent<Rigidbody2D>().AddForce(direction * 40);

            float velocityAfter = new Vector2(ballRigidBody.velocity.x, ballRigidBody.velocity.y).magnitude;

            //Debug.Log("Paddle coordinate is: " + transform.position.y);
            //Debug.Log("Normalized dist: " + (dist / (length * 10f / 2f))); //due to number of pixels
            //Debug.Log("Ball touched paddle at: " + dist);
            //Debug.Log("Length of paddle is: " + length);

            //Debug.Log("Ball x velocity is: " + ballRigidBody.velocity.x);
            //Debug.Log("Ball y velocity is: " + ballRigidBody.velocity.y);

            if (_gameManager != null) {
                _lastTouchedBy = collision.otherCollider.GetComponent<Paddle>().id;
                _gameManager.SetTouchedBy(_lastTouchedBy);
            }
        }
    }


    [SynchronizableMethod]
    public void ResetPosition() {
        if (!_avatar.IsMe) {
            return;
        }

        if (_rigidbody == null) {
            Initialize();
        }
        _rigidbody.position = new Vector2(_rigidbody.position.x, 0.0f);
        _rigidbody.velocity = Vector2.zero;
    }

    [SynchronizableMethod]
    public void ModifyLength() {
        if (!_avatar.IsMe) {
            return;
        }
        transform.localScale = new Vector3(transform.localScale.x, length, 1f);
    }

    public bool IsReady() {
        return _isPlayerReady;
    }

    public void SetReady(bool ready) {
        _isPlayerReady = ready;
    }

    public int GetPaddleTypeIndex() {
        return _paddleTypeIndex;
    }
    public void SetPaddleTypeIndex(int paddleTypeIndex) {
        _paddleTypeIndex = paddleTypeIndex;
    }

    [SynchronizableMethod]
    public void BlowWind(bool direction) {
        FindObjectOfType<EffectsManager>().PlayWindEffect(direction);
    }
}
