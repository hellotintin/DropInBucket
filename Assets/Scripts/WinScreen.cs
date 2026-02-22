using UnityEngine;
using UnityEngine.SceneManagement;


// Attach to any GameObject in WinScene.
public class WinScreen : MonoBehaviour
{
    // Called by 'Play Again' button
    public void PlayAgain()
    {
        SceneManager.LoadScene("Level1");
    }


    // Called by 'Quit' button
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

