using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    public float speed;
    public int id;

    void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();
        speed = 15;
    }

    void Update() {
        //left paddle
        if (id == 0) {

            if (Input.GetKey(KeyCode.W)) {
                _direction = Vector2.up;
            }
            else if (Input.GetKey(KeyCode.S)) {
                _direction = Vector2.down;
            }
            else {
                _direction = Vector2.zero;
            }
        }

        //right paddle (testing purposes)
        if (id == 1) {
            if (Input.GetKey(KeyCode.UpArrow)) {
                _direction = Vector2.up;
            }
            else if (Input.GetKey(KeyCode.DownArrow)) {
                _direction = Vector2.down;
            } else {
                _direction = Vector2.zero;
            }
        }

    }

    private void FixedUpdate() {
        if (_direction.sqrMagnitude != 0) {
            _rigidbody.AddForce(_direction * speed);
        }

    }

}
