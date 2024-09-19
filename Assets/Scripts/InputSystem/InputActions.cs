using System;
using UnityEngine;

public class InputActions : MonoBehaviour
{
    // VERY boilerplate input script, it work
    
    private InputSystem_Actions _inputSystem;

    // He gonna explain
    public float Horizontal;
    
    // He gonna explain
    public bool Jump;

    private void Update()
    {
        // Getting info from movescript, setting it to Horizontal var
        Horizontal = _inputSystem.Player.Move.ReadValue<Vector2>().x;
        
        // Same as Horizontal but for Jump
        Jump = _inputSystem.Player.Jump.WasPressedThisFrame();
    }

    private void Awake() { _inputSystem = new InputSystem_Actions(); }

    private void OnEnable() { _inputSystem.Enable(); }
    private void OnDisable() { _inputSystem.Disable(); }
}
