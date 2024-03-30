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


    public int id;

    private Alteruna.Avatar _avatar;
    private Multiplayer _multiplayer;

    private int _lastTouchedBy;

    void Start() {
        _avatar = GetComponent<Alteruna.Avatar>();

        if (!_avatar.IsMe) {
            return;
        }

        Initialize();
        //Debug.Log("My ID is: " + id);
        //Debug.Log("My name is: " + _multiplayer.Me.Name);
        //Debug.Log("Am I the host? " + _multiplayer.Me.IsHost);

    }

    void Update() {
        if (!_avatar.IsMe) {
            return;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            _direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            _direction = Vector2.down;
        }
        else {
            _direction = Vector2.zero;
        }

        //transform.localScale = new Vector3(transform.localScale.x, length, 1f);

    }

    private void FixedUpdate() {
        speed = startingSpeed; //TODO: plus any buffs and minus any nerfs

        if (_direction.sqrMagnitude != 0) {
            _rigidbody.AddForce(_direction * speed);
        }

    }

    private void Initialize() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _multiplayer = FindObjectOfType<Multiplayer>();
        _lastTouchedBy = -1;
        //id = _multiplayer.Me.Index;
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.CompareTag("Ball")) {
            _lastTouchedBy = collision.otherCollider.GetComponent<Paddle>().id;
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
}
