using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaddleOption : MonoBehaviour
{
    [SerializeField]
    private int _index;

    public static int selectedOption = 0;

    [SerializeField]
    private Button _readyButton;

    [SerializeField]
    private PaddleOption[] _paddleOptions;

    private bool clicked;

     private Color _selectedColor = new Color(0.412f, 0.651f, 0.463f);
    private Color _deselectedColor = new Color(1f, 1f, 1f);

    public void OnButtonClicked() {
        clicked = !clicked;
        if(clicked) {
            foreach (var item in _paddleOptions) {
                item.SetClicked(false);
                item.OnPaddleOptionDeselected();
            }
            clicked = true;
            OnPaddleOptionSelected();
        } else {
            OnPaddleOptionDeselected();
        }
        Debug.LogError("Selected option is now: " + selectedOption);
    }

    private void OnPaddleOptionSelected() {
        selectedOption = _index;
        var colors = GetComponent<Button>().colors;
        colors.normalColor = _selectedColor;
        colors.selectedColor = _selectedColor;
        colors.highlightedColor = _selectedColor;
        GetComponent<Button>().colors = colors;
    }

    private void OnPaddleOptionDeselected() {
        selectedOption = 0;
        var colors = GetComponent<Button>().colors;
        colors.normalColor = _deselectedColor;
        colors.selectedColor = _deselectedColor;
        colors.highlightedColor = _deselectedColor;
        GetComponent<Button>().colors = colors;
    }

    public void SetClicked(bool clicked) {
        this.clicked = clicked;
    }
}