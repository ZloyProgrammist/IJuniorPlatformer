using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private PatrolMode _patrolMode = PatrolMode.Horizontal;
    [SerializeField] private FacingDirection _startFacing = FacingDirection.Right;
    [SerializeField] private float _patrolSpeed = 2f;
    [SerializeField] private float _reachDistance = 0.1f;
    [SerializeField] private float _idleDuration = 1f;

    [Header("Animator Parameters")]
    [SerializeField] private string _speedParameter = "Speed";
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Rigidbody2D _rigidbody;
    private Coroutine _idleCoroutine;

    private int _currentWaypointIndex;
    private bool _isWaiting;
    private float _moveDirectionX;

    private enum PatrolMode
    {
        Horizontal,
        Flying
    }

    private enum FacingDirection
    {
        Right,
        Left
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        FlipSprite();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        MoveToWaypoint();
    }

    private void MoveToWaypoint()
    {
        if (_isWaiting || _waypoints.Length == 0)
        {
            StopMovement();
            return;
        }

        Transform target = _waypoints[_currentWaypointIndex];
        Vector2 toTarget = (Vector2)target.position - _rigidbody.position;

        if (HasReachedWaypoint(toTarget))
        {
            ReachWaypoint();
            return;
        }

        if (_patrolMode == PatrolMode.Horizontal)
            MoveHorizontally(toTarget);
        else
            MoveToTarget(toTarget);
    }

    private bool HasReachedWaypoint(Vector2 toTarget)
    {
        if (_patrolMode == PatrolMode.Horizontal)
            return Mathf.Abs(toTarget.x) <= _reachDistance;

        return toTarget.magnitude <= _reachDistance;
    }

    private void MoveHorizontally(Vector2 toTarget)
    {
        _moveDirectionX = Mathf.Sign(toTarget.x);
        _rigidbody.velocity = new Vector2(_moveDirectionX * _patrolSpeed, _rigidbody.velocity.y);
    }

    private void MoveToTarget(Vector2 toTarget)
    {
        Vector2 direction = toTarget.normalized;
        _moveDirectionX = direction.x;
        _rigidbody.velocity = direction * _patrolSpeed;
    }

    private void StopMovement()
    {
        if (_patrolMode == PatrolMode.Horizontal)
            _rigidbody.velocity = new Vector2(0f, _rigidbody.velocity.y);
        else
            _rigidbody.velocity = Vector2.zero;
    }

    private void ReachWaypoint()
    {
        _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
        StopMovement();

        if (_idleCoroutine != null)
            StopCoroutine(_idleCoroutine);

        _idleCoroutine = StartCoroutine(WaitAtWaypoint());
    }

    private IEnumerator WaitAtWaypoint()
    {
        _isWaiting = true;
        yield return new WaitForSeconds(_idleDuration);
        _isWaiting = false;
        _idleCoroutine = null;
    }

    private void FlipSprite()
    {
        if (_moveDirectionX > 0f && _startFacing == FacingDirection.Left) 
            _spriteRenderer.flipX = true;
        else if (_moveDirectionX > 0f && _startFacing == FacingDirection.Right)
            _spriteRenderer.flipX = false;
        else if (_moveDirectionX < 0f && _startFacing == FacingDirection.Left) 
            _spriteRenderer.flipX = false;
        else if (_moveDirectionX < 0f && _startFacing == FacingDirection.Right)
            _spriteRenderer.flipX = true;
    }

    private void UpdateAnimator()
    {
        if (_animator == null) 
            return;

        bool isMoving = _rigidbody.velocity.sqrMagnitude > 0.01f;

        _animator.SetFloat(_speedParameter, isMoving ? 1f : 0f);
    }
}