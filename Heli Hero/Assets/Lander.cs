using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private Rigidbody2D landerRigidbody2D;

    private void Awake () {
        landerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate( ) {
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
    }

    private void OnCollisionEnter2D(Collision2D collision2D) {
            if(!collision2D.gameObject.TryGetComponent(out LandingPad landingPad)){
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
            if(relativeVelocityMagnitude > softLandingVecloityMagnitude) {
                Debug.Log("Landed too fast");
                return;
            }

            Debug.Log("Landed successfully");


            float maxScoreAmountLandingAngle = 100;
            float scoreDotVectorMultiplier = 10f;
            float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;


            float maxScoreAmountLandingSpeed = 100;
            float landingSpeedScore = (softLandingVecloityMagnitude - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;
            Debug.Log("Landing speed score: " + landingSpeedScore);
            Debug.Log("Landing Angle Score: " + landingAngleScore);
    }
}
