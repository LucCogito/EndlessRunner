using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BestRunText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;

    void Start()
    {
        _text.text = $"Best run\n{GameManager.instance.Score}m";
    }
}
