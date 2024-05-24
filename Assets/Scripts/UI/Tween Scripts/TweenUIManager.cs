using Alteruna;
using TMPro;
using UnityEngine;

public class TweenUIManager : MonoBehaviour
{

    [SerializeField]
    GameObject BigPanel, PlayButton, OptionsButton, QuitButton, volumeSlider,
    vibrToggle, BackButton, BackPanel, MainMenuPanel, OptionsPanel;

    // Room List Panel
    [SerializeField]
    GameObject RoomListPanel, RLP_ScrollView, RLP_TitleText, RLP_CreateRoom, RLP_BackButton;

    // Waiting For Player Panel
    [SerializeField]
    GameObject WaitingForPlayerPanel, WFPP_JoinedRoomText, WFPP_WaitingText, WFPP_BackButton;

    // Paddle Selector Panel
    [SerializeField]
    GameObject PaddleSelectorPanel, PSP_Options, PSP_InRoomText, PSP_ReadyButton, PSP_BackButton;

    // Game Over Panel
    [SerializeField]
    GameObject GameOverPanel, GOP_WinLoseText, GOP_RematchButton, GOP_BackButton;

    private Multiplayer _multiplayer;

    private const LeanTweenType EASE_OUT_CIRC = LeanTweenType.easeOutCirc,
        EASE_IN_QUART = LeanTweenType.easeInQuart;

    void Awake()
    {
        MainMenuPanel.SetActive(true);
        Deactivate(OptionsPanel, RoomListPanel, WaitingForPlayerPanel, PaddleSelectorPanel, GameOverPanel);
        SetScaleToZero(PlayButton, OptionsButton, QuitButton, volumeSlider, vibrToggle, BackButton, BackPanel,
            RLP_ScrollView, RLP_TitleText, RLP_CreateRoom, RLP_BackButton, WFPP_JoinedRoomText, WFPP_WaitingText, WFPP_BackButton,
            PSP_Options, PSP_InRoomText, PSP_ReadyButton, PSP_BackButton, GOP_WinLoseText, GOP_RematchButton, GOP_BackButton);

        StartTween();
    }

    public void Start() {
        _multiplayer = FindObjectOfType<Multiplayer>();
        //_multiplayer.OnRoomJoined.AddListener(JoinedRoom);
    }

    // utility
    void Deactivate(params GameObject[] gameObjects) {
        for ( int i = 0; i < gameObjects.Length; i++ ) {
            gameObjects[i].SetActive(false);
        }
    }

    void SetScaleToZero(params GameObject[] gameObjects) {
        for (int i = 0; i < gameObjects.Length; i++) {
            gameObjects[i].transform.localScale = Vector3.zero;
        }
    }

    public void Play()
    {
        ShowRoomListButtons();
    }

    public void Options()
    {
        ShowOptionsButtons();
    }

    public void Ready() {
        PlayGame();
    }

