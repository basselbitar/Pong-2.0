using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TweenMainMenu : MonoBehaviour
{

    [SerializeField]
    GameObject PlayButton, OptionsButton, QuitButton, volumeSlider,
    vibrToggle, BackButton, BackPanel, MainMenuPanel, OptionsPanel;

    // Room List Panel
    [SerializeField]
    GameObject RoomListPanel, RLP_ScrollView, RLP_TitleText, RLP_CreateRoom, RLP_LeaveLobby;

    // Waiting For Player Panel
    [SerializeField]
    GameObject WaitingForPlayerPanel, WFPP_JoinedRoomText, WFPP_WaitingText, WFPP_LeaveRoomButton;

    // Paddle Selector Panel
    [SerializeField]
    GameObject PaddleSelectorPanel, PSP_Options, PSP_InRoomText, PSP_ReadyButton, PSP_LeaveRoomButton;

    // Game Over Panel
    [SerializeField]
    GameObject GameOverPanel, GOP_WinLoseText, GOP_RematchButton, GOP_LeaveRoomButton;

    private Multiplayer _multiplayer;


    void Awake()
    {
        MainMenuPanel.SetActive(true);
        OptionsPanel.SetActive(false);
        RoomListPanel.SetActive(false);
        WaitingForPlayerPanel.SetActive(false);
        PaddleSelectorPanel.SetActive(false);
        GameOverPanel.SetActive(false);

        PlayButton.transform.localScale = Vector3.zero;
        OptionsButton.transform.localScale = Vector3.zero;
        QuitButton.transform.localScale = Vector3.zero;
        volumeSlider.transform.localScale = Vector3.zero;
        vibrToggle.transform.localScale = Vector3.zero;
        BackButton.transform.localScale = Vector3.zero;
        BackPanel.transform.localScale = Vector3.zero;

        RLP_ScrollView.transform.localScale = Vector3.zero;
        RLP_TitleText.transform.localScale = Vector3.zero;
        RLP_CreateRoom.transform.localScale = Vector3.zero;
        RLP_LeaveLobby.transform.localScale = Vector3.zero;

        WFPP_JoinedRoomText.transform.localScale = Vector3.zero;
        WFPP_WaitingText.transform.localScale = Vector3.zero;
        WFPP_LeaveRoomButton.transform.localScale = Vector3.zero;

        PSP_Options.transform.localScale = Vector3.zero;
        PSP_InRoomText.transform.localScale = Vector3.zero;
        PSP_ReadyButton.transform.localScale = Vector3.zero;
        PSP_LeaveRoomButton.transform.localScale = Vector3.zero;

        GOP_WinLoseText.transform.localScale = Vector3.zero;
        GOP_RematchButton.transform.localScale = Vector3.zero;
        GOP_LeaveRoomButton.transform.localScale = Vector3.zero;

        StartTween();
    }

    public void Start() {
        _multiplayer = FindObjectOfType<Multiplayer>();
    }


    public void Play()
    {
        RoomListPanel.SetActive(true);
        RoomListTween();
    }

    public void Options()
    {
        OptionsPanel.SetActive(true);
        OptionsTween();
    }

    public void Back()
    {
        MainMenuPanel.SetActive(true);
        LeanTween.scale(volumeSlider, Vector3.zero, 0.6f).setDelay(.1f).setEase(LeanTweenType.easeInQuart);
        LeanTween.scale(vibrToggle, Vector3.zero, 0.6f).setDelay(.2f).setEase(LeanTweenType.easeInQuart);
        LeanTween.scale(BackButton, Vector3.zero, 0.6f).setDelay(.3f).setEase(LeanTweenType.easeInQuart).setOnComplete(DeactivateOptionsPanel);

        ShowMainMenuButtons();
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
        LeanTween.scale(BackButton, Vector3.one, 0.6f).setDelay(.7f).setEase(LeanTweenType.easeOutCirc)
        .setOnComplete(DisableMainMenu);
    }

    void RoomListTween() {
        HideMainMenuButtons();
        LeanTween.scale(RLP_TitleText, Vector3.one, 0.6f).setDelay(.5f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(RLP_ScrollView, Vector3.one, 0.6f).setDelay(.6f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(RLP_CreateRoom, Vector3.one, 0.6f).setDelay(.6f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(RLP_LeaveLobby, Vector3.one, 0.6f).setDelay(.6f).setEase(LeanTweenType.easeOutCirc);
        //TODO: Add a back button for every screen. Perhaps keep it there when transition from one panel to the next
        LeanTween.scale(BackButton, Vector3.one, 0.6f).setDelay(.7f).setEase(LeanTweenType.easeOutCirc)
        .setOnComplete(DisableMainMenu);
    }

    void WaitingForPlayersTween() {
        HideMainMenuButtons();
    }

    void PaddleSelectorTween() {
        HideMainMenuButtons();
    }

    void GameOverTween() {
        HideMainMenuButtons();
    }


    void DisableMainMenu()
    {
        MainMenuPanel.SetActive(false);
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
        LeanTween.scale(QuitButton, Vector3.zero, 0.6f).setDelay(.2f).setEase(LeanTweenType.easeInQuart);
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

}
