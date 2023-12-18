using UnityEngine;
using UnityEngine.SceneManagement;

// TODO rename file + add it in a directory "Menu"?
public class SC_MainMenu: MonoBehaviour {

    /// <summary>
    /// Play Now Button has been pressed, here you can initialize your game
    /// </summary>
    public void PlayNowButton() {
        SceneManager.LoadScene("MainScene");
    }

    /// <summary>
    /// Explain the projet's Menu
    /// </summary>
    public void Explain4DButton() {
        SceneManager.LoadScene("what's4D");
    }

    /// <summary>
    /// Show Credits Menu
    /// </summary>
    public void CreditsButton() {
        SceneManager.LoadScene("Credit");
    }

    /// <summary>
    /// Quit Game
    /// </summary>
    public void QuitButton() {
        Application.Quit();
    }
}
