using Alteruna;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject RoomMenuPanel;
    [SerializeField]
    private GameObject WaitingForPlayerPanel;
    [SerializeField]
    private GameObject ReadyCheckPanel;
    [SerializeField]
    private GameObject GameOverPanel;

    private Multiplayer _multiplayer;

    public void Start() {
        _multiplayer = FindObjectOfType<Multiplayer>();
    }


    public void OnOtherPlayerRoomLeft() {
        ActivateWaitingForPlayerPanel();
    }

    public void OnRoomJoined() {
        if(_multiplayer.CurrentRoom.Users.Count < 2) {
            ActivateWaitingForPlayerPanel();
        } else {
            ActivateReadyCheckPanel();
        }
    }

    public void ActivateRoomMenuPanel() {
        DeactivatePanels();
        // TODO:make the Tween Manager the new UI Manager
        // RoomMenuPanel.SetActive(true);
    }

    public void ActivateWaitingForPlayerPanel() {
        DeactivatePanels();
        WaitingForPlayerPanel.SetActive(true);
    }

    public void DeactivatePanels() {
        RoomMenuPanel.SetActive(false);
        WaitingForPlayerPanel.SetActive(false);
        ReadyCheckPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }

    public void ActivateReadyCheckPanel() {
        DeactivatePanels();
        ReadyCheckPanel.SetActive(true);
        FindObjectOfType<ReadyButton>().Initialize();
    }

    public void ActivateGameOverPanel(int winnerIndex) {
        DeactivatePanels();
        GameOverPanel.SetActive(true);
        if(winnerIndex == _multiplayer.Me.Index) {
            GameOverPanel.GetComponentInChildren<TMP_Text>().text = "You Win!";
        }else {
            GameOverPanel.GetComponentInChildren<TMP_Text>().text = "You Lose!";
        }
    }
}
