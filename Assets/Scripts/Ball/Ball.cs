using Alteruna;
using Unity.VisualScripting;
using UnityEngine;
using Alteruna.Trinity;
using System.Collections;
using System.Collections.Generic;

public class Ball : AttributesSync {
    public int skinIndex;
    [SynchronizableField]
    public float speed;
    private Rigidbody2D _rigidbody;
    //private InterpolationTransformSynchronizable _its;
    private Rigidbody2DSynchronizable _r2Ds;

    private BounceAudioManager _bounceAudioManager;
    private float _minVelocityXThreshold = 4f;

    private float _tweenTime = 1.2f;

    [SerializeField] private ParticleSystem collisionParticlesPrefab;
    private int _lastTouchedBy;

    private TrailRenderer _trailRenderer;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private List<Sprite> _ballSprites;

    [SerializeField]
    private float _magnusEffectFactor;

    public int GetLastTouchedBy() { return _lastTouchedBy; }

    public void SetLastTouchedBy(int lastTouchedIndex) { _lastTouchedBy = lastTouchedIndex; }

    private bool _flashing = false;
    private float _flashDuration = 15f;
    private int _flashNumber = 25;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        //_its = GetComponent<InterpolationTransformSynchronizable>();
        _r2Ds = GetComponent<Rigidbody2DSynchronizable>();
        _bounceAudioManager = FindObjectOfType<BounceAudioManager>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _lastTouchedBy = -1;
    }

    private void Start() {
        ResetPosition();
        //flash the ball
        StartCoroutine(FlashBall(_flashDuration, _flashNumber));
    }

    public void SetMagnusEffectFactor(float magnusEffectFactor) {
        _magnusEffectFactor = magnusEffectFactor;
    }
    public void ResetPosition() {
        _rigidbody.position = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
    }
    public void AddStartingForce() {
        float x = Random.value < 0.5f ? -1.0f : 1.0f;
        float y = Random.value < 0.5f ? Random.Range(-1.0f, -0.5f) : Random.Range(0.5f, 1.0f);

        Vector2 direction = new(x, y);
        //debug mode
        //direction = new Vector2(-1, 0);
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
        if (Mathf.Abs(_rigidbody.velocity.x) < _minVelocityXThreshold) {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x * 1.05f, _rigidbody.velocity.y);
            if(_rigidbody.velocity.x == 0) {
                _rigidbody.velocity = new Vector2(0.05f, _rigidbody.velocity.y);
            }
        }

        if(Mathf.Abs(_rigidbody.velocity.x) + Mathf.Abs(_rigidbody.velocity.y) > 12f) {
            _trailRenderer.enabled = true;
        } else {
            _trailRenderer.enabled = false;
        }

        //if (Input.GetKeyDown(KeyCode.F)) {
        //    _rigidbody.velocity = new Vector2(0f, 0f);
        //}

        //if (Input.GetKeyDown(KeyCode.M)) {
        //    skinIndex++;
        //    skinIndex %= _ballSprites.Count;
        //    SetBallSkin(skinIndex);
        //}

        // Magnus Effect
        AddForce( new Vector2(0,_magnusEffectFactor / 100));

        //flash the ball at the start (to not collect any upgrades before touching a paddle)
        if(_lastTouchedBy >=0) {
            _flashing = false;
            transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        var collisionGO = collision.gameObject;
        TweenBall();
        Quaternion rotation = Quaternion.FromToRotation(Vector2.right, collision.contacts[0].normal);

        ParticleSystem collisionParticles = GetComponentInChildren<ParticleSystem>();
        collisionParticles.transform.SetPositionAndRotation(collision.contacts[0].point, rotation);
        collisionParticles.Play();
        //// spin the ball
        //_rigidbody.AddTorque(10f, ForceMode2D.Force);

        if (_bounceAudioManager == null) {
            return;
        }
        if (collisionGO.CompareTag("Wall")) {
            _bounceAudioManager.OnWallBounce();
        }
        else if (collisionGO.CompareTag("Player")) {
            _lastTouchedBy = collisionGO.GetComponent<Paddle>().id;
            _bounceAudioManager.OnPaddleBounce();
        }
    }

    private void TweenBall() {

        LeanTween.cancel(gameObject);
        transform.localScale = new(0.3f, 0.3f, 1f);

        LeanTween.scale(gameObject, Vector3.one * 0.42f, _tweenTime).setEasePunch();
    }

    [SynchronizableMethod]
    public void SetBallSkin(int skinIndex) {
        if (skinIndex >= _ballSprites.Count) {
            Debug.LogError("Unknown ball index reqested");
            return;
        }
        this.skinIndex = skinIndex;
        GetComponent<SpriteRenderer>().sprite = _ballSprites[skinIndex];
    }

    public IEnumerator FlashBall(float flashDuration, int numberOfFlashes) {
        Color startColor = new(1, 1, 1, 1);
        Color flashColor = new(1, 1, 1, 0.25f);
        float elapsedFlashTime = 0;
        float elapsedFlashPercentage = 0;
        _flashing = true;

        while (elapsedFlashTime < flashDuration && _flashing) {
            elapsedFlashTime += Time.deltaTime;
            elapsedFlashPercentage = elapsedFlashTime / flashDuration;

            if(elapsedFlashPercentage > 1 ) {
                elapsedFlashPercentage = 1;
            }

            float pingPongPercentage = Mathf.PingPong(elapsedFlashPercentage * 2 * numberOfFlashes, 1);
            _spriteRenderer.color = Color.Lerp(startColor, flashColor, pingPongPercentage);
            yield return null;
        }
        _flashing = false;

    }
}
