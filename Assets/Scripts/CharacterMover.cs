using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CharacterMover : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 12f;

    [Header("Controls")]
    [SerializeField] private KeyCode _moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode _moveRightKey = KeyCode.D;
    [SerializeField] private KeyCode _jumpKey = KeyCode.W;

    [Header("Ground Checker")]
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float _groundCheckerRadius = 0.15f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Animator Parameters")]
    [SerializeField] private string _speedParam = "Speed";
    [SerializeField] private string _isGroundedParam = "IsGrounded";
    [SerializeField] private string _verticalSpeedParameters = "VerticalSpeed";

    private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private float _horizontalInput;
    private bool _isGrounded;
    private bool _jumpPressed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ReadInput();
        TryJump();
        FlipSprite();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        CheckGround();
        MoveHorizontally();
    }

    private void ReadInput()
    {
        _horizontalInput = 0f;

        if (Input.GetKey(_moveLeftKey)) 
            _horizontalInput -= 1f;
        if (Input.GetKey(_moveRightKey)) 
            _horizontalInput += 1f;

        _jumpPressed = Input.GetKeyDown(_jumpKey);
    }

    private void TryJump()
    {
        if (_jumpPressed && _isGrounded)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
        }
    }

    private void FlipSprite()
    {
        if (_horizontalInput > 0f) 
            _spriteRenderer.flipX = true;
        else if (_horizontalInput < 0f) 
            _spriteRenderer.flipX = false;
    }

    private void CheckGround()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _groundLayer);
    }

    private void MoveHorizontally()
    {
        _rigidbody.velocity = new Vector2(_horizontalInput * _moveSpeed, _rigidbody.velocity.y);
    }

    private void UpdateAnimator()
    {
        if (_animator == null) 
            return;

        float speed = Mathf.Abs(_rigidbody.velocity.x) / _moveSpeed;

        _animator.SetFloat(_speedParam, speed);
        _animator.SetBool(_isGroundedParam, _isGrounded);
        _animator.SetFloat(_verticalSpeedParameters, _rigidbody.velocity.y);
    }
}