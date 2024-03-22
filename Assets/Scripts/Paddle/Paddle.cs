using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private bool _isPlayerReady;
    public float speed;
    public int id;


    private Alteruna.Avatar _avatar;

    void Start() {
        _avatar = GetComponent<Alteruna.Avatar>();

        if (!_avatar.IsMe) {
            return;
        }

        _rigidbody = GetComponent<Rigidbody2D>();
        speed = 15;
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
