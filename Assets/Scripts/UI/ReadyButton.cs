using Alteruna;
using UnityEngine;

public class ReadyButton : MonoBehaviour
{
    private Multiplayer _multiplayer;
    private Paddle _player;
    private Paddle _player2Local;
    [SerializeField]
    private NextButton nextButton;

    public void Awake() {
        Initialize();
    }

    //must be called whenever a new room is joined
    public void Initialize() {
        Debug.Log("Initialize ready button");
        if (!PlayMode.IsOnline) {
            InitializeLocal();
            return;
        }

        _multiplayer = FindObjectOfType<Multiplayer>();
        _player = GameObject.Find("Paddle (" + _multiplayer.Me.Name + ")").GetComponent<Paddle>();
        _player.SetReady(false);
    }

    public void InitializeLocal() {
        _player = nextButton.player;
        _player2Local = nextButton.player2Local;

        _player.SetReady(false);
        _player2Local.SetReady(false);

        if(PlayMode.selectedPlayMode == PlayMode.PlayModeType.PlayVsPC) {
            _player2Local.SetReady(true);
        }
    }

    public void OnReadyClicked() {
        if(PlayMode.selectedPlayMode == PlayMode.PlayModeType.PlayLocal) {
            // if first player is not ready, set him to ready and switch to next player
            if(!_player.IsReady()) {
                _player.SetReady(true);
                _player.SetPaddleTypeIndex(PaddleOption.selectedOption);
                FindObjectOfType<RoomHandler>().SetPlayer2Turn();
                //FindObjectOfType<TweenUIManager>().HidePaddleSelectorButtons();
                FindObjectOfType<TweenUIManager>().ShowPaddleSelectorButtons();
            } else {
                _player2Local.SetReady(true);
                _player2Local.SetPaddleTypeIndex(PaddleOption.selectedOption);
                FindObjectOfType<TweenUIManager>().PlayGame();
            }
        } else {
            _player.SetReady(true);
            _player.SetPaddleTypeIndex(PaddleOption.selectedOption);
            FindObjectOfType<TweenUIManager>().PlayGame();
        }
    }
}
