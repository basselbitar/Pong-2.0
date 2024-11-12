using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyToggle : MonoBehaviour
{
    [SerializeField]
    private int _difficultyValue;

    public void OnOptionSelected() {
        PlayerPrefs.SetInt("Difficulty", _difficultyValue);
    }
}
