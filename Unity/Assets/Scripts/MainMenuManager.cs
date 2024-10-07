using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // References to the panels
    public GameObject mainMenuPanel;
    public GameObject playPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    // References to level buttons
    public GameObject level6Button; // Reference to the 6th level button

    void Start()
    {
        // Ensure the main menu is active at the start
        ShowMainMenu();

        // Hard-code the visibility of the 6th level button to false for now
        level6Button.SetActive(false); // TODO: Hook up to player progress and save data
    }

    // Show the main menu panel and hide others
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        playPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    // Show the play panel and hide others
    public void ShowPlayMenu()
    {
        mainMenuPanel.SetActive(false);
        playPanel.SetActive(true);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    // Show the settings panel and hide others
    public void ShowSettingsMenu()
    {
        mainMenuPanel.SetActive(false);
        playPanel.SetActive(false);
        settingsPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    // Show the credits panel and hide others
    public void ShowCreditsMenu()
    {
        mainMenuPanel.SetActive(false);
        playPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    // Load the corresponding level based on the button clicked
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    // Method to return to the main menu from any of the child panels
    public void GoBackToMainMenu()
    {
        ShowMainMenu(); // Simply call ShowMainMenu to go back
    }
}
