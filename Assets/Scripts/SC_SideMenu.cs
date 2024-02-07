using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_SideMenu: MonoBehaviour {

    /// <summary>
    /// Go back to the main menu
    /// </summary>
    public void BackMenuButton() {
        SceneManager.LoadScene("Menu");
    }
}
