using Alteruna;
using UnityEngine;

public class ReadyButton : MonoBehaviour
{
    private Multiplayer _multiplayer;
    private Paddle _player;

    public void Awake() {
        Initialize();
    }

    //must be called whenever a new room is joined
    public void Initialize() {
        _multiplayer = FindObjectOfType<Multiplayer>();
        _player = GameObject.Find("Paddle (" + _multiplayer.Me.Name + ")").GetComponent<Paddle>();
        _player.SetReady(false);
    }

    public void OnReadyClicked() {
        _player.SetReady(true);
    }
}
