using Alteruna;
using TMPro;
using UnityEngine;

public class TweenUIManager : MonoBehaviour
{

    [SerializeField]
    GameObject PlayButton, OptionsButton, QuitButton, volumeSlider,
    vibrToggle, BackButton, BackPanel, MainMenuPanel, OptionsPanel;

    // Room List Panel
    [SerializeField]
    GameObject RoomListPanel, RLP_ScrollView, RLP_TitleText, RLP_CreateRoom, RLP_LeaveLobby, RLP_BackButton;

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


    void Awake()
    {
        MainMenuPanel.SetActive(true);
        //OptionsPanel.SetActive(false);
        //RoomListPanel.SetActive(false);
        //WaitingForPlayerPanel.SetActive(false);
        //PaddleSelectorPanel.SetActive(false);
        //GameOverPanel.SetActive(false);
        Deactivate(OptionsPanel, RoomListPanel, WaitingForPlayerPanel, PaddleSelectorPanel, GameOverPanel);
        SetScaleToZero(PlayButton, OptionsButton, QuitButton, volumeSlider, vibrToggle, BackButton, BackPanel,
            RLP_ScrollView, RLP_TitleText, RLP_CreateRoom, RLP_LeaveLobby, RLP_BackButton, WFPP_JoinedRoomText, WFPP_WaitingText, WFPP_BackButton,
            PSP_Options, PSP_InRoomText, PSP_ReadyButton, PSP_BackButton, GOP_WinLoseText, GOP_RematchButton, GOP_BackButton);

        StartTween();
    }

    public void Start() {
        _multiplayer = FindObjectOfType<Multiplayer>();
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
        RoomListTween();
    }

    public void Options()
    {
        OptionsPanel.SetActive(true);
        OptionsTween();
    }

    public void BackFromOptions()
    {
        MainMenuPanel.SetActive(true);
        LeanTween.scale(volumeSlider, Vector3.zero, 0.6f).setDelay(.1f).setEase(LeanTweenType.easeInQuart);
        LeanTween.scale(vibrToggle, Vector3.zero, 0.6f).setDelay(.2f).setEase(LeanTweenType.easeInQuart);
        LeanTween.scale(BackButton, Vector3.zero, 0.6f).setDelay(.3f).setEase(LeanTweenType.easeInQuart).setOnComplete(DeactivateOptionsPanel);
        ShowMainMenuButtons();
    }

    public void BackFromRoomList() {
        MainMenuPanel.SetActive(true);
        HideRoomListButtons();
        ShowMainMenuButtons();
    }

    public void BackFromWaitingForPlayer() {
        // Leave the room
    }

    public void BackFromPaddleSelector() {
        // Leave the room
    }

    public void BackFromGameOver() {
        // Leave the room
    }

    void DeactivateOptionsPanel()
    {
        OptionsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }


    //TODO: call this when player is ready to play (after picking a paddle)
    void PlayTween()
    {
        HideMainMenuButtons();
        LeanTween.scale(BackPanel, Vector3.zero, 0.6f).setDelay(.3f).setEase(LeanTweenType.easeInQuart)
        .setOnComplete(() => { });
        //TODO: Start the game
    }


