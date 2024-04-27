using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private float _maxSpeed;

    private void Update()
    {
        if (_rigidbody.velocity.x > _maxSpeed)
            _rigidbody.velocity = new Vector2(_maxSpeed, _rigidbody.velocity.y); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.position.x < 0f || transform.position.y < -CameraManager.instance.HalfHeight)
            Destroy(gameObject);
    }
}
