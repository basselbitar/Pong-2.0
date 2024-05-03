using Alteruna;
using Unity.VisualScripting;
using UnityEngine;
using Alteruna.Trinity;

public class Ball : AttributesSync
{
    [SynchronizableField]
    public float speed;
    private Rigidbody2D _rigidbody;
    //private InterpolationTransformSynchronizable _its;
    private Rigidbody2DSynchronizable _r2Ds;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        //_its = GetComponent<InterpolationTransformSynchronizable>();
        _r2Ds = GetComponent<Rigidbody2DSynchronizable>();
    }

    private void Start()
    {
        ResetPosition();
    }

    public void ResetPosition() {
        _rigidbody.position = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;

    }
    public void AddStartingForce() {
        float x = Random.value < 0.5f ? -1.0f : 1.0f;
        float y = Random.value < 0.5f ? Random.Range(-1.0f, -0.5f) : Random.Range(0.5f, 1.0f);

        Vector2 direction = new Vector2(x, y);
        _rigidbody.AddForce(direction * speed);
    }

    public void AddForce(Vector2 force) {
        _rigidbody.AddForce(force);
    }

    public void DestroyBall() {
        Spawner spawner = FindObjectOfType<Spawner>();
        spawner.Despawn(gameObject);
    }

    public void Update() {
        //if(Input.GetKeyUp(KeyCode.L)) {
        //    _its.MovePosition(new Vector2(5f,-2f));

        //}
        //if (Input.GetKeyUp(KeyCode.K)) {
        //    _its.MovePosition(new Vector2(-5f, -2f));
        //}
    }
   
}
