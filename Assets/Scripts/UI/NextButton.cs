using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextButton : MonoBehaviour
{
    private Multiplayer _multiplayer;
    private Paddle _player;

    [SerializeField]
    private List<GameModeOption> _gameModeOptions;

    public void Awake() {
        Initialize();
    }

    //must be called whenever a new room is joined
    public void Initialize() {
        _multiplayer = FindObjectOfType<Multiplayer>();
        _player = GameObject.Find("Paddle (" + _multiplayer.Me.Name + ")").GetComponent<Paddle>();
        _player.SetVoted(false);
        _player.SetSelectedGameModes(new List<int>());
        foreach (var item in _gameModeOptions)
        {
            item.OnGameModeDeselected();
        }
    }

    public void OnNextClicked() {
        _player.SetVoted(true);
        _player.SetSelectedGameModes(GameModeOption.selectedGameModes);
    }
}
