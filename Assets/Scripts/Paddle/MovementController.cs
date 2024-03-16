using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {
    private Rigidbody2D paddleBody;
    public float speed;

    public float maxSpeed;

    public float acceleration;

    void Start() {
        paddleBody = GetComponent<Rigidbody2D>();
        speed = 0;
        maxSpeed = 5;
        acceleration = 150;
    }

    void Update() {

        if (Input.GetKey(KeyCode.UpArrow)) {
            speed += acceleration * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            speed += -acceleration * Time.deltaTime;
        }


        transform.position += new Vector3(0, speed * Time.deltaTime, 0);

        //adding force makes it sluggish. Next, I'll attempt to make the speed instantly change the position of the paddle
        //if (Input.GetKey(KeyCode.UpArrow)) {
        //    paddleBody.AddForce(new Vector2(0, speed * Time.deltaTime), ForceMode2D.Impulse);

        //}
        //if (Input.GetKey(KeyCode.DownArrow)) {
        //    paddleBody.AddForce(new Vector2(0, -speed * Time.deltaTime), ForceMode2D.Impulse);
        //}
    }

}
