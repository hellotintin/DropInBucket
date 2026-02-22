using UnityEngine;
using UnityEngine.SceneManagement;



public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;


    static readonly string[] levels =
    {
        "Level1",
        "Level2",
        "Level3",
        "Level4",
        "Level5",
        "WinScene"
    };


    static int currentIndex = 0;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void LoadNextLevel()
    {
        currentIndex++;
        if (currentIndex >= levels.Length)
            currentIndex = levels.Length - 1;
        SceneManager.LoadScene(levels[currentIndex]);
    }


    public void RestartLevel()
    {
        SceneManager.LoadScene(levels[currentIndex]);
    }


    public static int LevelNumber => currentIndex + 1;
}

