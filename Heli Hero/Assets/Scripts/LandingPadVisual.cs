using UnityEngine;
using TMPro;

public class LandingPadVisual : MonoBehaviour {

    [SerializeField] private TextMeshPro scoreMultiplierTextMesh;

    private void Awake() { 
        LandingPad landingPad = GetComponent<LandingPad>();
        if (landingPad == null) {
            Debug.LogError("LandingPadVisual: LandingPad component not found on " + gameObject.name);
            return;
        }
        
        if (scoreMultiplierTextMesh == null) {
            Debug.LogError("LandingPadVisual: scoreMultiplierTextMesh is not assigned on " + gameObject.name);
            return;
        }
        
        scoreMultiplierTextMesh.text = "x" + landingPad.GetScoreMultiplier();
    }
}