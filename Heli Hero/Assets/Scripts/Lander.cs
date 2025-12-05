using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour {

    private const float GRAVITY_NORMAL = 0.7f;

    public static Lander Instance { get; private set;}


    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeLanded;
    public event EventHandler <OnStateChangedEventArgs> OnStateChanged;
     public class OnStateChangedEventArgs : EventArgs{
        public State state;
     }
    public event EventHandler OnCoinPickup;
    public event EventHandler <OnLandedEventArgs> OnLanded;
   

    public class OnLandedEventArgs : EventArgs {
        public LandingType landingType;
        public int score;
        public float dotVector;
        public float landingSpeed;
        public float scoreMultiplier;
    }

    public enum LandingType {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding,
    }

    public enum State{
        WaitingToStart,
        Normal,
        GameOver,
    }

    private Rigidbody2D landerRigidbody2D;
    private float fuelAmount;
    private float fuelAmountMax = 10f;
    private State state;

    private void Awake() {
        Instance = this;
        fuelAmount = fuelAmountMax;
        landerRigidbody2D = GetComponent<Rigidbody2D>();
        landerRigidbody2D.gravityScale = 0f;
        state = State.WaitingToStart;
    }

    private void FixedUpdate() {
        switch (state) {
            default:
            case State.WaitingToStart:
                if (Keyboard.current.upArrowKey.isPressed || 
                    Keyboard.current.leftArrowKey.isPressed || 
                    Keyboard.current.rightArrowKey.isPressed) {
                    landerRigidbody2D.gravityScale = GRAVITY_NORMAL;
                    state = State.Normal;      
                setState(State.Normal);
                }
                break;
            case State.Normal:
                if (fuelAmount <= 0f) {
                    return;
                }

                if (Keyboard.current.upArrowKey.isPressed || 
                    Keyboard.current.leftArrowKey.isPressed || 
                    Keyboard.current.rightArrowKey.isPressed) {
                    consumeFuel();      
                }

                if (Keyboard.current.upArrowKey.isPressed) {
                    float force = 700f;
                    landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
                }
                if (Keyboard.current.leftArrowKey.isPressed) {
                    float turnSpeed = +100f;
                    landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
                }
                if (Keyboard.current.rightArrowKey.isPressed) {
                    float turnSpeed = -100f;
                    landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D) {
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad)) {
            Debug.Log("Crashed on terrain");
            OnLanded?.Invoke(this, new OnLandedEventArgs{
                landingType = LandingType.WrongLandingArea,
                dotVector = 0f,
                landingSpeed = 0f,
                scoreMultiplier = 0,
                score = 0,
            });
            setState(State.GameOver);
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        float minDotVector = .9f;
        if (dotVector < minDotVector) {
            Debug.Log("Landed on a bad angle");
            OnLanded?.Invoke(this, new OnLandedEventArgs{
                landingType = LandingType.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                score = 0,
            });
            setState(State.GameOver);
            return;
        }

        float softLandingVecloityMagnitude = 4f;
        if (relativeVelocityMagnitude > softLandingVecloityMagnitude) {
            Debug.Log("Landed too fast");
            OnLanded?.Invoke(this, new OnLandedEventArgs{
                landingType = LandingType.TooFastLanding,
                dotVector = 0f,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                score = 0,
            });
            setState(State.GameOver);
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
            landingType = LandingType.Success,
            dotVector = dotVector,
            landingSpeed = relativeVelocityMagnitude,
            scoreMultiplier = landingPad.GetScoreMultiplier(),
            score = score
        });
        setState(State.GameOver);
    }

    private void OnTriggerEnter2D(Collider2D collider2D) {

        if (collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup)) {
            float addFuelAmount = 10f;
            fuelAmount += addFuelAmount;
            if (fuelAmount > fuelAmountMax) {
                fuelAmount = fuelAmountMax;
            }
            fuelPickup.DestroySelf();
        }

        if (collider2D.gameObject.TryGetComponent(out CoinPickup coinPickup)) {

            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.DestroySelf();
        }
    }
    private void setState(State state){
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
            state = state,
        });
    }
    private void consumeFuel() {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.fixedDeltaTime;
        if (fuelAmount < 0f) {
            fuelAmount = 0f;
        }
    }

    public float GetFuelAmount() {
        return fuelAmount;
    }

    public float GetFuelAmountNormalized() {
        return Mathf.Clamp01(fuelAmount / fuelAmountMax);
    }

    public float GetSpeedX() {
        if (landerRigidbody2D == null) {
            return 0f;
        }
        return landerRigidbody2D.linearVelocity.x;
    }

    public float GetSpeedY() {
        if (landerRigidbody2D == null) {
            return 0f;
        }
        return landerRigidbody2D.linearVelocity.y;
    }

    public bool IsInNormalState() {
        return state == State.Normal;
    }

    public void CrashFromHazard() {
    // Avoid double-firing if already game over
    if (state == State.GameOver) return;

    Debug.Log("Crashed from hazard (missile/turret)");

    OnLanded?.Invoke(this, new OnLandedEventArgs {
        landingType = LandingType.WrongLandingArea, // reuse existing type
        dotVector = 0f,
        landingSpeed = 0f,
        scoreMultiplier = 0,
        score = 0,
    });

    setState(State.GameOver);
}
}
