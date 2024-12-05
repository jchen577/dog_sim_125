using System.Collections;
using UnityEngine;
using TMPro;
public class ScoreManage : MonoBehaviour
{
    public int score; // Player's score
    public TextMeshProUGUI scoreText; // Reference to the Score UI text
    public TextMeshProUGUI starText; // Reference to the Star UI text
    // Public property to get the current score
    public int Score => score;
    // Public property to get the current number of stars
    public int StarCount
    {
        get
        {
        // Calculate the number of stars based on the score (score / 100 gives 1 star per 100 score)
        return Mathf.Clamp(score / 100, 0, 5); // Max 5 stars
        }
    }
    void Start()
    {
        score = 0; // Start the score at 0
        UpdateScoreText();
        UpdateStarVisuals();
    }
    // Method to update the score
    public void updateScore(int addScore) // Notice the uppercase 'U'
    {
        score += addScore;
        UpdateScoreText();
        UpdateStarVisuals();
    }
    // Update the score text in the UI
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
    // Update the star visuals based on the current star count
    private void UpdateStarVisuals()
    {
        int starCount = StarCount; // Get the player's current star count
        starText.text = new string('*', starCount) + new string('_', 5 - starCount); // Filled and empty stars
    }
}