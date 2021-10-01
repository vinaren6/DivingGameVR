using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;

    public static ScoreManager Instance { get { return _instance; } }

    private int score = 0;

    public int Score => score;

    TextMeshPro[] texts;
    public TextMeshPro[] extraTexts;
    private void Awake()
    {
        texts = GetComponentsInChildren<TextMeshPro>();
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        score = 0;
    }

    public void AddScore()
    {
        score += 1;
        PlayerPrefs.SetString("LastScore", score.ToString());
    }

    public void UpdateText()
    {
        foreach(TextMeshPro text in texts)
        {
            text.text = "Catches: " + score.ToString();
        }
        foreach(TextMeshPro t in extraTexts)
        {
            t.text = score.ToString();
        }
    }
}
