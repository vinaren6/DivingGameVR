using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoaderManager : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
