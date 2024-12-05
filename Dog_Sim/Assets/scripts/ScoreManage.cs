using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManage: MonoBehaviour {
    private int score = 0;
    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start() {
        score = 0;
        scoreText.text = "Score: " + score;
    }

    public void updateScore(int addScore) {
        score += addScore;
        scoreText.text = "Score: " + score;
    }

}
