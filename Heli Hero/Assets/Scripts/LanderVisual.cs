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
            case Lander.LandingType.TooSteepAngle:
            case Lander.LandingType.WrongLandingArea:
            //crash!
                GameObject explosion = Instantiate(landerExplosionVfx, transform.position, Quaternion.identity);
                ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
                ps.Play();
                gameObject.SetActive(false);
                break;
        }
    }
}