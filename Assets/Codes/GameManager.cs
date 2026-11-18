
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int childrenNeeded = 3;
    public int pointsPerChild = 100;
    public float timeLimit = 10f;

    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text childrenText;

    private int score = 0;
    private int childrenSaved = 0;
    private float timeRemaining;

    private bool gameEnded = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        score = 0;
        childrenSaved = 0;
        timeRemaining = timeLimit;

        UpdateUI();
    }

    void Update()
    {
        if (gameEnded)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            UpdateUI();
            LoseGame();
            return;
        }

        UpdateUI();
    }

    // Called when the player saves a child
    public void SaveChild()
    {
        if (gameEnded)
            return;

        childrenSaved++;

        score += pointsPerChild;

        Debug.Log("Children saved: " + childrenSaved);
        Debug.Log("Current score: " + score);

        UpdateUI();

        if (childrenSaved >= childrenNeeded)
        {
            WinGame();
        }
    }

    // Update the visible UI
    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (timerText != null)
            timerText.text = "Time: " + timeRemaining.ToString("F1");

        if (childrenText != null)
            childrenText.text = "Children: " +
                childrenSaved + "/" + childrenNeeded;
    }

    // Winning condition
    public void WinGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.SetInt("ChildrenSaved", childrenSaved);
        PlayerPrefs.SetString("GameResult", "WIN");

        PlayerPrefs.Save();

        SceneManager.LoadScene("WinScreen");
    }

    public void LoseGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        score = -999;

        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.SetInt("ChildrenSaved", childrenSaved);
        PlayerPrefs.SetString("GameResult", "LOSE");

        PlayerPrefs.Save();

        SceneManager.LoadScene("LooseScreen");
    }

    // Restart the game
    public void RestartGame()
    {
        PlayerPrefs.DeleteKey("FinalScore");
        PlayerPrefs.DeleteKey("GameResult");

        SceneManager.LoadScene("MainGame");
    }

    public void StartGame()
    {
        // Reset saved game data
        PlayerPrefs.DeleteKey("FinalScore");
        PlayerPrefs.DeleteKey("ChildrenSaved");
        PlayerPrefs.DeleteKey("GameResult");

        // Load gameplay scene
        SceneManager.LoadScene("MainGame");
    }

}