    public void BackFromOptions()
    {
        LeanTween.scale(volumeSlider, Vector3.zero, 0.6f).setDelay(.1f).setEase(EASE_IN_QUART);
        LeanTween.scale(vibrToggle, Vector3.zero, 0.6f).setDelay(.2f).setEase(EASE_IN_QUART);
        LeanTween.scale(BackButton, Vector3.zero, 0.6f).setDelay(.3f).setEase(EASE_IN_QUART).setOnComplete(DeactivateOptionsPanel);
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

    public void BackFromPaddleSelector() {
        HidePaddleSelectorButtons();
        ShowRoomListButtons();
    }

    public void BackFromGameOver() {
        HideGameOverButtons();
        ShowRoomListButtons();
    }

    void DeactivateOptionsPanel()
    {
        OptionsPanel.SetActive(false);
    }


    void PlayGame()
    {
        HidePaddleSelectorButtons();
        HideBigPanel();
        //LeanTween.scale(BackPanel, Vector3.zero, 0.7f).setDelay(.3f).setEase(EASE_IN_QUART)
        //.setOnComplete(() => { });
        //TODO: Start the game
    }


   

    void DisableMainMenu()
    {
        MainMenuPanel.SetActive(false);
    }
    void DisableRoomList() {
        RoomListPanel.SetActive(false);
    }
    void DisableWaitingForPlayer() {
        WaitingForPlayerPanel.SetActive(false);
    }
    void DisablePaddleSelector() {
        PaddleSelectorPanel.SetActive(false);
    }
    void DisableGameOver() {
        GameOverPanel.SetActive(false);
    }



    // Display the Start Menu
    void StartTween()
    {
        LeanTween.scale(BackPanel, Vector3.one, 0.9f).setDelay(.3f).setEase(EASE_OUT_CIRC);
        ShowMainMenuButtons();
    }

    // Show buttons functions
    void ShowMainMenuButtons() {
        MainMenuPanel.SetActive(true);
        LeanTween.scale(PlayButton, Vector3.one, 0.7f).setDelay(.6f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(OptionsButton, Vector3.one, 0.7f).setDelay(.7f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(QuitButton, Vector3.one, 0.7f).setDelay(.8f).setEase(EASE_OUT_CIRC);
    }

    void ShowOptionsButtons() {
        HideMainMenuButtons();
        OptionsPanel.SetActive(true);
        LeanTween.scale(volumeSlider, Vector3.one, 0.6f).setDelay(.5f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(vibrToggle, Vector3.one, 0.6f).setDelay(.6f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(BackButton, Vector3.one, 0.6f).setDelay(.7f).setEase(EASE_OUT_CIRC);
    }

    void ShowRoomListButtons() {
        HideMainMenuButtons();
        RoomListPanel.SetActive(true);
        LeanTween.scale(RLP_TitleText, Vector3.one, 0.6f).setDelay(.5f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(RLP_ScrollView, Vector3.one, 0.6f).setDelay(.6f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(RLP_CreateRoom, Vector3.one, 0.6f).setDelay(.6f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(RLP_BackButton, Vector3.one, 0.6f).setDelay(.7f).setEase(EASE_OUT_CIRC);
    }

    public void ShowWaitingForPlayerButtons() {
        HideRoomListButtons();
        WaitingForPlayerPanel.SetActive(true);
        LeanTween.scale(WFPP_JoinedRoomText, Vector3.one, 0.6f).setDelay(.5f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(WFPP_WaitingText, Vector3.one, 0.6f).setDelay(.6f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(WFPP_BackButton, Vector3.one, 0.6f).setDelay(.7f).setEase(EASE_OUT_CIRC);

    }

    void ShowPaddleSelectorButtons() {
        HideRoomListButtons();
        HideWaitingForPlayerButtons();
        PaddleSelectorPanel.SetActive(true);
        LeanTween.scale(PSP_InRoomText, Vector3.one, 0.6f).setDelay(.5f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(PSP_Options, Vector3.one, 0.6f).setDelay(.6f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(PSP_ReadyButton, Vector3.one, 0.6f).setDelay(.6f).setEase(EASE_OUT_CIRC);
        FindObjectOfType<ReadyButton>().Initialize();

        LeanTween.scale(PSP_BackButton, Vector3.one, 0.6f).setDelay(.7f).setEase(EASE_OUT_CIRC);
    }

    void ShowGameOverButtons() {
        HideMainMenuButtons();
        GameOverPanel.SetActive(true);

        LeanTween.scale(GOP_WinLoseText, Vector3.one, 0.6f).setDelay(.5f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(GOP_RematchButton, Vector3.one, 0.6f).setDelay(.6f).setEase(EASE_OUT_CIRC);
        LeanTween.scale(GOP_BackButton, Vector3.one, 0.6f).setDelay(.7f).setEase(EASE_OUT_CIRC);
    }

    void ShowBigPanel() {
        BigPanel.SetActive(true);
        LeanTween.scale(BigPanel, Vector3.one, 0.4f).setDelay(.5f).setEase(EASE_OUT_CIRC);
    }

    void HideBigPanel() {
        LeanTween.scale(BigPanel, Vector3.zero, 0.6f).setDelay(.3f).setEase(EASE_IN_QUART).setOnComplete( () => { BigPanel.SetActive(false); } ) ;
    }

    // Hide buttons functions
    void HideMainMenuButtons() {
        LeanTween.scale(PlayButton, Vector3.zero, 0.6f).setEase(EASE_IN_QUART);
        LeanTween.scale(OptionsButton, Vector3.zero, 0.6f).setDelay(.1f).setEase(EASE_IN_QUART);
        LeanTween.scale(QuitButton, Vector3.zero, 0.6f).setDelay(.2f).setEase(EASE_IN_QUART)
        .setOnComplete(DisableMainMenu);
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

    void HidePaddleSelectorButtons() {
        LeanTween.scale(PSP_InRoomText, Vector3.zero, 0.6f).setEase(EASE_IN_QUART);
        LeanTween.scale(PSP_Options, Vector3.zero, 0.6f).setDelay(.1f).setEase(EASE_IN_QUART);
        LeanTween.scale(PSP_ReadyButton, Vector3.zero, 0.6f).setDelay(.2f).setEase(EASE_IN_QUART);
        LeanTween.scale(WFPP_BackButton, Vector3.zero, 0.6f).setDelay(.3f).setEase(EASE_IN_QUART)
            .setOnComplete(DisablePaddleSelector);
    }

    void HideGameOverButtons() {
        LeanTween.scale(GOP_WinLoseText, Vector3.zero, 0.6f).setEase(EASE_IN_QUART);
        LeanTween.scale(GOP_RematchButton, Vector3.zero, 0.6f).setDelay(.1f).setEase(EASE_IN_QUART);
        LeanTween.scale(GOP_BackButton, Vector3.zero, 0.6f).setDelay(.2f).setEase(EASE_IN_QUART)
            .setOnComplete(DisablePaddleSelector);
    }

    // When joining a room, it might be empty or it might have one player waiting
    public void OnRoomJoined() {
        if (_multiplayer.CurrentRoom.Users.Count < 2) {
            ShowWaitingForPlayerButtons();
        }
        else {
            ShowPaddleSelectorButtons();
        }
    }

    public void OnOtherPlayerRoomLeft() {

        HideGameOverButtons();
        HidePaddleSelectorButtons();
        ShowWaitingForPlayerButtons();
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

    void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }


    public void ActivateGameOverPanel(int winnerIndex) {
        //DeactivatePanels();
        Debug.LogError("Game is over");
        ShowBigPanel();
        ShowGameOverButtons();
        if (winnerIndex == _multiplayer.Me.Index) {
            GameOverPanel.GetComponentInChildren<TMP_Text>().text = "You Win!";
            Debug.LogError("You win");

        }
        else {
            GameOverPanel.GetComponentInChildren<TMP_Text>().text = "You Lose!";
            Debug.LogError("You lose");

        }
    }
}
