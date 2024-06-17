using Alteruna;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class GameModeManager : MonoBehaviour {
    public List<GameMode> gameModes;
    private List<GameMode> _gameModePool;

    private GameManager _gameManager;
    private Paddle p1, p2;

    private bool _gameModePoolIsSet;
    private bool _gameModeConfirmed;
    private GameMode _chosenGameMode;

    private UpgradeManager _upgradeManager;

    [SerializeField]
    private TMP_Text gameModeText;

    [SerializeField]
    private List<TMP_Text> _gameModeOptionTexts;


    void Start() {
        PopulateGameModes();
        Initialize();
    }

    public void Initialize() {
        _gameManager = FindObjectOfType<GameManager>();
        _upgradeManager = FindObjectOfType<UpgradeManager>();
        gameModeText.text = "Game Mode: ?";
        _gameModeConfirmed = false;
        _gameModePoolIsSet = false;
        _gameModePool = new();
        _gameModeOptionTexts[0].text = "";
        _gameModeOptionTexts[1].text = "";
        _gameModeOptionTexts[2].text = "";
    }

    private void PopulateGameModes() {
        gameModes = new List<GameMode>();

        // basic game mode: Original design of the game where everything is balanced
        int[] upgradeValues = new int[17] { 4, 4, 1, 3, 1, 2, 2, 2, 2, 1, 2, 1, 2, 1, 1, 1, 1 };
        GameMode Base = new("Base Mode", upgradeValues);
        gameModes.Add(Base);

        // Stormy Night: Game mode where there's lots of "wind" and "wind against player"
        upgradeValues = new int[17] { 3, 2, 1, 1, 1, 0, 10, 0, 0, 0, 6, 2, 3, 3, 2, 1, 2 };
        GameMode Stormy = new("Stormy Night", upgradeValues);
        gameModes.Add(Stormy);

        // Angelic: All buffs and good neutrals 
        upgradeValues = new int[17] { 4, 3, 1, 3, 3, 2, 3, 0, 0, 0, 0, 0, 2, 4, 0, 0, 1 };
        GameMode Angelic = new("Angelic", upgradeValues);
        gameModes.Add(Angelic);

        // Mine Field/Demonic: All the debuffs and negative neutrals
        upgradeValues = new int[17] { 0, 0, 0, 0, 0, 0, 0, 4, 2, 2, 2, 1, 0, 0, 1, 1, 1 };
        GameMode MineField = new("Mine Field", upgradeValues);
        gameModes.Add(MineField);

        // Ball Mania: Lots of split balls and bonus lives
        upgradeValues = new int[17] { 3, 3, 6, 2, 1, 2, 1, 0, 0, 0, 0, 2, 2, 1, 1, 1, 10 };
        GameMode BallMania = new("Ball Mania", upgradeValues);
        gameModes.Add(BallMania);

        // Drunken Brawl: Lots of “Flip Enemy Controls”, “Flip Controls”, “Flip Both Controls”
        upgradeValues = new int[17] { 2, 2, 1, 2, 2, 10, 2, 2, 2, 6, 2, 0, 0, 0, 0, 8, 1 };
        GameMode DrunkenBrawl = new("Drunken Brawl", upgradeValues);
        gameModes.Add(DrunkenBrawl);

        // Speed Racer: Lots of “Faster Paddle” and “Faster Both Paddles” and shorter
        upgradeValues = new int[17] { 0, 5, 1, 4, 0, 1, 1, 4, 0, 1, 0, 0, 0, 5, 0, 0, 1 };
        GameMode SpeedRacer = new("Speed Racer", upgradeValues);
        gameModes.Add(SpeedRacer);

        // Dumbo Style: Lots of Slowness and Elongation
        upgradeValues = new int[17] { 5, 0, 0, 0, 4, 0, 0, 0, 2, 0, 0, 0, 5, 0, 3, 0, 0 };
        GameMode Dumbo = new("Dumbo", upgradeValues);
        gameModes.Add(Dumbo);

        // We’re in this together: Heavier stats for neutral upgrades rather than buffs/debuffs
        upgradeValues = new int[17] { 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 6, 6, 6, 6, 4, 3 };
        GameMode Together = new("We're in this together", upgradeValues);
        gameModes.Add(Together);

        // Vanilla: No upgrades, but ball gets faster faster. TODO: how?
        upgradeValues = new int[17] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        GameMode Vanilla = new("Vanilla", upgradeValues);
        gameModes.Add(Vanilla);

        foreach (GameMode mode in gameModes) {
            //Debug.Log(mode.ToString());
        }
    }

    // Update is called once per frame
    void Update() {
        if (!_gameManager.AmITheHost()) {
            return;
        }

        if (p1 == null || p2 == null) {
            InitializePaddles();
            return;
        }

        // Randomly select 3 options for players to choose from
        if (!_gameModePoolIsSet) {
            GenerateGameModePool();
        }

        if (p1.HasVoted() && p2.HasVoted() && !_gameModeConfirmed) {
            _gameModeConfirmed = true;
            DecideOnGameMode();
            p1.SetVoted(false);
            p2.SetVoted(false);
        }
    }

    private void InitializePaddles() {
        var playerList = GameObject.FindGameObjectsWithTag("Player");
        p1 = playerList[0].GetComponent<Paddle>();
        if (playerList.Length < 2) {
            return;
        }

        p2 = playerList[1].GetComponent<Paddle>();
        _gameModeConfirmed = false;
        _chosenGameMode = null;
    }

    // The host picks 3 game modes to place in the pool and then broadcasts them to both players
    private void GenerateGameModePool() {
        if (!_gameManager.AmITheHost()) {
            return;
        }

        if (p1 == null || p2 == null) {
            InitializePaddles();
            return;
        }

        int randSeed = Random.Range(0, 1000);
        //int randSeed = 5;
        List<GameMode> shuffledGameModes = Shuffler.Shuffle(gameModes, randSeed);

        List<GameMode> availablePool = new List<GameMode>(shuffledGameModes.GetRange(0, 3));
        _gameModePool = availablePool;
        _gameModePoolIsSet = true;

        //now that we've set the available pool, we need to pass it to both players so they can update their UI, and vote on it
        BroadcastGameModes();
    }

    public void BroadcastGameModes() {
        if (!_gameManager.AmITheHost()) {
            return;
        }
        if (_gameModePoolIsSet) {
            p1.BroadcastRemoteMethod(nameof(p1.SetGameModePool), _gameModePool);
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
        _gameModePool = gameModes;
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
                    candidates.Add(_gameModePool[i - 1]);
                    //Debug.Log("Threshold met at index:" + i);
                }
            }
            threshold--;
        }

        // randomly select a game mode from all the available candidates
        int randomGameModeIndex = Random.Range(0, candidates.Count);
        _chosenGameMode = candidates[randomGameModeIndex];
        Debug.Log("Chosen mode: " + _chosenGameMode.GetName());
        p1.BroadcastRemoteMethod(nameof(p1.SetChosenGameMode), _chosenGameMode);
    }

    public void UpdateChosenGameMode(GameMode gameMode) {
        gameModeText.text = "Game Mode: " + gameMode.GetName();
        //take values and pass them to UpgradeManager

        _upgradeManager.SetUpgradeWeights(gameMode.GetUpgradeWeights());
    }
}
