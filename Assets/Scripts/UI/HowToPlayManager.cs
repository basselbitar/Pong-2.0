using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayManager : MonoBehaviour
{
    private int _currentPanelIndex = 0;

    [SerializeField]
    List<GameObject> _pages; 
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ShowCurrentPanel() {
        foreach (var page in _pages) {
            page.SetActive(false);
        }
        _pages[_currentPanelIndex].SetActive(true);
    }
}
