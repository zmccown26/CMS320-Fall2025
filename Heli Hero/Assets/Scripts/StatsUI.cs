using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject speedUpArrowGameObject;
    [SerializeField] private GameObject speedDownArrowGameObject;
    [SerializeField] private GameObject speedLeftArrowGameObject;
    [SerializeField] private GameObject speedRightArrowGameObject;
    [SerializeField] private Image fuelImage;

    private void Update() {
        UpdateStatsTextMesh();
    }

        
    private void UpdateStatsTextMesh() {
        
        if (GameManager.Instance == null || Lander.Instance == null) {
            return;
        }

        if (speedUpArrowGameObject != null) {
            speedUpArrowGameObject.SetActive(Lander.Instance.GetSpeedY() >= 0);
        }
        if (speedDownArrowGameObject != null) {
            speedDownArrowGameObject.SetActive(Lander.Instance.GetSpeedY() < 0);
        }
        if (speedLeftArrowGameObject != null) {
            speedLeftArrowGameObject.SetActive(Lander.Instance.GetSpeedX() < 0);
        }
        if (speedRightArrowGameObject != null) {
            speedRightArrowGameObject.SetActive(Lander.Instance.GetSpeedX() >= 0);
        }

        if (fuelImage != null) {
            float normalizedFuel = Lander.Instance.GetFuelAmountNormalized();
            fuelImage.fillAmount = Mathf.Clamp01(normalizedFuel);
        }

        if (statsTextMesh == null) {
            return;
        }

        statsTextMesh.text = 
        GameManager.Instance.GetScore() + "\n" +
        Mathf.Round(GameManager.Instance.GetTime()) + "\n" +
        Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedX() * 10f)) + "\n" +
        Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedY() * 10f));
    }
}
