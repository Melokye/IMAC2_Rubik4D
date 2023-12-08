using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_MainMenu : MonoBehaviour
{
    public void PlayNowButton()
    {
        // Play Now Button has been pressed, here you can initialize your game
        SceneManager.LoadScene("MainScene");
    }

    public void CreditsButton()
    {
        // Show Credits Menu
        SceneManager.LoadScene("Credit");
    }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }
}
