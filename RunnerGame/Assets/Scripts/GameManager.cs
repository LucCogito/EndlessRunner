using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
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

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
