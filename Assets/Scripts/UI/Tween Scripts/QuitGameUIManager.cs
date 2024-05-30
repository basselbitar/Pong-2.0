using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGameUIManager : MonoBehaviour {
    [SerializeField] GameObject yesOrNoPanel, yesBtn, noBtn, quitBtn;
    [SerializeField] RectTransform quitCanvas, hoverArea;

    private TweenUIManager _tweenUIManager;

    private bool enableQuitButton, panelOpen;

    public bool gameStarted;
    void Awake() {
        HideEverything();

    }
    void Start() {
        enableQuitButton = false;
        _tweenUIManager = FindObjectOfType<TweenUIManager>();
        //LeanTween.scale(gameText, new Vector3(1f, 1f, 1f), .5f).setEase(LeanTweenType.easeOutCirc);
    }

    private void Update() {
        // y needs to be bottom 30% of screen height
        // x needs to be in the middle of the screen (15% leeway on each side)

        gameStarted = _tweenUIManager.gameStarted;
        //when game finishes, close the yes or no panel if it's open
        if (!gameStarted) {
            if (yesOrNoPanel.transform.localScale.x > 0.1) {
                HideEverything();
                Debug.LogError("Hide the Yes/No Panel for quitting the game");
            }
            return;
        }
        if (panelOpen) return;

        if (Input.mousePosition.y < 0.3f * quitCanvas.rect.height && Mathf.Abs(Input.mousePosition.x - (quitCanvas.rect.width / 2f)) / quitCanvas.rect.width < 0.10f) {
            if (!enableQuitButton) {

                enableQuitButton = true;
                ShowQuitButton();
            }
        }
        else {
            if (enableQuitButton) {
                enableQuitButton = false;
                HideQuitButton();
            }
        }
    }
    public void ShowQuitButton() {
        LeanTween.scale(quitBtn, Vector3.one, .5f).setEase(LeanTweenType.easeOutCirc);
    }

    public void HideQuitButton() {
        LeanTween.scale(quitBtn, Vector3.zero, .5f).setEase(LeanTweenType.easeOutCirc);

    }

    public void HideEverything() {
        yesOrNoPanel.transform.localScale = Vector3.zero;
        yesBtn.transform.localScale = Vector3.zero;
        noBtn.transform.localScale = Vector3.zero;
        quitBtn.transform.localScale = Vector3.zero;
        panelOpen = false;
    }

    public void QuitButton() {
        PanelEnable();
    }

    public void YesButton() {
        LeanTween.scale(yesBtn, Vector3.zero, .5f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(noBtn, Vector3.zero, .5f).setDelay(.1f).setEase(LeanTweenType.easeOutCirc);
        //LeanTween.moveLocal(yesOrNoPanel, new Vector3(0f, -615f, 0f), 0.5f).setDelay(.1f).setEase(LeanTweenType.easeInQuart);
        LeanTween.scale(yesOrNoPanel, Vector3.zero, .5f).setDelay(.1f).setEase(LeanTweenType.easeInQuart)
        .setOnComplete(LoadMainMenu);
        panelOpen = false;
    }
    public void NoButton() {

        LeanTween.scale(yesBtn, Vector3.zero, .5f).setDelay(.1f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(noBtn, Vector3.zero, .5f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.moveLocal(yesOrNoPanel, new Vector3(0f, -615f, 0f), 0.5f).setDelay(.1f).setEase(LeanTweenType.easeInQuart);
        LeanTween.scale(yesOrNoPanel, Vector3.zero, .5f).setDelay(.1f).setEase(LeanTweenType.easeInQuart);
        //LeanTween.scale(quitBtn, Vector3.one, .5f).setDelay(.7f).setEase(LeanTweenType.easeOutCirc);
        panelOpen = false;
    }


    void PanelEnable() {
        LeanTween.scale(quitBtn, Vector3.zero, .5f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.moveLocal(yesOrNoPanel, Vector3.zero, 0.5f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(yesOrNoPanel, new Vector3(2f, 2f, 1f), .5f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(yesBtn, new Vector3(1.5f, 1.5f, 1.5f), .5f).setDelay(.3f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(noBtn, new Vector3(1.5f, 1.5f, 1.5f), .5f).setDelay(.4f).setEase(LeanTweenType.easeOutCirc);
        panelOpen = true;
    }



    void LoadMainMenu() {
        FindObjectOfType<TweenUIManager>().ShowBigPanel();
        FindObjectOfType<TweenUIManager>().StartTween();
    }
}
