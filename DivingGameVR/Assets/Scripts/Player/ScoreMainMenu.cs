using System;
using UnityEngine;

public class ScoreMainMenu : MonoBehaviour
{
    
    void Start()
    {
        string highscore = PlayerPrefs.GetString("Highscore", "0");
        string lastScore = PlayerPrefs.GetString("LastScore", "0");

        GetComponent<UnityEngine.UI.Text>().text = String.Format("Highscore: {0}\nLast score: {1}", highscore, lastScore);
    }
}