    void OptionsTween()
    {
        HideMainMenuButtons();
        LeanTween.scale(volumeSlider, Vector3.one, 0.6f).setDelay(.5f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(vibrToggle, Vector3.one, 0.6f).setDelay(.6f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(BackButton, Vector3.one, 0.6f).setDelay(.7f).setEase(LeanTweenType.easeOutCirc);
    }

    void RoomListTween() {
        HideMainMenuButtons();
        RoomListPanel.SetActive(true);
        LeanTween.scale(RLP_TitleText, Vector3.one, 0.6f).setDelay(.5f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(RLP_ScrollView, Vector3.one, 0.6f).setDelay(.6f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(RLP_CreateRoom, Vector3.one, 0.6f).setDelay(.6f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(RLP_LeaveLobby, Vector3.one, 0.6f).setDelay(.6f).setEase(LeanTweenType.easeOutCirc);
        //TODO: Add a back button for every screen. Perhaps keep it there when transition from one panel to the next
        LeanTween.scale(BackButton, Vector3.one, 0.6f).setDelay(.7f).setEase(LeanTweenType.easeOutCirc);
    }

    public void WaitingForPlayersTween() {
        HideRoomListButtons();
        WaitingForPlayerPanel.SetActive(true);
        LeanTween.scale(WFPP_JoinedRoomText, Vector3.one, 0.6f).setDelay(.5f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(WFPP_WaitingText, Vector3.one, 0.6f).setDelay(.6f).setEase(LeanTweenType.easeOutCirc);
        //TODO: Add a back button for every screen. Perhaps keep it there when transition from one panel to the next
        LeanTween.scale(WFPP_BackButton, Vector3.one, 0.6f).setDelay(.7f).setEase(LeanTweenType.easeOutCirc);
        
    }

    void PaddleSelectorTween() {
        HideRoomListButtons();
        PaddleSelectorPanel.SetActive(true);
        LeanTween.scale(WFPP_JoinedRoomText, Vector3.one, 0.6f).setDelay(.5f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(WFPP_WaitingText, Vector3.one, 0.6f).setDelay(.6f).setEase(LeanTweenType.easeOutCirc);
        //TODO: Add a back button for every screen. Perhaps keep it there when transition from one panel to the next
        LeanTween.scale(WFPP_BackButton, Vector3.one, 0.6f).setDelay(.7f).setEase(LeanTweenType.easeOutCirc)
        .setOnComplete(DisableRoomList);
        FindObjectOfType<ReadyButton>().Initialize();
    }

    void GameOverTween() {
        HideMainMenuButtons();
        GameOverPanel.SetActive(true);
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
        LeanTween.scale(BackPanel, Vector3.one, 0.9f).setDelay(.3f).setEase(LeanTweenType.easeOutCirc);
        ShowMainMenuButtons();
    }

    // Tween the Play, Options, and Quit Buttons
    void ShowMainMenuButtons() {
        LeanTween.scale(PlayButton, Vector3.one, 0.7f).setDelay(.6f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(OptionsButton, Vector3.one, 0.7f).setDelay(.7f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(QuitButton, Vector3.one, 0.7f).setDelay(.8f).setEase(LeanTweenType.easeOutCirc);
    }

    void HideMainMenuButtons() {
        LeanTween.scale(PlayButton, Vector3.zero, 0.6f).setEase(LeanTweenType.easeInQuart);
        LeanTween.scale(OptionsButton, Vector3.zero, 0.6f).setDelay(.1f).setEase(LeanTweenType.easeInQuart);
        LeanTween.scale(QuitButton, Vector3.zero, 0.6f).setDelay(.2f).setEase(LeanTweenType.easeInQuart)
        .setOnComplete(DisableMainMenu);
    }

    void HideRoomListButtons() {
        LeanTween.scale(RLP_TitleText, Vector3.zero, 0.6f).setEase(LeanTweenType.easeInQuart);
        LeanTween.scale(RLP_ScrollView, Vector3.zero, 0.6f).setDelay(.1f).setEase(LeanTweenType.easeInQuart);
        LeanTween.scale(RLP_CreateRoom, Vector3.zero, 0.6f).setDelay(.2f).setEase(LeanTweenType.easeInQuart);
        LeanTween.scale(RLP_LeaveLobby, Vector3.zero, 0.6f).setDelay(.2f).setEase(LeanTweenType.easeInQuart)
            .setOnComplete(DisableRoomList);
    }

    // When joining a room, it might be empty or it might have one player waiting
    public void OnRoomJoined() {
        if (_multiplayer.CurrentRoom.Users.Count < 2) {
            WaitingForPlayersTween();
            //ActivateWaitingForPlayerPanel();
        }
        else {
            
            PaddleSelectorTween();
            //ActivateReadyCheckPanel();
        }
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
        //GameOverPanel.SetActive(true);
        //if (winnerIndex == _multiplayer.Me.Index) {
        //    GameOverPanel.GetComponentInChildren<TMP_Text>().text = "You Win!";
        //}
        //else {
        //    GameOverPanel.GetComponentInChildren<TMP_Text>().text = "You Lose!";
        //}
    }
}
