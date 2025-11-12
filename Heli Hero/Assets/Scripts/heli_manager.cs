using UnityEngine;
using UnityEngine.InputSystem;

public class heli : MonoBehaviour
{
    private Rigidbody2D heliRigidbody2D;

    private void Awake () {
        heliRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate( ) {
        if (Keyboard.current.upArrowKey.isPressed) {
            float force = 700f;
            heliRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
              }
        if (Keyboard.current.leftArrowKey.isPressed) {
            float turnSpeed = +100f;
            heliRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
       }
        if (Keyboard.current.rightArrowKey.isPressed) {
            float turnSpeed = -100f;
            heliRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);       }
    }
}
