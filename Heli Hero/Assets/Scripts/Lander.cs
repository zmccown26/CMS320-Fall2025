using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour {

    public static Lander Instance { get; private set;}


    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeLanded;
    public event EventHandler OnCoinPickup;
    public event EventHandler <OnLandedEventArgs> OnLanded;

    public class OnLandedEventArgs : EventArgs {
        public int score;
    }

    private Rigidbody2D landerRigidbody2D;
    private float fuelAmount = 10f;

    private void Awake() {
        Instance = this;
        landerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {


        if(fuelAmount <= 0f){
            Debug.Log("No fuel");
            return;
        }

        if (Keyboard.current.upArrowKey.isPressed) {
            float force = 700f;
            landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
            consumeFuel();
        }
        if (Keyboard.current.leftArrowKey.isPressed) {
            float turnSpeed = +100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
            consumeFuel();
        }
        if (Keyboard.current.rightArrowKey.isPressed) {
            float turnSpeed = -100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
            consumeFuel();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D) {
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad)) {
            Debug.Log("Crashed on terrain");
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = .9f;
        if (dotVector < minDotVector) {
            Debug.Log("Landed on a bad angle");
            return;
        }

        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        float softLandingVecloityMagnitude = 4f;
        if (relativeVelocityMagnitude > softLandingVecloityMagnitude) {
            Debug.Log("Landed too fast");
            return;
        }

        Debug.Log("Landed successfully");

        float maxScoreAmountLandingAngle = 100;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;

        float maxScoreAmountLandingSpeed = 100;
        float landingSpeedScore = (softLandingVecloityMagnitude - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;

        Debug.Log("Debug - dotVector: " + dotVector);
        Debug.Log("Debug - relativeVelocityMagnitude: " + relativeVelocityMagnitude);
        Debug.Log("Landing speed score: " + landingSpeedScore);
        Debug.Log("Landing Angle Score: " + landingAngleScore);
        Debug.Log("Debug - scoreMultiplier: " + landingPad.GetScoreMultiplier());
        Debug.Log("Debug - sum before multiply: " + (landingAngleScore + landingSpeedScore));

        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.GetScoreMultiplier());

        Debug.Log("Score: " + score);
        OnLanded?.Invoke(this, new OnLandedEventArgs{
            score = score
        });
    }

    private void OnTriggerEnter2D(Collider2D collider2D) {

        if (collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup)) {
            float addFuelAmount = 10f;
            fuelAmount += addFuelAmount;
            fuelPickup.DestroySelf();
        }

        if (collider2D.gameObject.TryGetComponent(out CoinPickup coinPickup)) {

            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.DestroySelf();
        }
    }

    private void consumeFuel() {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }

    public float GetFuelAmount() {
        return fuelAmount;
    }

    public float GetSpeedX() {
        return landerRigidbody2D.LinearVelocityX;
    }

    public float GetSpeedY() {
        return landerRigidbody2D.linearVelocityY;
    }
}
