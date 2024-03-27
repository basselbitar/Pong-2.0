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
    public float speed;
    public int id;

    private Alteruna.Avatar _avatar;
    private Multiplayer _multiplayer;

    void Start() {
        _avatar = GetComponent<Alteruna.Avatar>();

        if (!_avatar.IsMe) {
            return;
        }

        _rigidbody = GetComponent<Rigidbody2D>();
        speed = 15;
        _multiplayer = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Multiplayer>();
        id = _multiplayer.Me.Index;
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

    }

    private void FixedUpdate() {
        if (_direction.sqrMagnitude != 0) {
            _rigidbody.AddForce(_direction * speed);
        }

    }

    public void ResetPosition() {
        _rigidbody.position = new Vector2(_rigidbody.position.x, 0.0f);
        _rigidbody.velocity = Vector2.zero;
    }

    public bool IsReady() {
        return _isPlayerReady;
    }

    public void SetReady(bool ready) {
        _isPlayerReady = ready;
    }
}
