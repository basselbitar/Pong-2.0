using Alteruna;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TweenUIManager : MonoBehaviour {

    [SerializeField]
    GameObject BigPanel, PlayButton, HowToPlayButton, OptionsButton, QuitButton, MusicSlider, SFXSlider, UpgradeSlider, AIDifficulty,
    BackButton, BackPanel, MainMenuPanel, OptionsPanel;

    // How to Play Panel
    [SerializeField]
    GameObject HowToPlayPanel, HTPP_ObjectiveButton, HTPP_ControlsButton, HTPP_PowerUpsButton, HTPP_GameModesButton, HTPP_Explanation, HTPP_BackButton;

    // Play Panel
    [SerializeField]
    GameObject PlayPanel, PP_PlayVsPc, PP_LocalPvP, PP_PlayOnline, PP_BackButton; 

    // Room List Panel
    [SerializeField]
    GameObject RoomListPanel, RLP_ScrollView, RLP_TitleText, RLP_CreateRoom, RLP_BackButton;

    // Waiting For Player Panel
    [SerializeField]
    GameObject WaitingForPlayerPanel, WFPP_JoinedRoomText, WFPP_WaitingText, WFPP_BackButton;

    // Game Mode Selection Panel
    [SerializeField]
    GameObject GameModeSelectionPanel, GMSP_InRoomText, GMSP_Options, GMSP_PromptText, GMSP_NextButton, GMSP_BackButton;

    // Paddle Selector Panel
    [SerializeField]
    GameObject PaddleSelectorPanel, PSP_Options, PSP_InRoomText, PSP_GameModeText, PSP_ReadyButton, PSP_BackButton;

    // Game Over Panel
    [SerializeField]
    GameObject GameOverPanel, GOP_WinLoseText, GOP_RematchButton, GOP_BackButton;

    private Multiplayer _multiplayer;

    private const LeanTweenType EASE_OUT_CIRC = LeanTweenType.easeOutCirc,
        EASE_IN_QUART = LeanTweenType.easeInQuart;

    public bool gameStarted;

    void Awake() {
        MainMenuPanel.SetActive(true);
        InitializeVolumeSliders();
        Deactivate(HowToPlayPanel, OptionsPanel, PlayPanel, RoomListPanel, WaitingForPlayerPanel, GameModeSelectionPanel, PaddleSelectorPanel, GameOverPanel);
        SetScaleToZero(PlayButton, OptionsButton, HowToPlayButton, QuitButton, MusicSlider, SFXSlider, UpgradeSlider, AIDifficulty, BackButton, BackPanel,
            HTPP_ObjectiveButton, HTPP_ControlsButton, HTPP_PowerUpsButton, HTPP_GameModesButton, HTPP_Explanation, HTPP_BackButton,
            PP_PlayVsPc, PP_LocalPvP, PP_PlayOnline, PP_BackButton,
            RLP_ScrollView, RLP_TitleText, RLP_CreateRoom, RLP_BackButton, WFPP_JoinedRoomText, WFPP_WaitingText, WFPP_BackButton,
            GMSP_InRoomText, GMSP_Options, GMSP_PromptText, GMSP_NextButton,
            PSP_Options, PSP_InRoomText, PSP_GameModeText, PSP_ReadyButton, PSP_BackButton, GOP_WinLoseText, GOP_RematchButton, GOP_BackButton);

        StartTween();
    }

    public void Start() {
        _multiplayer = FindObjectOfType<Multiplayer>();
        //_multiplayer.OnRoomJoined.AddListener(JoinedRoom);
    }


    public void Update() {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            int activePanels = 0;
            if(OptionsPanel.activeSelf) {
                BackFromOptions();
                activePanels++;
            }
            if (PlayPanel.activeSelf) {
                BackFromPlayPanel();
                activePanels++;
            }
            if(HowToPlayPanel.activeSelf) {
                BackFromHowToPlay();
                activePanels++;
            }
            if (RoomListPanel.activeSelf) {
                BackFromRoomList();
                activePanels++;
            }
            if (WaitingForPlayerPanel.activeSelf) {
                BackFromWaitingForPlayer();
                FindObjectOfType<RoomHandler>().OnLeaveRoomClicked();
                activePanels++;
            }
            if (GameModeSelectionPanel.activeSelf) {
                BackFromGameModeSelection();
                activePanels++;
            }
            if (PaddleSelectorPanel.activeSelf) {
                BackFromPaddleSelector();
                activePanels++;
            }
            Debug.Log("There are " + activePanels + " active panels");
            if(activePanels == 0) {
                ShowMainMenuButtons();
            }
        }
    }

    void InitializeVolumeSliders() {
        float musicVolume = PlayerPrefs.GetFloat("musicVolume");
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        float upgradeVolume = PlayerPrefs.GetFloat("upgradeVolume");

        MusicSlider.GetComponent<UnityEngine.UI.Slider>().SetValueWithoutNotify(musicVolume);
        SFXSlider.GetComponent<UnityEngine.UI.Slider>().SetValueWithoutNotify(sfxVolume);
        UpgradeSlider.GetComponent<UnityEngine.UI.Slider>().SetValueWithoutNotify(upgradeVolume);

        var toggles = AIDifficulty.GetComponentsInChildren<Toggle>();
        foreach (var item in toggles)
        {
            item.isOn = false;
        }
        toggles[PlayerPrefs.GetInt("Difficulty")].isOn = true;
    }

    // utility
    void Deactivate(params GameObject[] gameObjects) {
        for (int i = 0; i < gameObjects.Length; i++) {
            gameObjects[i].SetActive(false);
        }
    }

    void SetScaleToZero(params GameObject[] gameObjects) {
        for (int i = 0; i < gameObjects.Length; i++) {
            gameObjects[i].transform.localScale = Vector3.zero;
        }
    }

    public void Play() {
        ShowPlayButtons();
    }

    public void Options() {
        ShowOptionsButtons();
    }

    public void HowToPlay() {
        ShowHowToPlayButtons();
    }

    public void PlayVsPC() {
        PlayMode.selectedPlayMode = PlayMode.PlayModeType.PlayVsPC;
        ShowGameModeSelectionButtons();
        FindObjectOfType<RoomHandler>().SetOfflineRoomText();
        OnRoomJoined(); //simulate entering a room
    }

    public void PlayLocalMultiplayer() {
        PlayMode.selectedPlayMode = PlayMode.PlayModeType.PlayLocal;
        ShowGameModeSelectionButtons();
        FindObjectOfType<RoomHandler>().SetOfflineRoomText();
        OnRoomJoined(); //simulate entering a room
    }

    public void PlayOnline() {
        PlayMode.selectedPlayMode = PlayMode.PlayModeType.PlayOnline;
        ShowRoomListButtons();
    }

    public void Next() {
        ShowPaddleSelectorButtons();
    }

    public void BackFromOptions() {
        LeanTween.scale(MusicSlider, Vector3.zero, 0.6f).setDelay(.1f).setEase(EASE_IN_QUART);
        LeanTween.scale(SFXSlider, Vector3.zero, 0.6f).setDelay(.2f).setEase(EASE_IN_QUART);
        LeanTween.scale(UpgradeSlider, Vector3.zero, 0.6f).setDelay(.3f).setEase(EASE_IN_QUART);
        LeanTween.scale(AIDifficulty, Vector3.zero, 0.6f).setDelay(.4f).setEase(EASE_IN_QUART);
        LeanTween.scale(BackButton, Vector3.zero, 0.6f).setDelay(.5f).setEase(EASE_IN_QUART).setOnComplete(DisableOptionsPanel);
        ShowMainMenuButtons();
    }

    public void BackFromHowToPlay() {
        HideHowToPlayButtons();
        ShowMainMenuButtons();
    }

    public void BackFromPlayPanel() {
        HidePlayButtons();
        ShowMainMenuButtons();
    }

    public void BackFromRoomList() {
        HideRoomListButtons();
        ShowMainMenuButtons();
    }

    public void BackFromWaitingForPlayer() {
        HideWaitingForPlayerButtons();
        ShowRoomListButtons();
    }

    public void BackFromGameModeSelection() {
        HideGameModeSelectionButtons();
        if(PlayMode.IsOnline) {
            ShowRoomListButtons();
        } else {
            ShowPlayButtons();
        }
    }

    public void BackFromPaddleSelector() {
        HidePaddleSelectorButtons();
        if (PlayMode.IsOnline) {
            ShowRoomListButtons();
        }
        else {
            ShowPlayButtons();
        }
    }

    public void BackFromGameOver() {
        HideGameOverButtons();
        if (PlayMode.IsOnline) {
            ShowRoomListButtons();
        }
        else {
            ShowPlayButtons();
            FindObjectOfType<GameManager>().DestroyRemainingPaddles();
        }
    }

    public void Rematch() {
        HideGameOverButtons();
        if(PlayMode.IsOnline)
            ShowPaddleSelectorButtons();
        else if(PlayMode.selectedPlayMode == PlayMode.PlayModeType.PlayLocal) {
            PlayLocalMultiplayer();
        } else {
            PlayVsPC();
        }

    }
    public void PlayGame() {
        HidePaddleSelectorButtons();
        HideBigPanel();
        gameStarted = true; //this activates the in-game Quit button
    }

    void DisableOptionsPanel() {
        OptionsPanel.SetActive(false);
    }
    void DisableMainMenu() {
        MainMenuPanel.SetActive(false);
    }
    void DisableHowToPlayPanel() {
        HowToPlayPanel.SetActive(false);
    }
    void DisablePlayPanel() {
        PlayPanel.SetActive(false);
    }

    void DisableRoomList() {
        RoomListPanel.SetActive(false);
    }
    void DisableWaitingForPlayer() {
        WaitingForPlayerPanel.SetActive(false);
    }
    void DisableGameModeSelection() {
        GameModeSelectionPanel.SetActive(false);
    }
    void DisablePaddleSelector() {
        PaddleSelectorPanel.SetActive(false);
    }
    void DisableGameOver() {
        GameOverPanel.SetActive(false);
    }

    // Display the Start Menu
    public void StartTween() {
        LeanTween.scale(BackPanel, Vector3.one, 0.9f).setDelay(.3f).setEase(EASE_OUT_CIRC);
        ShowMainMenuButtons();
    }

    // Show buttons functions
    void ShowMainMenuButtons() {
        MainMenuPanel.SetActive(true);
        LeanTween.scale(PlayButton, Vector3.one, 0.7f).setDelay(.6f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(HowToPlayButton, Vector3.one, 0.7f).setDelay(.7f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(OptionsButton, Vector3.one, 0.7f).setDelay(.8f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(QuitButton, Vector3.one, 0.7f).setDelay(.9f).setEase(EASE_OUT_CIRC);
        PlayButton.GetComponent<Button>().Select();
    }

    void ShowHowToPlayButtons() {
        HideMainMenuButtons();
        HowToPlayPanel.SetActive(true);

        LeanTween.scale(HTPP_ObjectiveButton, Vector3.one, 0.6f).setDelay(.5f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(HTPP_ControlsButton, Vector3.one, 0.6f).setDelay(.6f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(HTPP_PowerUpsButton, Vector3.one, 0.6f).setDelay(.7f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(HTPP_GameModesButton, Vector3.one, 0.6f).setDelay(.8f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(HTPP_Explanation, Vector3.one, 0.6f).setDelay(.9f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(HTPP_BackButton, Vector3.one, 0.6f).setDelay(1.1f).setEase(EASE_OUT_CIRC);
        FindObjectOfType<HowToPlayManager>().ShowCurrentPanel();
        HTPP_ObjectiveButton.GetComponent<Button>().Select();
    }

    void ShowOptionsButtons() {
        HideMainMenuButtons();
        OptionsPanel.SetActive(true);

        LeanTween.scale(MusicSlider, Vector3.one, 0.6f).setDelay(.5f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(SFXSlider, Vector3.one, 0.6f).setDelay(.6f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(UpgradeSlider, Vector3.one, 0.6f).setDelay(.7f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(AIDifficulty, Vector3.one, 0.6f).setDelay(0.9f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(BackButton, Vector3.one, 0.6f).setDelay(1.2f).setEase(EASE_OUT_CIRC);
        MusicSlider.GetComponent<Slider>().Select();
    }

    void ShowPlayButtons() {
        HideMainMenuButtons();
        PlayPanel.SetActive(true);

        GMSP_NextButton.GetComponent<NextButton>().Uninitialize();
        LeanTween.scale(PP_PlayVsPc, Vector3.one, 0.6f).setDelay(.5f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(PP_LocalPvP, Vector3.one, 0.6f).setDelay(.6f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(PP_PlayOnline, Vector3.one, 0.6f).setDelay(.7f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(PP_BackButton, Vector3.one, 0.6f).setDelay(.8f).setEase(EASE_OUT_CIRC);
        PP_PlayVsPc.GetComponent<Button>().Select();
    }

    void ShowRoomListButtons() {
        HideMainMenuButtons();
        HidePlayButtons();
        RoomListPanel.SetActive(true);
        LeanTween.scale(RLP_TitleText, Vector3.one, 0.6f).setDelay(.5f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(RLP_ScrollView, Vector3.one, 0.6f).setDelay(.7f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(RLP_CreateRoom, Vector3.one, 0.6f).setDelay(.9f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(RLP_BackButton, Vector3.one, 0.6f).setDelay(1.2f).setEase(EASE_OUT_CIRC);
        gameStarted = false;
        FindObjectOfType<Multiplayer>().RefreshRoomList();
        RLP_CreateRoom.GetComponent<Button>().Select();
    }

    public void ShowWaitingForPlayerButtons() {
        HideRoomListButtons();
        WaitingForPlayerPanel.SetActive(true);
        LeanTween.scale(WFPP_JoinedRoomText, Vector3.one, 0.6f).setDelay(.5f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(WFPP_WaitingText, Vector3.one, 0.6f).setDelay(.7f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(WFPP_BackButton, Vector3.one, 0.6f).setDelay(.9f).setEase(EASE_OUT_CIRC);
        gameStarted = false;
        WFPP_BackButton.GetComponent<Button>().Select();
    }

    public void ShowGameModeSelectionButtons() {
        HidePlayButtons();
        HideRoomListButtons();
        HideWaitingForPlayerButtons();
        HidePaddleSelectorButtons();
        GameModeSelectionPanel.SetActive(true);
        LeanTween.scale(GMSP_InRoomText, Vector3.one, 0.6f).setDelay(.7f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(GMSP_Options, Vector3.one, 0.9f).setDelay(.8f).setEase(EASE_OUT_CIRC).setOnComplete(() => { InitializeGameModeOptionsPositions(); });
        LeanTween.scale(GMSP_PromptText, Vector3.one, 0.6f).setDelay(.9f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(GMSP_NextButton, Vector3.one, 0.6f).setDelay(1.0f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(GMSP_BackButton, Vector3.one, 0.6f).setDelay(1.1f).setEase(EASE_OUT_CIRC);
        FindObjectOfType<NextButton>().Initialize();
        LeanTween.scale(PSP_BackButton, Vector3.one, 0.6f).setDelay(1.2f).setEase(EASE_OUT_CIRC);
        gameStarted = false;
    }

    private void InitializeGameModeOptionsPositions() {
        foreach (var osh in GMSP_Options.GetComponentsInChildren<OptionSelectionHandler>()) {
            osh.enabled = true;
            osh.InitializePositionAndScale();
        }
        GMSP_Options.GetComponentInChildren<Button>().Select();
    }

    private void DeactivateGameModeOptionsSelectionHandlers() {
        foreach (var osh in GMSP_Options.GetComponentsInChildren<OptionSelectionHandler>()) {
            osh.enabled = false;
        }
    }
    public void ShowPaddleSelectorButtons() {
        HideRoomListButtons();
        HideWaitingForPlayerButtons();
        HideGameModeSelectionButtons();
        PaddleSelectorPanel.SetActive(true);
        LeanTween.scale(PSP_InRoomText, Vector3.one, 0.6f).setDelay(.6f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(PSP_GameModeText, Vector3.one, 0.6f).setDelay(.7f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(PSP_Options, Vector3.one, 0.6f).setDelay(.8f).setEase(EASE_OUT_CIRC).setOnComplete(() => { InitializePaddleOptionsPositions(); });
        LeanTween.scale(PSP_ReadyButton, Vector3.one, 0.6f).setDelay(.9f).setEase(EASE_OUT_CIRC);
        if(PlayMode.IsOnline)
            FindObjectOfType<ReadyButton>().Initialize();

        LeanTween.scale(PSP_BackButton, Vector3.one, 0.6f).setDelay(1f).setEase(EASE_OUT_CIRC);
        gameStarted = false;
    }

    private void InitializePaddleOptionsPositions() {
        foreach (var osh in PSP_Options.GetComponentsInChildren<OptionSelectionHandler>()) {
            osh.enabled = true;
            osh.InitializePositionAndScale();
        }
        PSP_Options.GetComponentInChildren<Button>().Select();
    }

    private void DeactivatePaddleOptionsSelectionHandlers() {
        foreach (var osh in GMSP_Options.GetComponentsInChildren<OptionSelectionHandler>()) {
            osh.enabled = false;
        }
    }


    void ShowGameOverButtons() {
        HideMainMenuButtons();
        GameOverPanel.SetActive(true);

        LeanTween.scale(GOP_WinLoseText, Vector3.one, 0.6f).setDelay(.6f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(GOP_RematchButton, Vector3.one, 0.6f).setDelay(.7f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(GOP_BackButton, Vector3.one, 0.6f).setDelay(.8f).setEase(EASE_OUT_CIRC);

        gameStarted = false;
        GOP_RematchButton.GetComponent<Button>().Select();
    }

    public void ShowBigPanel() {
        if (BigPanel.activeSelf) return;
        BigPanel.SetActive(true);
        LeanTween.scale(BigPanel, Vector3.one, 0.4f).setDelay(.5f).setEase(EASE_OUT_CIRC);
        gameStarted = false;
    }

    void HideBigPanel() {
        LeanTween.scale(BigPanel, Vector3.zero, 0.6f).setDelay(.3f).setEase(EASE_IN_QUART).setOnComplete(() => { BigPanel.SetActive(false); });
    }

    // Hide buttons functions
    void HideMainMenuButtons() {
        LeanTween.scale(PlayButton, Vector3.zero, 0.6f).setEase(EASE_IN_QUART);
        LeanTween.scale(HowToPlayButton, Vector3.zero, 0.6f).setDelay(.1f).setEase(EASE_IN_QUART);
        LeanTween.scale(OptionsButton, Vector3.zero, 0.6f).setDelay(.2f).setEase(EASE_IN_QUART);
        LeanTween.scale(QuitButton, Vector3.zero, 0.6f).setDelay(.3f).setEase(EASE_IN_QUART)
        .setOnComplete(DisableMainMenu);
    }

    void HideHowToPlayButtons() {
        LeanTween.scale(HTPP_ObjectiveButton, Vector3.zero, 0.6f).setEase(EASE_IN_QUART);
        LeanTween.scale(HTPP_ControlsButton, Vector3.zero, 0.6f).setDelay(.1f).setEase(EASE_IN_QUART);
        LeanTween.scale(HTPP_PowerUpsButton, Vector3.zero, 0.6f).setDelay(.2f).setEase(EASE_IN_QUART); 
        LeanTween.scale(HTPP_GameModesButton, Vector3.zero, 0.6f).setDelay(.3f).setEase(EASE_IN_QUART);
        LeanTween.scale(HTPP_Explanation, Vector3.zero, 0.6f).setDelay(.4f).setEase(EASE_IN_QUART);
        LeanTween.scale(HTPP_BackButton, Vector3.zero, 0.6f).setDelay(.5f).setEase(EASE_IN_QUART)
        .setOnComplete(DisableHowToPlayPanel);
    }

    void HidePlayButtons() {
        LeanTween.scale(PP_PlayVsPc, Vector3.zero, 0.6f).setEase(EASE_IN_QUART);
        LeanTween.scale(PP_LocalPvP, Vector3.zero, 0.6f).setDelay(.1f).setEase(EASE_IN_QUART);
        LeanTween.scale(PP_PlayOnline, Vector3.zero, 0.6f).setDelay(.2f).setEase(EASE_IN_QUART);
        LeanTween.scale(PP_BackButton, Vector3.zero, 0.6f).setDelay(.3f).setEase(EASE_IN_QUART)
        .setOnComplete(DisablePlayPanel);
    }

    void HideRoomListButtons() {
        LeanTween.scale(RLP_TitleText, Vector3.zero, 0.6f).setEase(EASE_IN_QUART);
        LeanTween.scale(RLP_ScrollView, Vector3.zero, 0.6f).setDelay(.1f).setEase(EASE_IN_QUART);
        LeanTween.scale(RLP_CreateRoom, Vector3.zero, 0.6f).setDelay(.2f).setEase(EASE_IN_QUART);
        LeanTween.scale(RLP_BackButton, Vector3.zero, 0.6f).setDelay(.3f).setEase(EASE_IN_QUART)
            .setOnComplete(DisableRoomList);
    }

    void HideWaitingForPlayerButtons() {
        LeanTween.scale(WFPP_JoinedRoomText, Vector3.zero, 0.6f).setEase(EASE_IN_QUART);
        LeanTween.scale(WFPP_WaitingText, Vector3.zero, 0.6f).setDelay(.1f).setEase(EASE_IN_QUART);
        LeanTween.scale(WFPP_BackButton, Vector3.zero, 0.6f).setDelay(.2f).setEase(EASE_IN_QUART)
            .setOnComplete(DisableWaitingForPlayer);
    }

    void HideGameModeSelectionButtons() {
        DeactivateGameModeOptionsSelectionHandlers();
        LeanTween.scale(GMSP_InRoomText, Vector3.zero, 0.6f).setEase(EASE_IN_QUART);
        LeanTween.scale(GMSP_Options, Vector3.zero, 0.6f).setDelay(.1f).setEase(EASE_IN_QUART);
        LeanTween.scale(GMSP_PromptText, Vector3.zero, 0.6f).setDelay(.2f).setEase(EASE_IN_QUART);
        LeanTween.scale(GMSP_NextButton, Vector3.zero, 0.6f).setDelay(.3f).setEase(EASE_IN_QUART);
        LeanTween.scale(GMSP_BackButton, Vector3.zero, 0.6f).setDelay(.4f).setEase(EASE_IN_QUART)
            .setOnComplete(DisableGameModeSelection);
    }

    public void HidePaddleSelectorButtons() {
        DeactivatePaddleOptionsSelectionHandlers();
        LeanTween.scale(PSP_InRoomText, Vector3.zero, 0.6f).setEase(EASE_IN_QUART);
        LeanTween.scale(PSP_GameModeText, Vector3.zero, 0.6f).setEase(EASE_IN_QUART);
        LeanTween.scale(PSP_Options, Vector3.zero, 0.6f).setDelay(.1f).setEase(EASE_IN_QUART);
        LeanTween.scale(PSP_ReadyButton, Vector3.zero, 0.6f).setDelay(.2f).setEase(EASE_IN_QUART);
        LeanTween.scale(WFPP_BackButton, Vector3.zero, 0.6f).setDelay(.3f).setEase(EASE_IN_QUART)
            .setOnComplete(DisablePaddleSelector);
    }

    void HideGameOverButtons() {
        LeanTween.scale(GOP_WinLoseText, Vector3.zero, 0.6f).setEase(EASE_IN_QUART);
        LeanTween.scale(GOP_RematchButton, Vector3.zero, 0.6f).setDelay(.1f).setEase(EASE_IN_QUART);
        LeanTween.scale(GOP_BackButton, Vector3.zero, 0.6f).setDelay(.2f).setEase(EASE_IN_QUART)
            .setOnComplete(DisableGameOver);
    }

    // When joining a room, it might be empty or it might have one player waiting
    public void OnRoomJoined() {
        if (_multiplayer.CurrentRoom != null && _multiplayer.CurrentRoom.Users.Count < 2) {
            ShowWaitingForPlayerButtons();
        }
        else {
            ShowGameModeSelectionButtons();
            // if I'm still the host when a new player joins, I'll broadcast to them the game modes
            if (_multiplayer.LowestUserIndex == _multiplayer.Me.Index || !PlayMode.IsOnline) {
                StartCoroutine(FindObjectOfType<GameModeManager>().WaitAndBroadcastGameModes());
            }
        }
    }

    public void OnOtherPlayerRoomLeft() {
        ShowBigPanel();
        HideGameOverButtons();
        HidePaddleSelectorButtons();
        HideGameModeSelectionButtons();
        ShowWaitingForPlayerButtons();

        FindObjectOfType<GameModeManager>().Initialize();

        //TODO: perhaps "You win" panel should show, saying that enemy has resigned
    }

    // On Quit, tween out the 3 main buttons and exit the game
    public void Quit() {
        QuitTween();
    }
    void QuitTween() {
        LeanTween.scale(QuitButton, Vector3.zero, 0.3f);
        LeanTween.scale(OptionsButton, Vector3.zero, 0.3f).setDelay(.1f);
        LeanTween.scale(PlayButton, Vector3.zero, 0.3f).setDelay(.2f);
        LeanTween.scale(BackPanel, Vector3.zero, 0.3f).setDelay(.3f).setOnComplete(QuitGame);
    }

    void QuitGame() {
        Application.Quit();
    }


    public void ActivateGameOverPanel(int winnerIndex) {
        bool isLocalMultiplayer = PlayMode.selectedPlayMode == PlayMode.PlayModeType.PlayLocal;
        if (winnerIndex == _multiplayer.Me.Index) {
            GameOverPanel.GetComponentInChildren<TMP_Text>().text = isLocalMultiplayer ? "<color=#3BD3ED>Player 1 Wins!</color>" : "<color=#3BD3ED>You Win!</color>";
            GameOverPanel.GetComponentInChildren<TMP_Text>().font = Resources.Load<TMP_FontAsset>("Fonts/Game Over/GreatVibes-Regular SDF");
            FindObjectOfType<UIAudioManager>().PlayYouWinSound();
        }
        else {
            GameOverPanel.GetComponentInChildren<TMP_Text>().text = isLocalMultiplayer ? "<color=#3BD3ED>Player 2 Wins!</color>" : "<color=#BE4545>You Lose!</color>";
            GameOverPanel.GetComponentInChildren<TMP_Text>().font = isLocalMultiplayer ? Resources.Load<TMP_FontAsset>("Fonts/Game Over/GreatVibes-Regular SDF") : Resources.Load<TMP_FontAsset>("Fonts/Game Over/jollygoodsans-basic SDF");
            FindObjectOfType<UIAudioManager>().PlayYouLoseSound();

        }
        Invoke(nameof(ShowBigPanel), 0.5f);
        Invoke(nameof(ShowGameOverButtons), 0.7f);
    }

    public void TweenScoreUI(TMP_Text scoreUI, string text) {
        LeanTween.scale(RLP_ScrollView, Vector3.zero, 0.3f).setEase(EASE_IN_QUART);
        scoreUI.text = text;
        LeanTween.scale(GMSP_InRoomText, Vector3.one, 0.5f).setDelay(.3f).setEase(EASE_OUT_CIRC);
    }
}
