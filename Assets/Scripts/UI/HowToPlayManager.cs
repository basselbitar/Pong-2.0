using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayManager : MonoBehaviour
{
    private int _currentPanelIndex = 0;

    [SerializeField]
    List<GameObject> _pages;

    [SerializeField]
    Button Objective, Controls, PowerUps, GameModes;

    [Space(15)]
    [SerializeField]
    GameObject HowToPlayPanel;

    // Start is called before the first frame update
    void Update()
    {
        if(!HowToPlayPanel.activeSelf) {
            return;
        }

        if(Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D)) {
            NextPage();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A)) {
            PreviousPage();
        }
    }
    public void ShowCurrentPanel() {
        foreach (var page in _pages) {
            page.SetActive(false);
        }
        _pages[_currentPanelIndex].SetActive(true);

        if (_currentPanelIndex == 0) { Objective.Select(); }
        else if (_currentPanelIndex == 1) { Controls.Select(); }
        else if (_currentPanelIndex >= 2 && _currentPanelIndex < 9) { PowerUps.Select(); } 
        else if(_currentPanelIndex >=0) { GameModes.Select(); }
    }

    public void PreviousPage() {
        if(_currentPanelIndex <= 0) {
            _currentPanelIndex = 0;
            return;
        }
        _currentPanelIndex--;
        ShowCurrentPanel();
    }

    public void NextPage() {
        if (_currentPanelIndex == _pages.Count - 1) {
            _currentPanelIndex = _pages.Count - 1;
            return;
        }
        _currentPanelIndex++;
        ShowCurrentPanel();
    }

    public void SetCurrentPanelIndex(int n) {
        _currentPanelIndex = n;
        ShowCurrentPanel();
    }
}
