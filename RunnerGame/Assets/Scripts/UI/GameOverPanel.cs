using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _infoText;

    public void Show(int score)
    {
        _infoText.text = $"You've reached\n{score}m";
        gameObject.SetActive(true);
    }

    public void StartMenuScene() => GameManager.instance.LoadScene(0);
}
