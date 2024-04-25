using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private float _maxSpeed, _acceleration, _jumpPower, _glidingGravity, _fallingGravity;
    [SerializeField]
    private Vector2 _groundedCastPoint, _groundCheckSize;
    [SerializeField]
    private LayerMask _groundLayer;

    private Vector2 _inputDirection, _previousInputDirection, _groundOverlapPoint;
    private float _accelerationTick, _horizontalVelocity;
    private bool _isDeaccelerating;

    private void Update()
    {
        _previousInputDirection = _inputDirection;
        _inputDirection.x = Input.GetAxis("Horizontal");
        _inputDirection.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnBecameInvisible()
    {
        SceneManager.LoadScene(0);
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
        _rigidbody.velocity = new Vector2(_horizontalVelocity, CalculateJump());
    }

    private float CalculateJump()
    {
        _groundOverlapPoint = _groundedCastPoint + _rigidbody.position;
        if (_inputDirection.y > 0f)
        {
            _rigidbody.gravityScale = _glidingGravity;
            if (Physics2D.OverlapArea(_groundOverlapPoint - _groundCheckSize * .5f, _groundOverlapPoint + _groundCheckSize * .5f, _groundLayer) != null)
                return _jumpPower;
        }
        else
            _rigidbody.gravityScale = _fallingGravity;
        return _rigidbody.velocity.y;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(_groundedCastPoint + _rigidbody.position, new Vector3(_groundCheckSize.x, _groundCheckSize.y, .1f));
    }
#endif
}
