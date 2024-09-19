using UnityEngine;

public class FollowEnemyController : MonoBehaviour
{
    public float moveSpeed;
    [Space(5)]
    public float sightRange;
    public bool canChase;
    private Transform _target;
   
    private Vector2 _moveDirection;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.Play("Bat_Idle");
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        if (_target == null) return;

        _moveDirection = _target.position - transform.position;
    
        if (Vector2.Distance(_target.position, transform.position) < sightRange)
        {
            canChase = true;
            _animator.Play("Bat_Fly");
        }

        transform.localScale = transform.position.x < _target.position.x ? 
            new Vector2(1, 1) : new Vector2(-1, 1);
    }
    
    private void FixedUpdate()
    {
        if (canChase)
        {
            _rigidbody2D.linearVelocity = _moveDirection * moveSpeed;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;                                
        Gizmos.DrawWireSphere(transform.position, sightRange);    
        Gizmos.color = Color.yellow;  
    }
}