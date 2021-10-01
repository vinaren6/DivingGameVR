using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string Highscore = PlayerPrefs.GetString("Highscore", "0");

        GetComponent<UnityEngine.UI.Text>().text = "Highscore: " + Highscore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
