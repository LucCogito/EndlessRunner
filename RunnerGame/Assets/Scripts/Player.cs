using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _maxSpeed, _acceleration, _jumpPower, _glidingGravity, _fallingGravity, _climbingAcceleration, _climbingMaxSpeed, _tasedTime;
    [SerializeField]
    private Vector2 _wallLeftCastPoint, _wallRightCastPoint, _wallCheckSize, _groundedCastPoint, _groundCheckSize;
    [SerializeField]
    private LayerMask _climbLayer, _jumpLayer;

    private float _jumpDisableTimeAfterClimbing = .3f;

    private Vector2 _inputDirection, _previousInputDirection, _groundOverlapPoint, _wallLeftOverlapPoint, _wallRightOverlapPoint;
    private float _accelerationTick, _horizontalVelocity, _currentJumpDisableTimeAfterClimbing, _currentTasedTime;
    private bool _isDeaccelerating, _isClimbing, _isGrounded, _isTased;

    public event Action OnDied;

    private void Update()
    {
        if (_isTased)
        {
            if ((_currentTasedTime -= Time.deltaTime) > 0f)
                return;
            else
                _animator.SetBool("Tased", _isTased = false);
        }
        _previousInputDirection = _inputDirection;
        _inputDirection.x = Input.GetAxis("Horizontal");
        if (_currentJumpDisableTimeAfterClimbing > 0)
        {
            _currentJumpDisableTimeAfterClimbing -= Time.deltaTime;
            _inputDirection.y = 0f;
        }
        else
            _inputDirection.y = Input.GetAxis("Vertical");
        _animator.SetFloat("InputY", _inputDirection.y);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnDied?.Invoke();
        Destroy(gameObject);
    }

    public void GetTased()
    {
        _inputDirection = _previousInputDirection = Vector2.zero;
        _currentTasedTime = _tasedTime;
        _animator.SetFloat("InputY", _inputDirection.y);
        _animator.SetBool("Tased", _isTased = true);
    }

    private void Move()
    {
        _accelerationTick = _acceleration * Time.fixedDeltaTime;
        _horizontalVelocity = _rigidbody.velocity.x + _inputDirection.x * _accelerationTick;
        _isDeaccelerating = false;
        if (Mathf.Sign(_previousInputDirection.x) == Mathf.Sign(_inputDirection.x))
        {
            if (Mathf.Abs(_previousInputDirection.x) > Mathf.Abs(_inputDirection.x) || _inputDirection.x == 0f)
                _isDeaccelerating = true;
        }
        else
            _isDeaccelerating = true;
        if (_isDeaccelerating)
        {
            if (Mathf.Abs(_horizontalVelocity) > _accelerationTick)
                _horizontalVelocity -= (_horizontalVelocity > 0f ? 1f : -1f) * _accelerationTick;
            else
                _horizontalVelocity = 0f;
        }
        else if (Mathf.Abs(_horizontalVelocity) > _maxSpeed)
            _horizontalVelocity = _maxSpeed * (_horizontalVelocity > 0f ? 1f : -1f);
        _rigidbody.velocity = CalculateJump(new Vector2(_horizontalVelocity, _rigidbody.velocity.y));
    }

    private Vector2 CalculateJump(Vector2 newVelocity)
    {
        _groundOverlapPoint = _groundedCastPoint + _rigidbody.position;
        _isGrounded = Physics2D.OverlapArea(_groundOverlapPoint - _groundCheckSize * .5f, _groundOverlapPoint + _groundCheckSize * .5f, _jumpLayer) != null;
        _animator.SetBool("Grounded", _isGrounded);
        if (_inputDirection.y > 0f)
        {
            _rigidbody.gravityScale = _glidingGravity;
            if (_previousInputDirection.y <= _inputDirection.y && _isGrounded)
                return new Vector2(newVelocity.x, _jumpPower);
            else
            {
                _wallLeftOverlapPoint = _wallLeftCastPoint + _rigidbody.position;
                if (Physics2D.OverlapArea(_wallLeftOverlapPoint - _wallCheckSize * .5f, _wallLeftOverlapPoint + _wallCheckSize * .5f, _climbLayer) != null)
                {
                    _isClimbing = true;
                    return new Vector2(newVelocity.x, Mathf.Min(_climbingMaxSpeed, newVelocity.y + _climbingAcceleration * Time.fixedDeltaTime));
                }
                else
                {
                    _wallRightOverlapPoint = _wallRightCastPoint + _rigidbody.position;
                    if (Physics2D.OverlapArea(_wallRightOverlapPoint - _wallCheckSize * .5f, _wallRightOverlapPoint + _wallCheckSize * .5f, _climbLayer) != null)
                    {
                        _isClimbing = true;
                        return new Vector2(newVelocity.x, Mathf.Min(_climbingMaxSpeed, newVelocity.y + _climbingAcceleration * Time.fixedDeltaTime));
                    }
                    if (_isClimbing)
                    {
                        _isClimbing = false;
                        _currentJumpDisableTimeAfterClimbing = _jumpDisableTimeAfterClimbing;
                    }
                }
            }
        }
        else
            _rigidbody.gravityScale = _fallingGravity;
        return newVelocity;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(_groundedCastPoint + _rigidbody.position, new Vector3(_groundCheckSize.x, _groundCheckSize.y, .1f));
        Gizmos.color = Color.green;
        Gizmos.DrawCube(_wallLeftCastPoint + _rigidbody.position, new Vector3(_wallCheckSize.x, _wallCheckSize.y, .1f));
        Gizmos.DrawCube(_wallRightCastPoint + _rigidbody.position, new Vector3(_wallCheckSize.x, _wallCheckSize.y, .1f));
    }
#endif
}
