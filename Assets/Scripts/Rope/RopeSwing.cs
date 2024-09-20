using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSwing : MonoBehaviour
{
    public float swingForce = 10f; // Force applied when swinging
    public Transform rope; // Reference to the rope transform
    public Rigidbody2D rb; // Rigidbody of the player
    public bool isSwinging = false;
    public Transform swingCheck;
    public float swingRadius = 2f;
    public LayerMask whatIsRope;

    void Update()
    {
        isSwinging = Physics2D.OverlapCircle(swingCheck.position, swingRadius, whatIsRope);
        // Check for input to start swinging
        if (Input.GetKeyDown(KeyCode.Space) && isSwinging)
        {
            StartSwing();
            print("Swing");
        }

        // Stop swinging
        if (Input.GetKeyDown(KeyCode.E) && isSwinging)
        {
            StopSwing();
        }
        
    }
    private void StartSwing()
    {
        isSwinging = true;
        rb.isKinematic = false; // Enable physics
        rb.gravityScale = 0; // Disable gravity
        rb.linearVelocity = Vector2.zero; // Reset velocity
        
        // Add a hinge joint to create the swinging effect
        HingeJoint2D hinge = gameObject.AddComponent<HingeJoint2D>();
        hinge.connectedBody = rope.GetComponent<Rigidbody2D>();
        hinge.anchor = Vector2.zero; // Adjust the anchor if needed

        // Set the swing force
        hinge.useMotor = true;
        JointMotor2D motor = hinge.motor;
        motor.motorSpeed = swingForce;
        hinge.motor = motor;
    }

    private void StopSwing()
    {
        isSwinging = false;
        rb.gravityScale = 1; // Enable gravity again
        Destroy(GetComponent<HingeJoint2D>()); // Remove the hinge joint
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(swingCheck.position, swingRadius);
    }
}

