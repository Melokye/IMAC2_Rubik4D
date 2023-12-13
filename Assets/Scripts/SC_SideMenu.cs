using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_SideMenu: MonoBehaviour {
    public void BackMenuButton() {
        SceneManager.LoadScene("Menu");
    }

    // TODO ?
    // public void CommandBoardButton() {
    //     SceneManager.LoadScene("CommandBoard");
    // }
}
