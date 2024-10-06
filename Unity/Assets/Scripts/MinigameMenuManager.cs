using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameMenuManager : MonoBehaviour
{
    public GameObject pausePanel; // Reference to the pause panel
    public GameObject resultsPanel; // Reference to the results panel
    public GameObject pauseButton; // Reference to the pause button (to show/hide it)
    public string mainMenuSceneName = "MainMenu"; // Hardcoded name of the Main Menu scene
    public TMP_Text algaeText;
    public TMP_Text coinCountText;

    private bool isPaused = false; // Check if the game is paused

    private void OnEnable()
    {
        EventManager.OnLevelCompletion += OpenResultsPanel;
    }

    private void OnDisable()
    {
        EventManager.OnLevelCompletion -= OpenResultsPanel;
    }

    void Start()
    {
        // Ensure no panel is open at the start, and the pause button is visible
        pausePanel.SetActive(false);
        resultsPanel.SetActive(false);
        pauseButton.SetActive(true); // Ensure the pause button is visible at start
    }

    void Update()
    {
        // Toggle pause menu with the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ClosePauseMenu();
            }
            else
            {
                OpenPauseMenu();
            }
        }
    }

    // Opens the pause menu and pauses the game
    public void OpenPauseMenu()
    {
        pausePanel.SetActive(true);
        pauseButton.SetActive(false); // Hide the pause button when the pause menu is open
        Time.timeScale = 0f; // Stop time in the game
        isPaused = true;
    }

    // Closes the pause menu and resumes the game
    public void ClosePauseMenu()
    {
        pausePanel.SetActive(false);
        pauseButton.SetActive(true); // Show the pause button when the pause menu is closed
        Time.timeScale = 1f; // Resume time in the game
        isPaused = false;
    }

    // Opens the results panel (e.g., when the minigame ends)
    public void OpenResultsPanel()
    {
        resultsPanel.SetActive(true);
        pauseButton.SetActive(false); // Hide pause button when results are shown
        Time.timeScale = 0f; // Pause the game when showing results
    }

    // Button action to return to the main menu
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Make sure to resume time before changing the scene
        SceneManager.LoadScene(mainMenuSceneName); // Load the main menu scene
    }

    private void UpdateUI()
    {
        algaeText.text = "Algae: " + GameManager.Instance.GetTotalAlgae().ToString();
        coinCountText.text = "Coins: " + GameManager.Instance.GetCoinCount().ToString();
    }
}
