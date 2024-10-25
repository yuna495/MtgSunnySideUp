using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    public GameObject sunnySideUpButton;
    public GameObject existButton;
    public void SunnySideUpButton() {
        // SunnySideUpシーンに切り替える
        SceneManager.LoadScene("SunnySideUp");

    }
    public void ExistButton() {
        // ゲームを終了する
        Application.Quit();
    }
}
