using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {

    public static GameInput Instance { get; private set; }

    public event EventHandler OnMenuButtonPressed;

    private InputSystem_Actions inputActions;

    private void Awake() {
        Instance = this;

        inputActions = new InputSystem_Actions();
        inputActions.Enable();

        inputActions.Player.MenuAction.performed += Menu_performed;
    }

    private void Menu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnMenuButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy() {
        inputActions.Disable();
    }
}