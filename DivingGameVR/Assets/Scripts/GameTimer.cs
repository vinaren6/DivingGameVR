using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TextMeshPro))]
public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshPro display;
    [SerializeField] private int roundTime = 135;
    [SerializeField] private float timeLeft;

    private void Awake()
    {
        timeLeft = roundTime;
    }

    private void Start()
    {
        display = GetComponent<TextMeshPro>();
        timeLeft = roundTime;
    }

    private void Update()
    {
        if (timeLeft <= 0)
            EndRound();
        else
            timeLeft -= Time.deltaTime;

        display.text = ((int)timeLeft).ToString();
    }

    private void EndRound()
    {
        int currentScore = ScoreManager.Instance.Score;
        int highscore = int.Parse(PlayerPrefs.GetString("Highscore", "0"));
        
        if (ScoreManager.Instance.Score > highscore)
        {
            PlayerPrefs.SetString("Highscore", currentScore.ToString());
        }
        else
        {
            PlayerPrefs.SetString("LastScore", currentScore.ToString());
        }
        
        SceneManager.LoadScene("MainMenu");
    }
}
