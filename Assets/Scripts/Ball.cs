using Alteruna;
using Unity.VisualScripting;
using UnityEngine;
using Alteruna.Trinity;
using System.Collections;

public class Ball : AttributesSync {
    [SynchronizableField]
    public float speed;
    private Rigidbody2D _rigidbody;
    //private InterpolationTransformSynchronizable _its;
    private Rigidbody2DSynchronizable _r2Ds;

    private BounceAudioManager _bounceAudioManager;
    private float minVelocityXThreshold = 4f;

    private float tweenTime = 1.4f;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        //_its = GetComponent<InterpolationTransformSynchronizable>();
        _r2Ds = GetComponent<Rigidbody2DSynchronizable>();
        _bounceAudioManager = FindObjectOfType<BounceAudioManager>();
    }

    private void Start() {
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
        //debug mode
        direction = new Vector2(-1, 0);
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
        if (Mathf.Abs(_rigidbody.velocity.x) < minVelocityXThreshold) {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x * 1.05f, _rigidbody.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        var collisionGO = collision.gameObject;
        TweenBall();
        GetComponentInChildren<ParticleSystem>().Play();

        if (_bounceAudioManager == null) {
            return;
        }
        if (collisionGO.CompareTag("Wall")) {
            _bounceAudioManager.OnWallBounce();
        }
        else if (collisionGO.CompareTag("Player")) {
            _bounceAudioManager.OnPaddleBounce();
        }
 
    }

    private void TweenBall() {

        LeanTween.cancel(gameObject);
        transform.localScale = new(0.4f, 0.4f, 1f);

        LeanTween.scale(gameObject, Vector3.one * 0.58f, tweenTime).setEasePunch();
    }
}
