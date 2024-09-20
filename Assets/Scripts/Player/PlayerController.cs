using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpSpeed = 3f;
    
    
    [Header("Health")]
    public int playerHealth = 3;
    public float damageCooldown = 0.3f;
    
    [Tooltip("Sets the amount of time for it to cool down")]
    public float _damageCooldownTimer;
    
    // Decides if you have DoubleJump, bool for simplicity
    public float jumpCount = 2f;
    
    // Allows interaction with Animator component
    private Animator _animator;
        
    [Header("GroundCheck")]
    public bool playerIsGrounded;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public Vector2 groundBoxSize = new Vector2(0.8f, 0.2f);

    [Header("Audio")] 
    public AudioClip deathSounds;
    public AudioClip[] jumpSounds;
    public AudioClip[] takeDamageSounds;
    [Space(2)]
    public float currentPitchRandom;
    public float pitchRandomTime;
    private float pitchTimer;
    
    [Header("Swinging")]
    public Transform swingingTarget;
    public Transform ropeCheck;
    public LayerMask whatIsRope;
    public HingeJoint2D hingeJoint2D;
    public bool isSwinging;
    public float swingCheckRadius = 0.2f;
    
    
    [Header("Components")]
    private SpriteRenderer _playerSpriteRenderer;
    private InputActions _input;
    private Rigidbody2D _rigidbody2D;
    private Rigidbody2D _parentRigidbody2D;
    private AudioSource _audioSource;
    private Rigidbody2D _ropeRigidbody;
    

    private void Start()
    {
        // At the start, imports InputActions, RigidBody2D, and Animator
        _input = GetComponent<InputActions>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        hingeJoint2D = GetComponent<HingeJoint2D>();
    }

    private void Update()
    {
        if (_input.Horizontal != 0)
        {
            if (_rigidbody2D.linearVelocityX >= 0.0f) 
            {
                _playerSpriteRenderer.flipX = false;
            } 
            else if (_rigidbody2D.linearVelocityX <= 0.0f) 
            {
                _playerSpriteRenderer.flipX = true;
            }
        }
        
        // Uses Physics2D.OverlapBox() to check if the ground and the player's bottom box is overlapping
        playerIsGrounded = Physics2D.OverlapBox(groundCheck.position, groundBoxSize, 0f, whatIsGround);

        if (playerIsGrounded)
        {
            jumpCount = 2f;
        }
        
        // Checks both if jump is possible (Which it always is) and if the player is on the ground
        if (_input.JumpPressed)
        {
            if (playerIsGrounded)
            {
                _rigidbody2D.linearVelocityY = jumpSpeed;
                _audioSource.pitch = Random.Range(0.6f, 0.8f);
                _audioSource.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
                
            }
            else if (jumpCount == 3f)
            {
                _rigidbody2D.linearVelocityY = jumpSpeed;
                jumpCount -= 1;
                _audioSource.pitch = 0.9f;
                _audioSource.PlayOneShot(jumpSounds[Random.Range(1, jumpSounds.Length -1)]);
            }
            else if (jumpCount >= 2f)
            {
                _rigidbody2D.linearVelocityY = jumpSpeed;
                jumpCount -= 1;
                _audioSource.pitch = 1.1f;
                _audioSource.PlayOneShot(jumpSounds[Random.Range(2, jumpSounds.Length)]);
            }
        }
        else if (_input.JumpRelease && !playerIsGrounded && _rigidbody2D.linearVelocity.y > 0f)
        {
            _rigidbody2D.linearVelocityY = 0f;
            _audioSource.Stop();
        }
        UpdateAnimation();
        UpdateSwing();
        Attack();
    }
    
    private void UpdateAnimation()
    {
        if (playerIsGrounded)
        {
            if (_input.Horizontal != 0)
            {
                _animator.Play("Monkey_Walk");
            }
            else
            {
                _animator.Play("Monkey_Idle");
            }
        }
        else
        {
            if (_rigidbody2D.linearVelocityY > 0)
            {
                _animator.Play("Monkey_Jump");
            }
            else
            {
                _animator.Play("Monkey_Fall");
            }
        }
    }

    private void FixedUpdate()
    {
        // FixedUpdate() updates 50 times every second
        if (isSwinging) return;
        _rigidbody2D.linearVelocityX = _input.Horizontal * moveSpeed;
    }

    private void Attack()
    {
        if (!Physics2D.OverlapCircle(
                groundCheck.position,
                0.2f,
                LayerMask.GetMask("Enemy"))) return;
        if (playerIsGrounded == true) return;
        
        // Checks for all circle overlaps with enemy layer
        var enemyColliders = Physics2D.OverlapCircleAll(
            groundCheck.position,
            0.2f,
            LayerMask.GetMask("Enemy"));

        // Destroys every gameObject you collide with
        foreach (var enemy in enemyColliders)
        {

            enemy.GetComponent<FollowEnemyController>().PlayDeath();
            //Destroy(enemy.gameObject);
        }
        
        _rigidbody2D.linearVelocityY = jumpSpeed / 1.1f;



    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(groundCheck.position, groundBoxSize);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(ropeCheck.position, 0.2f);
    }
    
    private void RestartScene() 
    {
        // Gets current scene's name and uses it as input in LoadScene()
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Death"))
        {
            
            RestartScene();
        }

        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = other.transform;
            _parentRigidbody2D = other.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = null;
            _parentRigidbody2D = null;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        if (Time.time > _damageCooldownTimer)
        {
            playerHealth -= 1;
            _damageCooldownTimer = Time.time + damageCooldown;

            _audioSource.pitch = Random.Range(0.8f, 1.2f);
            _audioSource.PlayOneShot(takeDamageSounds[Random.Range(0, takeDamageSounds.Length)]);
        }

        if (playerHealth <= 0)
        {
            
            RestartScene();
        }
    }

    // Updates 50 times a second and detects if two objects have prolonged contact
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

    private void UpdateSwing()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            var other = Physics2D.OverlapCircle(ropeCheck.position, swingCheckRadius, whatIsRope);
            if (other == null) return;
            swingingTarget = other.transform;
            _ropeRigidbody = swingingTarget.GetComponent<Rigidbody2D>();
            transform.parent = other.transform;
            isSwinging = true;
            _ropeRigidbody.linearVelocity = _rigidbody2D.linearVelocity;
            _rigidbody2D.gravityScale = 0; // Disable gravity
            _rigidbody2D.linearVelocity = Vector2.zero;
            jumpCount = 3f;

        }
        else if (isSwinging && (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame))
        {
            transform.parent = null;
            swingingTarget = null;
            isSwinging = false;
            _rigidbody2D.gravityScale = 1;
            _ropeRigidbody = null;

        }

        if (isSwinging)
        {
            _ropeRigidbody.AddForce(transform.right * (_input.Horizontal * 0.05f), ForceMode2D.Impulse);
            transform.position = _ropeRigidbody.transform.position;
        }
    }
}