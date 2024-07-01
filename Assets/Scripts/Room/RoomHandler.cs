using Alteruna;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    private Multiplayer _multiplayer;

    public TMP_Text WFPP_JoinedRoomText; //waiting for players panel
    public TMP_Text GMSP_InRoomText;
    public TMP_Text PSP_InRoomText;

    // Start is called before the first frame update
    void Start()
    {
        _multiplayer = FindObjectOfType<Multiplayer>();
        _multiplayer.OnRoomJoined.AddListener(JoinedRoom);
    }
    
    // offline gameplay functions
    public void SetOfflineRoomText() {
        if(PlayMode.selectedPlayMode == PlayMode.PlayModeType.PlayLocal) {
            GMSP_InRoomText.text = "Local Multiplayer";
            PSP_InRoomText.text = "Player 1 Choose a Paddle";
        } else if(PlayMode.selectedPlayMode == PlayMode.PlayModeType.PlayVsPC) {
            GMSP_InRoomText.text = "Play VS PC";
            PSP_InRoomText.text = "Play VS PC";
        }
    }

    public void SetPlayer2Turn() {
        PSP_InRoomText.text = "Player 2 Choose a Paddle";
    }

    // online gameplay functions
    public void OnLeaveRoomClicked() {
        if (_multiplayer != null) {
            _multiplayer.CurrentRoom?.Leave();
            FindObjectOfType<GameModeManager>().Initialize();
        }
    }

    private void JoinedRoom(Multiplayer multiplayer, Room room, User user) {
        UpdateJoinedRoomText(room);
        UpdateInRoomText(room);
    }

    private void UpdateJoinedRoomText(Room room) {
        WFPP_JoinedRoomText.text = "Joined Room: " + room.Name;
    }

    private void UpdateInRoomText(Room room) {
        PSP_InRoomText.text = "In Room: " + room.Name;
        GMSP_InRoomText.text = "In Room: " + room.Name;
    }
}
