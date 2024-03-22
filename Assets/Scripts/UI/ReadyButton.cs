using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyButton : MonoBehaviour
{
    private Alteruna.Avatar _avatar;
    // Start is called before the first frame update
    void Start()
    {
        _avatar = GetComponent<Alteruna.Avatar>();
        if (!_avatar.IsMe) {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnReadyClicked() {
        Debug.Log("Ready has been clicked by");
        Debug.Log(_avatar.IsMe);
        Debug.Log(_avatar);
    }
}
