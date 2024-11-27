using Alteruna;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class GameModeManager : MonoBehaviour {
    public List<GameMode> gameModes;
    private List<GameMode> _gameModePool;

    private GameManager _gameManager;
    private Paddle p1, p2;

    private bool _gameModePoolIsSet;
    private bool _gameModeConfirmed;
    private bool _waiting;
    private GameMode _chosenGameMode;
    [SerializeField]
    private string _chosenGameModeName; //debugging purposes

    private UpgradeManager _upgradeManager;

    [SerializeField]
    private TMP_Text gameModeText;

    [SerializeField]
    private List<TMP_Text> _gameModeOptionTexts;

    [SerializeField]
    private List<Image> _gameModeOptionImages;

    private Multiplayer _multiplayer;

    [SerializeField]
    private GameModeOption _gameModeOption;

    void Start() {
        PopulateGameModes();
        Initialize();
    }

    public void Initialize() {
        _gameManager = FindObjectOfType<GameManager>();
        _upgradeManager = FindObjectOfType<UpgradeManager>();
        _multiplayer = FindObjectOfType<Multiplayer>();

        gameModeText.text = "Game Mode: ?";
        _gameModeConfirmed = false;
        _gameModePoolIsSet = false;
        _waiting = false;
        _gameModePool = new();
        _gameModeOptionTexts[0].text = "";
        _gameModeOptionTexts[1].text = "";
        _gameModeOptionTexts[2].text = "";
    }

    private void PopulateGameModes() {
        gameModes = new List<GameMode>();

        // basic game mode: Original design of the game where everything is balanced
        int[] upgradeValues = new int[17] { 4, 4, 1, 3, 1, 2, 2, 2, 2, 1, 2, 1, 2, 1, 1, 1, 1 };
        GameMode Balanced = new("Balanced", upgradeValues, 1.0f);
        gameModes.Add(Balanced);

        // Stormy Night: Game mode where there's lots of "wind" and "wind against player"
        upgradeValues = new int[17] { 3, 2, 1, 1, 1, 0, 10, 0, 0, 0, 6, 2, 3, 3, 2, 1, 2 };
        GameMode Stormy = new("Stormy Night", upgradeValues, 0.6f);
        gameModes.Add(Stormy);

        // Angelic: All buffs and good neutrals 
        upgradeValues = new int[17] { 4, 3, 1, 3, 3, 2, 3, 0, 0, 0, 0, 0, 2, 4, 0, 0, 1 };
        GameMode Angelic = new("Angelic", upgradeValues, 2.0f);
        gameModes.Add(Angelic);

        // Mine Field/Demonic: All the debuffs and negative neutrals
        upgradeValues = new int[17] { 0, 0, 0, 0, 0, 0, 0, 4, 2, 2, 2, 1, 0, 0, 1, 1, 1 };
        GameMode MineField = new("Mine Field", upgradeValues, 1.0f);
        gameModes.Add(MineField);

        // Ball Mania: Lots of split balls and bonus lives
        upgradeValues = new int[17] { 3, 3, 6, 2, 1, 2, 1, 0, 0, 0, 0, 2, 2, 1, 1, 1, 10 };
        GameMode BallMania = new("Ball Mania", upgradeValues, 0.4f);
        gameModes.Add(BallMania);

        // Drunken Brawl: Lots of “Flip Enemy Controls”, “Flip Controls”, “Flip Both Controls”
        upgradeValues = new int[17] { 2, 2, 1, 2, 2, 10, 2, 2, 2, 6, 2, 0, 0, 0, 0, 8, 1 };
        GameMode DrunkenBrawl = new("Drunken Brawl", upgradeValues, 0.6f);
        gameModes.Add(DrunkenBrawl);

        // Speed Racer: Lots of “Faster Paddle” and “Faster Both Paddles” and shorter
        upgradeValues = new int[17] { 0, 5, 1, 4, 0, 1, 1, 4, 0, 1, 0, 0, 0, 5, 0, 0, 1 };
        GameMode SpeedRacer = new("Speed Racer", upgradeValues, 1.5f);
        gameModes.Add(SpeedRacer);

        // Dumbo Style: Lots of Slowness and Elongation
        upgradeValues = new int[17] { 5, 0, 0, 0, 4, 0, 0, 0, 2, 0, 0, 0, 5, 0, 3, 0, 0 };
        GameMode Dumbo = new("Dumbo", upgradeValues, 1.0f);
        gameModes.Add(Dumbo);

        // We’re in this together: Heavier stats for neutral upgrades rather than buffs/debuffs
        upgradeValues = new int[17] { 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 6, 6, 6, 6, 4, 3 };
        GameMode Together = new("We're in this Together", upgradeValues, 1.0f);
        gameModes.Add(Together);

        // Classic: No upgrades, but ball gets faster faster. TODO: how?
        upgradeValues = new int[17] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        GameMode Classic = new("Classic Pong", upgradeValues, 3f);
        gameModes.Add(Classic);

        foreach (GameMode mode in gameModes) {
            //Debug.Log(mode.ToString());
        }
    }

    // Update is called once per frame
    void Update() {
        if (!_gameManager.AmITheHost() && PlayMode.IsOnline) {
            return;
        }

        if (p1 == null || p2 == null) {
            InitializePaddles();
            return;
        }

        if (PlayMode.IsOnline &&_multiplayer.CurrentRoom.Users.Count < 2) {
            //Debug.Log("getting out before creating new game modes");
            return;
        }

        // Randomly select 3 options for players to choose from
        if (!_gameModePoolIsSet) {
            if(!_waiting) {
                _waiting = true;
                StartCoroutine(WaitAndGenerateGameModePool());
            }
        }

        if (p1.HasVoted() && p2.HasVoted() && !_gameModeConfirmed) {
            _gameModeConfirmed = true;
            DecideOnGameMode();
            p1.SetVoted(false);
            p2.SetVoted(false);
        }
    }

    private IEnumerator WaitAndGenerateGameModePool() {
        yield return new WaitForSeconds(0.3f);
        if (!_gameModePoolIsSet && _gameManager.AmITheHost()) {
            GenerateGameModePool();
        }
        _waiting = false;
    }

    private void InitializePaddles() {
        var playerList = GameObject.FindGameObjectsWithTag("Player");
        if (playerList.Length < 2) {
            return;
        }

        p1 = playerList[0].GetComponent<Paddle>();
        p2 = playerList[1].GetComponent<Paddle>();
        _gameModeConfirmed = false;
        _chosenGameMode = null;
        _chosenGameModeName = "";
    }

    // The host picks 3 game modes to place in the pool and then broadcasts them to both players
    private void GenerateGameModePool() {
        if (!_gameManager.AmITheHost() && PlayMode.IsOnline) {
            return;
        }

        if (p1 == null || p2 == null) {
            InitializePaddles();
            if(PlayMode.IsOnline)
                return;
        }

        _gameModeOption.ClearGameModeOptions();

        int randSeed = Random.Range(0, 1000);
        //int randSeed = 5;
        List<GameMode> shuffledGameModes = Shuffler.Shuffle(gameModes, randSeed);

        List<GameMode> availablePool = new List<GameMode>(shuffledGameModes.GetRange(0, 3));
        _gameModePool = availablePool;
        _gameModePoolIsSet = true;

        //Debug.LogError("Generated pool successfully, time to broadcast");
        //now that we've set the available pool, we need to pass it to both players so they can update their UI, and vote on it
        StartCoroutine(WaitAndBroadcastGameModes());
        //BroadcastGameModes();
    }

    public IEnumerator WaitAndBroadcastGameModes() {
        yield return new WaitForSeconds(0.3f);
        BroadcastGameModes();
    }

    public void BroadcastGameModes() {

        if (!_gameManager.AmITheHost() && PlayMode.IsOnline) {
            return;
        }
        if (_gameModePoolIsSet && _gameModePool != null) {
            p1.BroadcastRemoteMethod(nameof(p1.SetGameModePool), _gameModePool);
            if(PlayMode.IsOnline)
                p2.BroadcastRemoteMethod(nameof(p2.SetGameModePool), _gameModePool);
        }
        else {
            GenerateGameModePool();
        }
    }

    public void SetGameModePool(List<GameMode> gameModes) {
        _gameModeOptionTexts[0].text = gameModes[0].GetName();
        _gameModeOptionTexts[1].text = gameModes[1].GetName();
        _gameModeOptionTexts[2].text = gameModes[2].GetName();

        _gameModeOptionImages[0].sprite = Resources.Load<Sprite>(gameModes[0].GetImageURL());
        _gameModeOptionImages[1].sprite = Resources.Load<Sprite>(gameModes[1].GetImageURL());
        _gameModeOptionImages[2].sprite = Resources.Load<Sprite>(gameModes[2].GetImageURL());
        //GameOverPanel.GetComponentInChildren<TMP_Text>().font = Resources.Load<TMP_FontAsset>("Fonts/Game Over/GreatVibes-Regular SDF");

        //Resources.Load<TMP_FontAsset>("Fonts/Game Over/GreatVibes-Regular SDF");
        _gameModePool = gameModes;
        //TODO: add the images to the buttons that describe each game mode and what it entails
    }

    private void DecideOnGameMode() {
        if (p2 == null)
            return;

        List<int> p1Choices = p1.GetSelectedGameModes(), p2Choices = p2.GetSelectedGameModes();
        List<GameMode> candidates = new();
        int votes;
        int threshold = 2;

        while (candidates.Count == 0) {
            for (int i = 1; i < 4; i++) {
                votes = 0;
                if (p1Choices.Contains(i)) {
                    votes++;
                }

                if (p2Choices.Contains(i)) {
                    votes++;
                }

                if (votes == threshold) {
                    //Debug.Log(_gameModePool[i - 1]);
                    candidates.Add(_gameModePool[i - 1]);
                    //Debug.Log("Threshold met at index:" + i);
                }
            }
            threshold--;
        }

        // randomly select a game mode from all the available candidates
        int randomGameModeIndex = Random.Range(0, candidates.Count);
        _chosenGameMode = candidates[randomGameModeIndex];
        _chosenGameModeName = _chosenGameMode.GetName();
        //Debug.Log("Chosen mode: " + _chosenGameMode.GetName());
        p1.BroadcastRemoteMethod(nameof(p1.SetChosenGameMode), _chosenGameMode);
    }

    public void UpdateChosenGameMode(GameMode gameMode) {
        gameModeText.text = "Game Mode: " + gameMode.GetName();
        //take values and pass them to UpgradeManager

        _upgradeManager.SetUpgradeWeights(gameMode.GetUpgradeWeights());
        GameManager.gameMode = gameMode;
    }

    public void UnsetGameMode() {
        _gameModeConfirmed = false;
    }
}
