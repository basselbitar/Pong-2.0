using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleSpeedSetter : MonoBehaviour
{
    private Paddle p;
    public void Start()
    {
        p = gameObject.GetComponent<Paddle>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SetSpeed(100);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SetSpeed(200);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SetSpeed(300);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            SetSpeed(400);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            SetSpeed(500);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            SetSpeed(600);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) {
            SetSpeed(700);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8)) {
            SetSpeed(800);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            SetSpeed(900);
        }

    }

    public void SetSpeed(float speed) {
        p.speed = speed;
        Debug.Log("Speed set to " + p.speed);

    }
}
