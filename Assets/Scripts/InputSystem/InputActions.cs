using System;
using UnityEngine;
using UnityEngine.Serialization;

public class InputActions : MonoBehaviour
{
    private InputSystem_Actions _inputSystem;

    public float Horizontal;
    public bool JumpPressed;
    public bool JumpRelease;

    private void Update()
    {
        Horizontal = _inputSystem.Player.Move.ReadValue<Vector2>().x;
        JumpPressed = _inputSystem.Player.Jump.WasPressedThisFrame();
        JumpRelease = _inputSystem.Player.Jump.WasReleasedThisFrame();
    }

    private void Awake() { _inputSystem = new InputSystem_Actions(); }
    private void OnEnable() { _inputSystem.Enable(); }
    private void OnDisable() { _inputSystem.Disable(); }
}
