using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;

    private void Update() {
        UpdateStatsTextMesh();
    }

        
    private void UpdateStatsTextMesh() {
        if (statsTextMesh == null) {
            return;
        }
        
        if (GameManager.Instance == null || Lander.Instance == null) {
            return;
        }
        
        statsTextMesh.text = 
        GameManager.Instance.GetScore() + "\n" +
        Mathf.Round(GameManager.Instance.GetTime()) + "\n" +
        Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedX() * 10f)) + "\n" +
        Mathf.Abs(Mathf.Round(Lander.Instance.GetSpeedY() * 10f)) + "\n" +
        Lander.Instance.GetFuelAmount();
    }
}
