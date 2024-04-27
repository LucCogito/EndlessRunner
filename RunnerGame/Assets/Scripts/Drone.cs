using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [field: SerializeField]
    public Rigidbody2D Rigidbody { get; private set; }
    [SerializeField]
    private Vector2 _shotDelayRange;
    [SerializeField]
    private Transform _projectilePrefab;

    private float _currentShotDelay;
    private Player _player;

    public void Setup(Player player)
    {
        _player = player;
        _currentShotDelay = Random.Range(_shotDelayRange.x, _shotDelayRange.y);
    }

    private void Update()
    {
        if ((_currentShotDelay -= Time.deltaTime) <= 0)
        {
            _currentShotDelay = Random.Range(_shotDelayRange.x, _shotDelayRange.y);
            Instantiate(_projectilePrefab, transform.position, Quaternion.identity).up = _player.transform.position - transform.position;
        }
    }

    private void OnBecameInvisible()
    {
        if (transform.position.x < 0f)
            Destroy(gameObject);
    }
}
