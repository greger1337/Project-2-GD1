
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void WinGame()
    {
        SceneManager.LoadScene("WinScreen");
    }

    public void LoseGame()
    {
        SceneManager.LoadScene("LoseScreen");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainGame");
    }
}
