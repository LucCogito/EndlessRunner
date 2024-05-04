using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    private GameplayManager _gameplayManager;
    [SerializeField]
    private float _startingSpeed1, _startingSpeed2, _speed1Increase, _speed2Increase;
    [SerializeField]
    private Transform _background1, _background2;

    private float _currentSpeed1, _currentSpeed2, _screenWidth;
    private bool _gameOver;

    private void Awake()
    {
        _gameplayManager.OnSpeedIncreased += IncreaseSpeed;
        _gameplayManager.OnStopGame += Stop;
        _currentSpeed1 = _startingSpeed1;
        _currentSpeed2 = _startingSpeed2;
    }

    private void Start()
    {
        _screenWidth = CameraManager.instance.HalfWidth;
    }

    private void Update()
    {
        if (_gameOver)
            return;
        _background1.position = Vector3.right * (Mathf.Repeat(_background1.position.x - Time.deltaTime * _currentSpeed1 + _screenWidth, 2 * _screenWidth) - _screenWidth);
        _background2.position = Vector3.right * (Mathf.Repeat(_background2.position.x - Time.deltaTime * _currentSpeed2 + _screenWidth, 2 * _screenWidth) - _screenWidth);
    }

    private void IncreaseSpeed()
    {
        _currentSpeed1 += _speed1Increase;
        _currentSpeed2 += _speed2Increase;
    }

    private void Stop() => _gameOver = true;
}
