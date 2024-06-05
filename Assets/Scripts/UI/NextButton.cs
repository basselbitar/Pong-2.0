using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextButton : MonoBehaviour
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
    }

    public void OnNextClicked() {
        _player.SetReady(true);
        _player.SetPaddleTypeIndex(PaddleOption.selectedOption);
    }
}
