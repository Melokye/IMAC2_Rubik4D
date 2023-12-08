using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_MainMenu : MonoBehaviour
{
    public GameObject MainMenu;
    // public GameObject CreditsMenu;

    // Start is called before the first frame update
    // void Start()
    // {
    //     MainMenuButton();
    // }

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

    // public void MainMenuButton()
    // {
    //     // Show Main Menu
    //     MainMenu.SetActive(true);
    //     // CreditsMenu.SetActive(false);
    // }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }
}
