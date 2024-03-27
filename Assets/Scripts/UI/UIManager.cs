using Alteruna;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject RoomMenuPanel;
    [SerializeField]
    private GameObject WaitingForPlayerPanel;
    [SerializeField]
    private GameObject ReadyCheckPanel;


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
        RoomMenuPanel.SetActive(true);
        WaitingForPlayerPanel.SetActive(false);
        ReadyCheckPanel.SetActive(false);
    }

    public void ActivateWaitingForPlayerPanel() {
        RoomMenuPanel.SetActive(false);
        WaitingForPlayerPanel.SetActive(true);
        ReadyCheckPanel.SetActive(false);
    }

    public void DeactivatePanels() {
        RoomMenuPanel.SetActive(false);
        WaitingForPlayerPanel.SetActive(false);
        ReadyCheckPanel.SetActive(false);
    }

    public void ActivateReadyCheckPanel() {
        RoomMenuPanel.SetActive(false);
        ReadyCheckPanel.SetActive(true);
        WaitingForPlayerPanel.SetActive(false);
        FindObjectOfType<ReadyButton>().Initialize();
    }

    public void OnQuit() {
        Application.Quit();
    }
}
