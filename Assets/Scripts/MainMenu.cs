using UnityEngine;
using UnityEngine.SceneManagement;

// TODO add it in a directory "Menu"?
public class MainMenu: MonoBehaviour {

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
        Application.OpenURL("https://www.youtube.com/watch?v=4TI1onWI_IM");
    }

    /// <summary>
    /// Show Credits Menu
    /// </summary>
    public void CreditsButton() {
        Application.OpenURL("https://github.com/melokye/IMAC2_Rubik4D");
    }

    /// <summary>
    /// Quit Game
    /// </summary>
    public void QuitButton() {
        Application.Quit();
    }
}
