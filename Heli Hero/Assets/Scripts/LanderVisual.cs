using UnityEngine;

public class LanderVisual : MonoBehaviour {
    [SerializeField] private GameObject landerExplosionVfx;
    private Lander lander;

    private void Awake() {
        lander = GetComponent<Lander>();
    }

    private void Start() {
        lander.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e) {
        switch (e.landingType) {
            case Lander.LandingType.TooFastLanding:
                break;
            case Lander.LandingType.TooSteepAngle:
                break;
            case Lander.LandingType.WrongLandingArea:
                Instantiate(landerExplosionVfx, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                break;
        }
    }
}