using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [Header("Buttons")]
    public Button playAgainButton;
    public Button quitButton;

    void Start()
    {
        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(PlayAgain);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    void PlayAgain()
    {
        SceneManager.LoadScene("Level1");
    }

    void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}