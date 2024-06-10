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
        _player.SetVoted(false);
        _player.SetGameModePreference(new List<int>());
    }

    public void OnNextClicked() {
        _player.SetVoted(true);
        _player.SetGameModePreference(GameModeOption.selectedGameModes);
    }
}
