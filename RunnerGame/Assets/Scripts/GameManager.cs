using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private AudioSource[] _audioSources;

    public int Score
    {
        get => _score;
        set
        {
            if (value > _score)
                PlayerPrefs.SetInt("score", _score = value);
        }
    }
    private int _score;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        if (PlayerPrefs.HasKey("score"))
            _score = PlayerPrefs.GetInt("score");
    }

    public void ChangeVolume(float value)
    {
        foreach (var audioSource in _audioSources)
            audioSource.volume = value;
    }

    public float GetVolume() => _audioSources[0].volume;

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        _audioSources[(sceneIndex+1)%2].Stop();
        _audioSources[sceneIndex].Play();
    }
}
