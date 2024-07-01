using Alteruna;
using System.Collections.Generic;
using UnityEngine;

public class NextButton : MonoBehaviour
{
    private Multiplayer _multiplayer;
    public Paddle player;
    public Paddle player2Local;

    [SerializeField]
    private GameObject PlayerPrefab;

    [SerializeField]
    private Transform _p1position, _p2position;

    [SerializeField]
    private List<GameModeOption> _gameModeOptions;

    [SerializeField]
    private ReadyButton _readyButton;
    [SerializeField]
    private PaddleOption _paddleOption;
    public void Awake() {
        Initialize();
    }

    //must be called whenever a new room is joined
    public void Initialize() {

        if(!PlayMode.IsOnline) {
            InitializeLocal();
            return;
        }

        _multiplayer = FindObjectOfType<Multiplayer>();
        player = GameObject.Find("Paddle (" + _multiplayer.Me.Name + ")").GetComponent<Paddle>();
        player.SetVoted(false);
        player.SetSelectedGameModes(new List<int>());
        foreach (var item in _gameModeOptions)
        {
            item.OnGameModeDeselected();
        }
    }

    public void InitializeLocal() {
        if(player == null) {
            //Spawn Player 1
            player = Instantiate(PlayerPrefab, _p1position.position, Quaternion.identity).GetComponent<Paddle>();
            Destroy(player.gameObject.GetComponent<TransformSynchronizable2D>());
            player.id = 0;

            //Spawn Player 2
            GameObject player2GO = Instantiate(PlayerPrefab, _p2position.position, Quaternion.identity);
            Destroy(player2GO.GetComponent<TransformSynchronizable2D>());
            player2Local = player2GO.GetComponent<Paddle>();
            player2Local.SetVoted(true);
            player2Local.id = 1;

            _multiplayer = FindObjectOfType<Multiplayer>();
            player.SetVoted(false);
            player.SetSelectedGameModes(new List<int>());
            foreach (var item in _gameModeOptions) {
                item.OnGameModeDeselected();
            }
        }
    }

    public void OnNextClicked() {
        player.SetVoted(true);
        player.SetSelectedGameModes(GameModeOption.selectedGameModes);
        _readyButton.Initialize();
        _paddleOption.ClearOptions();
        if (!PlayMode.IsOnline) {
            player2Local.SetVoted(true);
            player2Local.SetSelectedGameModes(GameModeOption.selectedGameModes);
        }
    }
}
