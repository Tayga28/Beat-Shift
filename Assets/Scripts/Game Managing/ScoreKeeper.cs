using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public int score;
    public Text scoreText;

    private void Start()
    {
        score = 0;  // Initialize score
    }

    public void UpdateScore(int fragmentID)
    {
        // Assuming fragmentID can be used directly for scoring
        // Modify the scoring logic as needed
        score += 1;  // Increment score (can customize based on fragmentID)
        scoreText.text = score.ToString();
    }
}
