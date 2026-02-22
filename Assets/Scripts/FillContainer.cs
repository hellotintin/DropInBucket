using UnityEngine;
using TMPro;

// Attach to your container GameObject.
// No longer detects particles directly — WaterParticleHandler calls AddWater().
public class FillContainer : MonoBehaviour
{
    [Header("Fill Settings")]
    public float goalFill      = 100f;
    public float decreaseRate  = 6f;    // units/sec when no water arriving
    public float fillGrace     = 0.25f; // seconds before decrease kicks in

    [Header("UI")]
    public TextMeshProUGUI fillText;
    public GameObject      nextLevelBtn;

    [Header("Water Visual (optional)")]
    public Transform waterRiseVisual;
    public float     maxFillHeight = 2f;

    float currentFill;
    float graceTimer;
    bool  goalReached;

    void Start()
    {
        if (nextLevelBtn != null) nextLevelBtn.SetActive(false);
    }

    void Update()
    {
        if (goalReached) return;

        graceTimer -= Time.deltaTime;

        if (graceTimer <= 0f)
        {
            currentFill -= decreaseRate * Time.deltaTime;
            currentFill  = Mathf.Max(currentFill, 0f);
        }

        UpdateUI();
        UpdateWaterVisual();
    }

    // Called by WaterParticleHandler each frame particles land
    public void AddWater(float amount)
    {
        if (goalReached) return;

        graceTimer   = fillGrace;
        currentFill += amount;
        currentFill  = Mathf.Min(currentFill, goalFill);

        if (currentFill >= goalFill)
        {
            currentFill = goalFill;
            goalReached = true;
            if (nextLevelBtn != null) nextLevelBtn.SetActive(true);
        }
    }

    void UpdateUI()
    {
        if (fillText != null)
            fillText.text = Mathf.FloorToInt(currentFill) + " / "
                          + Mathf.FloorToInt(goalFill);
    }

    void UpdateWaterVisual()
    {
        if (waterRiseVisual == null) return;
        float ratio = currentFill / goalFill;
        Vector3 s   = waterRiseVisual.localScale;
        s.y = Mathf.Lerp(0.01f, maxFillHeight, ratio);
        waterRiseVisual.localScale = s;
    }

    public void GoToNextLevel()
    {
        if (LevelManager.instance != null)
            LevelManager.instance.LoadNextLevel();
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }
}