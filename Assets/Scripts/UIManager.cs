using Alteruna;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject WaitingForPlayerPanel;
    [SerializeField]
    private GameObject ReadyCheckPanel;

    private Multiplayer _multiplayer;

    public void Start() {
        _multiplayer = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Multiplayer>();
    }

    public void OnRoomJoined() {
        if(_multiplayer.CurrentRoom.Users.Count < 2) {
            ActivateWaitingForPlayerPanel();
        } else {
            ActivateReadyCheckPanel();
        }
    }

    public void ActivateWaitingForPlayerPanel() {
        WaitingForPlayerPanel.SetActive(true);
        ReadyCheckPanel.SetActive(false);
    }

    public void ActivateReadyCheckPanel() {
        ReadyCheckPanel.SetActive(true);
        WaitingForPlayerPanel.SetActive(false);
    }
}
