using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeOption : MonoBehaviour {
    [SerializeField]
    private int _index;

    public static List<int> selectedGameModes = new();

    [SerializeField]
    private Button _readyButton;

    [SerializeField]
    private GameModeOption[] _gameModeOptions;

    private bool clicked;

    private Color _selectedColor = new(0.412f, 0.651f, 0.463f);
    private Color _deselectedColor = new(1f, 1f, 1f);

    public void OnButtonClicked() {
        clicked = !clicked;
        if (clicked) {
            OnGameModeSelected();
        }
        else {
            OnGameModeDeselected();
        }
    }

    private void OnGameModeSelected() {

        if (selectedGameModes.Count >= 2) {
            //Debug.Log("we must first remove: " + _gameModeOptions[selectedGameModes[0] - 1]._index);
            _gameModeOptions[selectedGameModes[0] - 1].OnGameModeDeselected();
        }

        this.clicked = true;
        selectedGameModes.Add(_index);
        var colors = GetComponent<Button>().colors;
        colors.normalColor = _selectedColor;
        colors.selectedColor = _selectedColor;
        colors.highlightedColor = _selectedColor;
        GetComponent<Button>().colors = colors;
        //PrintGameModeList();
    }

    public void OnGameModeDeselected() {
        this.clicked = false;
        selectedGameModes.Remove(_index);

        var colors = GetComponent<Button>().colors;
        colors.normalColor = _deselectedColor;
        colors.selectedColor = _deselectedColor;
        colors.highlightedColor = _deselectedColor;
        GetComponent<Button>().colors = colors;
        //PrintGameModeList();
    }
    private void PrintGameModeList() {
        string result = "current modes:";
        result += PrintArray(selectedGameModes);
        Debug.Log(result);
    }

    public static string PrintArray(List<int> list) {
        string result = "";
        foreach (var item in list) {
            result += item.ToString() + ", ";
        }
        return result;
    }
}
