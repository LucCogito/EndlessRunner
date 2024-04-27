using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Vector2 _speedRange;
    [field:SerializeField]
    public float Width { get; private set; }

    private float _speed, _directionCoefficient;

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + Vector2.right * (_speed * _directionCoefficient * Time.fixedDeltaTime));
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void Setup(bool startsFromTheLeft)
    {
        _speed = Random.Range(_speedRange.x, _speedRange.y);
        _directionCoefficient = startsFromTheLeft ? 1 : -1;
        _spriteRenderer.flipX = !startsFromTheLeft;
    }
}
