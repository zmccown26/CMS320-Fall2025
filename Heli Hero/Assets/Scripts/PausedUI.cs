using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;

    private void Awake() {
        resumeButton.onClick.AddListener(() => {
            GameManager.Instance.ResumeGame();
        });
    }

    private void Start() {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;

        Hide();
    }
    private void GameManager_OnGamePaused(object sender, System.EventArgs e) {
        Show();
    }
    private void GameManager_OnGameResumed(object sender, System.EventArgs e) {
        Hide();
    }
    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
    
}
