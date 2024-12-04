using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager: MonoBehaviour {
    public void Start() {
        ShowCursor();
    }
    public void StartGame() {
        SceneManager.LoadScene("PlayScene");
    }

    public void SelectOptions() {
        SceneManager.LoadScene("OptionScene");
    }

    public void SelectCredits() {
        SceneManager.LoadScene("CreditScene");
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene("MenuScene");
    }

    public void Restart() {
        StartGame();
    }

    // Helpers
    private void ShowCursor() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
