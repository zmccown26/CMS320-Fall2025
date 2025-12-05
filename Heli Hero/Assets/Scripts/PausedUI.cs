using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    private void Awake() {
        resumeButton.onClick.AddListener(() => {
            GameManager.Instance.ResumeGame();
        });
        
        quitButton.onClick.AddListener(() => {
            // Resume game first to restore timeScale
            GameManager.Instance.ResumeGame();
            // Then load main menu by scene index (0 = Main_menu)
            SceneManager.LoadScene(0);
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
