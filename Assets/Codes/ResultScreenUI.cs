
using UnityEngine;
using TMPro;

public class ResultScreenUI : MonoBehaviour
{
    public TMP_Text finalScoreText;
    public TMP_Text resultText;
    public TMP_Text childrenSavedText;

    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);

        int childrenSaved = PlayerPrefs.GetInt("ChildrenSaved", 0);

        string result = PlayerPrefs.GetString("GameResult", "");

        finalScoreText.text = "Final Score: " + finalScore;

        childrenSavedText.text =
            "Children Saved: " + childrenSaved + "/3";

        if (result == "WIN")
        {
            resultText.text = "You Won";
        }
        else
        {
            resultText.text = "GAME OVER!";
        }
    }
}
