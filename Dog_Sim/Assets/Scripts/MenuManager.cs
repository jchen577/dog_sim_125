using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager: MonoBehaviour {
    public void StartGame() {
        SceneManager.LoadScene("PlayScene");
    }

    public void SelectOptiions() {
        SceneManager.LoadScene("OptionScene");
    }

    public void SelectCredits() {
        SceneManager.LoadScene("CreditScene");
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene("MenuScene");
    }
}
