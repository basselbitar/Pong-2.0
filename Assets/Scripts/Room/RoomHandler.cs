using Alteruna;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    private Multiplayer _multiplayer;

    public TMP_Text joinedRoomText;
    public TMP_Text inRoomText;

    // Start is called before the first frame update
    void Start()
    {
        _multiplayer = FindObjectOfType<Multiplayer>();
        _multiplayer.OnRoomJoined.AddListener(JoinedRoom);

        //TODO: check how to pass JoinedRoom  to the function (check RoomMenu line 51)
    }
    
    public void OnLeaveRoomClicked() {
        if (_multiplayer != null) {
            _multiplayer.CurrentRoom?.Leave();
        }
    }

    private void JoinedRoom(Multiplayer multiplayer, Room room, User user) {
        UpdateJoinedRoomText(room);
        UpdateInRoomText(room);
    }

    private void UpdateJoinedRoomText(Room room) {
        joinedRoomText.text = "Joined Room: " + room.Name;
    }

    private void UpdateInRoomText(Room room) {
        inRoomText.text = "In Room: " + room.Name;
    }
}
