using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToPlayScene()
    {
        SceneManager.LoadScene("PlayScene");
    }
    public void SwitchToGameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }
    public void SwitchToCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }
    public void SwitchToMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void SwitchToLockerScene()
    {
        SceneManager.LoadScene("LockerScene");
    }
}
