using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    private Vector2 _moveDirection;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInput();
        _rigidbody2D.AddForce(transform.right * _moveDirection.x, ForceMode2D.Impulse);
    }

    private void GetInput()
    {
        _moveDirection.x = (Keyboard.current.aKey.isPressed ? -1 : 0) + (Keyboard.current.dKey.isPressed ? 1 : 0);
        _moveDirection.y = (Keyboard.current.sKey.isPressed ? -1 : 0) + (Keyboard.current.wKey.isPressed ? 1 : 0);
    }
}