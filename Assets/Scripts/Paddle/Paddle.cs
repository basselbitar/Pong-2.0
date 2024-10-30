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
    private bool _hasPlayerVoted;
    [SynchronizableField]
    private List<int> _selectedGameModes;

    [SynchronizableField]
    public int startingLives;
    [SynchronizableField]
    public float startingSpeed;
    [SynchronizableField]
    public float speed;
    [SynchronizableField]
    public float length;
    [SynchronizableField]
    public bool isInvertedControls;


    public int id;
    private readonly float _tweenTime = 1.2f;

    private Alteruna.Avatar _avatar;
    private Multiplayer _multiplayer;
    private GameManager _gameManager;
    private GameModeManager _gameModeManager;

    // from 0 to 100 denoting how much velocity should increase per bounce. ie  velocity * (1 + x/100)
    private readonly float _defaultBounceMultiplier = 5f;
    // multiplier factor that applies to the defaultBounceMultipler before adding 1. ie (1 + (x * y)/100)
    private float _bounceMultiplierFactor = 1f;

    public bool debug = false;

    [SerializeField]
    private Sprite[] skins;
    private int currentSkinIndex;

    private bool _isTweening;
    void Start() {
        _avatar = GetComponent<Alteruna.Avatar>();
        //debug code
        if (debug) {
            length = 0.3f;
            transform.localScale = new Vector3(transform.localScale.x, length, 1f);
        }
        _selectedGameModes = new();
        Initialize();
    }

    void Update() {
        if (!_avatar.IsMe && !debug && PlayMode.IsOnline) {
            return;
        }

        if (Input.GetKeyUp(KeyCode.T)) {
            TweenPaddle();
        }

        HandleInputs();
        //transform.localScale = new Vector3(transform.localScale.x, length, 1f);
    }

    private void FixedUpdate() {
        if ((speed == -1)) {
            speed = startingSpeed; //TODO: plus any buffs and minus any nerfs
        }
        if (_direction.sqrMagnitude != 0) {
            _rigidbody.AddForce(speed * _direction);
        }

        if (this.transform.position.y >= 6f || this.transform.position.y <= -6f) {
            Debug.Log("Out of bounds paddle");
            if (!this._isTweening) {
                this.BroadcastRemoteMethod(nameof(ResetPosition));
            }
        }

        int skinIndex = GetSkinIndex();
        if (skinIndex != currentSkinIndex) {
            currentSkinIndex = skinIndex;
            this.BroadcastRemoteMethod(nameof(ModifySkin), skinIndex);
        }
    }

    private void Initialize() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _multiplayer = FindObjectOfType<Multiplayer>();
        _gameManager = FindObjectOfType<GameManager>();
        _gameModeManager = FindObjectOfType<GameModeManager>();
        //id = _multiplayer.Me.Index;
    }

    private void HandleInputs() {
        if (PlayMode.selectedPlayMode == PlayMode.PlayModeType.PlayVsPC && id == 1)
            return;
        //for single player vs CPU and for multiplayer
        if (PlayMode.IsOnline || PlayMode.selectedPlayMode == PlayMode.PlayModeType.PlayVsPC) { 
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                _direction = isInvertedControls ? Vector2.down : Vector2.up;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                _direction = isInvertedControls ? Vector2.up : Vector2.down;
            }
            else {
                _direction = Vector2.zero;
            }
        } else {
            //for local multiplayer  W,S  moves p1  ... Up,Down moves p2
            if ((Input.GetKey(KeyCode.W) && id == 0) || (Input.GetKey(KeyCode.UpArrow) && id == 1)) {
                _direction = isInvertedControls ? Vector2.down : Vector2.up;
            } else if ((Input.GetKey(KeyCode.S) && id == 0) || (Input.GetKey(KeyCode.DownArrow) && id == 1)) {
                _direction = isInvertedControls ? Vector2.up : Vector2.down;
            } else {
                _direction = Vector2.zero;
            }
        }
    }

    private int GetSkinIndex() {

        if (isInvertedControls) {
            return 1;
        }

        //check if game hasn't started yet, stick to default skin
        if (startingSpeed == 0) {
            return 0;
        }
        if (speed > startingSpeed * 1.01) {
            return 2;
        }
        else if (speed < startingSpeed * 0.99) {
            return 3;
        }

        return 0;
    }

    public void SetDirection(Vector2 direction) {
        _direction = direction;
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Ball")) {
            Transform ballTransform = collision.collider.transform;
            Rigidbody2D ballRigidBody = ballTransform.GetComponent<Rigidbody2D>();
            float velocityX = ballRigidBody.velocity.x;
            float velocityY = ballRigidBody.velocity.y;
            //float velocityBefore = new Vector2(velocityX, velocityY).magnitude;
            float dist = ballTransform.position.y - transform.position.y;
            float normalizedDist = (dist / (length * 10f / 2f)); //due to number of pixels
            Vector2 direction = new(0f, normalizedDist);
            collision.collider.GetComponent<Rigidbody2D>().AddForce(direction * 40);
            //Debug.Log("Velocity before = " + ballRigidBody.velocity.x + ", " + ballRigidBody.velocity.y);
            //Debug.Log("Multiplying by a factor of " + (1 + (_defaultBounceMultiplier * _bounceMultiplierFactor) / 100));

            //ball velocity increases per bounce and is clamped by 30 on each axis
            float newVelocityX = Mathf.Clamp(velocityX * (1 + (_defaultBounceMultiplier * _bounceMultiplierFactor)/100), -30f, 30f);
            float newVelocityY = Mathf.Clamp(velocityY * (1 + (_defaultBounceMultiplier * _bounceMultiplierFactor)/100), -30f, 30f);

            ballRigidBody.velocity = new Vector2(newVelocityX, newVelocityY);

            //Debug.Log("Velocity after = " + ballRigidBody.velocity.x + ", " + ballRigidBody.velocity.y);


            float paddleVelocityY = this.transform.GetComponent<Rigidbody2D>().velocity.y;
            float paddlePositionX = this.transform.position.x;
            float ballPositionX = ballTransform.position.x;

            float torqueAmount = Mathf.Clamp(10f * paddleVelocityY, -20, 20);

            // spin the ball
            if (paddlePositionX < ballPositionX) {
                ballRigidBody.AddTorque(-1 * torqueAmount, ForceMode2D.Force);
            }
            else {
                ballRigidBody.AddTorque(torqueAmount, ForceMode2D.Force);
            }

            //Debug.Log("Paddle coordinate is: " + transform.position.y);
            //Debug.Log("Normalized dist: " + (dist / (length * 10f / 2f))); //due to number of pixels
            //Debug.Log("Ball touched paddle at: " + dist);
            //Debug.Log("Length of paddle is: " + length);

            //Debug.Log("Ball x velocity is: " + ballRigidBody.velocity.x);
            //Debug.Log("Ball y velocity is: " + ballRigidBody.velocity.y);w
        }
    }


    [SynchronizableMethod]
    public void ResetPosition() {
        if (!_avatar.IsMe && PlayMode.IsOnline) {
            return;
        }

        if (_rigidbody == null) {
            Initialize();
        }
        if (!_isTweening) {
            TweenPaddle();
        }
    }

    [SynchronizableMethod]
    public void ModifyLength() {
        if (!_avatar.IsMe && PlayMode.IsOnline) {
            return;
        }
        LeanTween.scale(gameObject, new(0.2f, length, 1f), 0.6f).setEaseInBack();
    }

    [SynchronizableMethod]
    public void ModifySkin(int index) {
        GetComponent<SpriteRenderer>().sprite = skins[index];
    }

    [SynchronizableMethod]
    public void SetBounceMultiplierFactor(float bmf) {
        _bounceMultiplierFactor = bmf;
    }

    // getters and setters
    public bool IsReady() {
        return _isPlayerReady;
    }

    public void SetReady(bool ready) {
        _isPlayerReady = ready;
    }

    public bool HasVoted() {
        return _hasPlayerVoted;
    }

    public void SetVoted(bool voted) {
        _hasPlayerVoted = voted;
    }

    public void SetSelectedGameModes(List<int> selectedGameModes) {
        _selectedGameModes = selectedGameModes;
    }

    public List<int> GetSelectedGameModes() {
        return _selectedGameModes;
    }

    public int GetPaddleTypeIndex() {
        return _paddleTypeIndex;
    }
    public void SetPaddleTypeIndex(int paddleTypeIndex) {
        _paddleTypeIndex = paddleTypeIndex;
    }
    // end of getters and setters


    [SynchronizableMethod]
    public void BlowWind(bool direction) {
        FindObjectOfType<EffectsManager>().PlayWindEffect(direction);
    }

    [SynchronizableMethod]
    public void SetGameModePool(List<GameMode> gameModes) {
        //Debug.LogError(gameModes[0].GetName());
        //Debug.LogError(gameModes[1].GetName());
        //Debug.LogError(gameModes[2].GetName());
        //if (!_avatar.IsMe) {
        //    return;
        //}
        if (gameModes == null || gameModes.Count == 0)
            return;

        if (_gameModeManager == null)
            _gameModeManager = FindObjectOfType<GameModeManager>();

        _gameModeManager.SetGameModePool(gameModes);
    }

    [SynchronizableMethod]
    public void SetChosenGameMode(GameMode gameMode) {
        _gameModeManager.UpdateChosenGameMode(gameMode);
    }

    private void TweenPaddle() {
        _isTweening = true;
        LeanTween.cancel(gameObject);
        transform.localScale = new(0.2f, length, 1f);

        LeanTween.scale(gameObject, new(0, 0, 0), 0.3f).setEaseInBack().setOnComplete(() => {
            _rigidbody.position = new Vector2(_rigidbody.position.x, 0.0f);
            _rigidbody.velocity = Vector2.zero;

            LeanTween.scale(gameObject, new(0.2f, length, 1f), 0.3f).setEaseInBack().setOnComplete(() => {
                //transform.localScale = new(0.01187452f, length, 1f);
                LeanTween.scale(gameObject, new(0.3f, length * 1.3f, 1f), _tweenTime).setEasePunch().setOnComplete(() => { _isTweening = false; });
            });
        });

        //LeanTween.scale(gameObject, new(0f, 0f, 0f), 3f * _tweenTime).setEasePunch();
    }
}
