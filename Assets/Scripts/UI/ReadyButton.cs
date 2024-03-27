using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyButton : MonoBehaviour
{
    private Multiplayer _multiplayer;
    private Paddle _player;

    public void Start() {
        _multiplayer = FindObjectOfType<Multiplayer>();
        Debug.Log(_multiplayer.Me.Name);
        _player = GameObject.Find("Paddle (" + _multiplayer.Me.Name + ")").GetComponent<Paddle>();
        Debug.Log(_player);
    }


    void Update()
    {
        
    }

    public void OnReadyClicked() {
        Debug.Log("Ready has been clicked");
        _player.SetReady(true);
        Debug.Log(_player.IsReady());
    }
}
