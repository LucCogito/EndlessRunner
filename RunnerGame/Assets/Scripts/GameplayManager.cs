using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private float _startingSpeed, _speedIncreasePerRoof;
    [SerializeField]
    private Vector2 _gapSizeRange, _heightDifferenceRange;
    [SerializeField]
    private GameObject _rooftopPrefab, _startingRooftop;
    [SerializeField]
    private Transform _playerTransform;

    List<GameObject> _rooftops;

    private float _targetGapSize, _currentGapSize, _currentSpeed;

    private void Awake()
    {
        _rooftops = new List<GameObject>() {_startingRooftop};
        _currentSpeed = _startingSpeed;
    }

    private void FixedUpdate()
    {
        if ((_currentGapSize += Time.fixedDeltaTime) >= _targetGapSize)
        {
            _targetGapSize = Random.Range(_gapSizeRange.x, _gapSizeRange.y);
            _currentGapSize = 0f;
            _currentSpeed += _speedIncreasePerRoof;
            var roofHeight = Random.Range(Mathf.Max(_playerTransform.position.y + _heightDifferenceRange.x, -4f), 
                Mathf.Min(_playerTransform.position.y + _heightDifferenceRange.y, 2f));
            _rooftops.Add(Instantiate(_rooftopPrefab, new Vector2(9f + 4f / 2f, roofHeight), Quaternion.identity)); // Get size from Rooftop prefab in the future
        }
        foreach (var rooftop in _rooftops)
            rooftop.transform.Translate(Time.fixedDeltaTime * _currentSpeed * Vector2.left);
    }
}
