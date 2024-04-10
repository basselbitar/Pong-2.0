using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugBall : MonoBehaviour
{

    public float forceRight;
    public float forceTop;

    private Ball _ball;
    // Start is called before the first frame update
    void Start()
    {
        _ball = FindObjectOfType<Ball>();
        AddStartingForce();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void AddStartingForce() {
        //float x = Random.value < 0.5f ? -1.0f : 1.0f;
        //float y = Random.value < 0.5f ? Random.Range(-1.0f, -0.5f) : Random.Range(0.5f, 1.0f);
        float x = forceRight;
        float y = forceTop;

        Vector2 direction = new Vector2(x, y);
        _ball.GetComponent<Rigidbody2D>().AddForce(direction * _ball.speed);
    }
}
