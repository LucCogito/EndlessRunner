using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private float _startingSpeed, _speedIncreasePerRoof, _maxSpeed;
    [SerializeField]
    private Vector2 _gapSizeRange, _heightDifferenceRange, _carSpawnTimeRange, _droneSpawnTimeRange;
    [SerializeField]
    private Rooftop[] _rooftopPrefabs;
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private GameOverPanel _gameOverPanel;
    [SerializeField]
    private Rooftop _startingRooftop;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private Transform _cratePrefab;
    [SerializeField]
    private Car _carPrefab;
    [SerializeField]
    private Drone _dronePrefab;

    List<Rigidbody2D> _movedBodies;

    private float _targetGapSize, _currentGapSize, _currentSpeed, _lastRooftopHeight, _score, _currentCarSpawnTime = 15f, _currentDroneSpawnTime = 30f;
    private Rooftop _nextRooftop;
    private Rigidbody2D _currentIteratedRigidbody;
    private int _roofsIterationIndex;
    private bool _gameOver;

    public event Action OnSpeedIncreased, OnStopGame;

    private void Awake()
    {
        _player.OnDied += StopGame;
        _movedBodies = new List<Rigidbody2D>() {_startingRooftop.Rigidbody};
        _nextRooftop = _rooftopPrefabs[Random.Range(0, _rooftopPrefabs.Length)];
        _currentSpeed = _startingSpeed;
    }

    private void Update()
    {
        if (_gameOver)
            return;
        _scoreText.text = $"Distance\n" + Mathf.RoundToInt(_score += Time.deltaTime * _currentSpeed) + "m";
        if ((_currentCarSpawnTime -= Time.deltaTime) <= 0f)
        {
            _currentCarSpawnTime = Random.Range(_carSpawnTimeRange.x, _carSpawnTimeRange.y);
            var carSpawnPosition = new Vector2(CameraManager.instance.HalfWidth + _carPrefab.Width * .5f, Random.Range(-4f, 4f));
            if (Random.value > .5f)
                carSpawnPosition.x *= -1f;
            Instantiate(_carPrefab, carSpawnPosition, Quaternion.identity).Setup(carSpawnPosition.x < 0f);
        }
        if ((_currentDroneSpawnTime -= Time.deltaTime) <= 0f)
        {
            _currentDroneSpawnTime = Random.Range(_droneSpawnTimeRange.x, _droneSpawnTimeRange.y);
            var droneSpawnPosition = new Vector2(CameraManager.instance.HalfWidth + _carPrefab.Width * .5f, Random.Range(-4f, 4f));
            var drone = Instantiate(_dronePrefab, droneSpawnPosition, Quaternion.identity);
            _movedBodies.Add(drone.Rigidbody);
            drone.Setup(_player);
        }
    }

    private void FixedUpdate()
    {
        if (_gameOver)
            return;
        if ((_currentGapSize += Time.fixedDeltaTime * _currentSpeed) >= _targetGapSize)
        {
            _targetGapSize = Random.Range(_gapSizeRange.x, _gapSizeRange.y) + _nextRooftop.Width * .5f;
            _currentGapSize = 0f;
            if (_currentSpeed < _maxSpeed)
            {
                _currentSpeed = Mathf.Min(_currentSpeed + _speedIncreasePerRoof, _maxSpeed);
                OnSpeedIncreased?.Invoke();
            }
            _lastRooftopHeight = Random.Range(Mathf.Clamp(_lastRooftopHeight + _heightDifferenceRange.x, -3f, 3f), 
                Mathf.Clamp(_lastRooftopHeight + _heightDifferenceRange.y, -4f, 2f));
            var rooftop = Instantiate(_nextRooftop, new Vector2(CameraManager.instance.HalfWidth + _nextRooftop.Width * .5f, _lastRooftopHeight), Quaternion.identity);
            _movedBodies.Add(rooftop.Rigidbody);
            if (Random.value > .5f)
            {
                var crateSpawnPosition = rooftop.transform.position + new Vector3(Random.Range(-rooftop.Width, rooftop.Width) * .3f, .01f, 0f);
                Instantiate(_cratePrefab, crateSpawnPosition, Quaternion.identity);
            }
            _nextRooftop = _rooftopPrefabs[Random.Range(0, _rooftopPrefabs.Length)];
            _targetGapSize += _nextRooftop.Width * .5f;
        }
        while(_roofsIterationIndex < _movedBodies.Count)
        {
            _currentIteratedRigidbody = _movedBodies[_roofsIterationIndex];
            if (_currentIteratedRigidbody == null)
                _movedBodies.RemoveAt(_roofsIterationIndex);
            else
            {
                _currentIteratedRigidbody.MovePosition(_currentIteratedRigidbody.position + Time.fixedDeltaTime * _currentSpeed * Vector2.left);
                _roofsIterationIndex++;
            }
        }
        _roofsIterationIndex = 0;
    }

    private void StopGame()
    {
        _gameOver = true;
        _score = Mathf.Round(_score);
        _gameOverPanel.Show((int)_score);
        GameManager.instance.Score = (int)_score;
        OnStopGame?.Invoke();
    }
}
