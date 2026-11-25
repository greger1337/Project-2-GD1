
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
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Only initialize gameplay in MainGame
        if (SceneManager.GetActiveScene().name != "MainGame")
            return;

        score = 0;
        childrenSaved = 0;
        timeRemaining = timeLimit;
        gameEnded = false;

        UpdateUI();
    }

    void Update()
    {
        // Timer only works during gameplay
        if (SceneManager.GetActiveScene().name != "MainGame")
            return;

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

        if (SceneManager.GetActiveScene().name != "MainGame")
            return;

        childrenSaved++;

        score += pointsPerChild;

        Debug.Log("Children saved: " + childrenSaved);
        Debug.Log("Current score: " + score);

        UpdateUI();

        if (childrenSaved >= childrenNeeded)
        {
            WinGame();
            return;
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

        // Hide timer
        if (timerText != null)
            timerText.gameObject.SetActive(false);

        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.SetInt("ChildrenSaved", childrenSaved);
        PlayerPrefs.SetString("GameResult", "WIN");

        PlayerPrefs.Save();

        Debug.Log("PLAYER WON!");

        SceneManager.LoadScene("WinScreen");
    }

    // Losing condition
    public void LoseGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        // Hide timer
        if (timerText != null)
            timerText.gameObject.SetActive(false);

        score = -999;

        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.SetInt("ChildrenSaved", childrenSaved);
        PlayerPrefs.SetString("GameResult", "LOSE");

        PlayerPrefs.Save();

        Debug.Log("PLAYER LOST!");

        SceneManager.LoadScene("LooseScreen");
    }

    // Restart the game
    public void RestartGame()
    {
        PlayerPrefs.DeleteKey("FinalScore");
        PlayerPrefs.DeleteKey("ChildrenSaved");
        PlayerPrefs.DeleteKey("GameResult");

        SceneManager.LoadScene("MainGame");
    }

    // Start a new game
    public void StartGame()
    {
        PlayerPrefs.DeleteKey("FinalScore");
        PlayerPrefs.DeleteKey("ChildrenSaved");
        PlayerPrefs.DeleteKey("GameResult");

        SceneManager.LoadScene("MainGame");
    }
}
