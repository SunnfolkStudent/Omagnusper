using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float moveSpeed;
   
    public LayerMask whatIsWall;
    public Transform wallCheck;
    public Transform fallCheck;
    
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (DetectedWallOrFall())
        {
            moveSpeed *= -1;
            transform.localScale  = new Vector2(transform.localScale.x * -1f, 0.65f);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocityX = moveSpeed;
    }

    private bool DetectedWallOrFall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.1f, whatIsWall) 
               || !Physics2D.OverlapCircle(fallCheck.position, 0.1f);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(wallCheck.position, 0.1f);
        Gizmos.DrawWireSphere(fallCheck.position, 0.1f);
    }
}