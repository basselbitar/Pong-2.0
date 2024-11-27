using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PaddleOption : MonoBehaviour, ISelectHandler, IDeselectHandler {
    [SerializeField]
    private int _index;

    public static int selectedOption = 0;

    [SerializeField]
    private Button _readyButton;

    [SerializeField]
    private PaddleOption[] _paddleOptions;

    [SerializeField]
    private GameObject _frame;

    private bool clicked;

    private Color _selectedColor = new(0.57f, 1f, 0.66f);
    private Color _deselectedColor = new(1f, 1f, 1f);
    private Color _hoveredColor = new(0.76f, 0.78f, 0.384f);


    public void OnButtonClicked() {
        clicked = !clicked;
        if(clicked) {
            ClearOptions();
            clicked = true;
            OnPaddleOptionSelected();
        } else {
            OnPaddleOptionDeselected();
        }
    }

    public void OnSelect(BaseEventData eventData) {
        _frame.SetActive(true);
        if (selectedOption == 0) {
            _frame.GetComponent<Image>().color = Color.yellow;
        }
    }

    public void OnDeselect(BaseEventData eventData) {
        if (selectedOption != _index) {
            _frame.SetActive(false);
        }
    }

    private void OnPaddleOptionSelected() {
        selectedOption = _index;
        _frame.SetActive(true);
        _frame.GetComponent<Image>().color = Color.green;

        //var colors = GetComponent<Button>().colors;
        //colors.normalColor = _selectedColor;
        //colors.selectedColor = _selectedColor;
        //colors.highlightedColor = _selectedColor;
        //GetComponent<Button>().colors = colors;
    }

    private void OnPaddleOptionDeselected() {
        selectedOption = 0;
        _frame.SetActive(false);
        //var colors = GetComponent<Button>().colors;
        //colors.normalColor = _deselectedColor;
        //colors.selectedColor = _hoveredColor;
        //colors.highlightedColor = _deselectedColor;
        //GetComponent<Button>().colors = colors;
    }

    public void ClearOptions() {
        foreach (var item in _paddleOptions) {
            item.SetClicked(false);
            item.OnPaddleOptionDeselected();
        }
    }

    public void SetClicked(bool clicked) {
        this.clicked = clicked;
    }
}