using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameModeOption : MonoBehaviour, ISelectHandler, IDeselectHandler {
    [SerializeField]
    private int _index;

    public static List<int> selectedGameModes = new();

    [SerializeField]
    private Button _readyButton;

    [SerializeField]
    private GameModeOption[] _gameModeOptions;

    private bool clicked;

    [SerializeField]
    private GameObject _frame;

    //private Color _selectedColor = new(0.57f, 1f, 0.66f);
    private Color _selectedColor = new(1f, 1f, 1f);
    //private Color _deselectedColor = new(1f, 1f, 1f);
    private Color _deselectedColor = new(0.7176f, 0.7176f, 0.7176f);
    private Color _hoveredColor = new(0.76f, 0.78f, 0.384f);

    public void OnButtonClicked() {
        clicked = !clicked;
        if (clicked) {
            OnGameModeSelected();
        }
        else {
            OnGameModeDeselected();
        }
    }

    public void OnSelect(BaseEventData eventData) {
        _frame.SetActive(true);
        if (selectedGameModes.IndexOf(_index) == -1) {
            _frame.GetComponent<Image>().color = Color.yellow;
        }
    }

    public void OnDeselect(BaseEventData eventData) {
        if (selectedGameModes.IndexOf(_index) == -1) {
            _frame.SetActive(false);
        }
    }


    private void OnGameModeSelected() {

        if (selectedGameModes.Count >= 2) {
            //Debug.Log("we must first remove: " + _gameModeOptions[selectedGameModes[0] - 1]._index);
            _gameModeOptions[selectedGameModes[0] - 1].OnGameModeDeselected();
        }

        this.clicked = true;
        selectedGameModes.Add(_index);

        _frame.SetActive(true);
        _frame.GetComponent<Image>().color = Color.green;


        var colors = GetComponent<Button>().colors;
        //colors.normalColor = _selectedColor;
        //colors.selectedColor = _selectedColor;
        //colors.highlightedColor = _selectedColor;
        //GetComponent<Button>().colors = colors;
        //PrintGameModeList();
    }

    public void OnGameModeDeselected() {
        this.clicked = false;
        selectedGameModes.Remove(_index);

        _frame.SetActive(false);

        var colors = GetComponent<Button>().colors;
        //colors.normalColor = _deselectedColor;
        //colors.selectedColor = _hoveredColor;
        //colors.highlightedColor = _deselectedColor;
        //GetComponent<Button>().colors = colors;
        //PrintGameModeList();
    }


    public void ClearGameModeOptions() {
        foreach (var item in _gameModeOptions) {
            item.clicked = false;
            item.OnGameModeDeselected();
        }
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
