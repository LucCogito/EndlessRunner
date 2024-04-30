using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : Button
{
    public void StartGameplayScene() => GameManager.instance.LoadScene(1);
}
