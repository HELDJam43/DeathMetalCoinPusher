using UnityEngine;
using TMPro;
public class UITopScore : MonoBehaviour
{
    public TMP_Text TitleText;
    public TMP_Text TopScoreText;
    
    private int HighScore = 0;

    private const string DMCPTopScore = "DMCPTopScore";

    private void Start()
    {
        HighScore = PlayerPrefs.GetInt(DMCPTopScore);
        UpdateTopScore(HighScore);
        GameManager.Instance.OnPointsAdded.AddListener(OnScoreUpdated);
        GameManager.Instance.OnGameOverEvent.AddListener(OnGameOver);
    }

    private void UpdateTopScore(int score)
    {
        TopScoreText.text = score.ToString();
    }

    private void OnGameOver(int score)
    {
        if (score > HighScore)
        {
            PlayerPrefs.SetInt(DMCPTopScore, score);
            UpdateTopScore(score);
        }
    }

    public void OnScoreUpdated(int addedScore, int totalScore)
    {
        if(totalScore > HighScore)
        {
            UpdateTopScore(totalScore);
        }
    }
}
