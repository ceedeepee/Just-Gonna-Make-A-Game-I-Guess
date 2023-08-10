using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseMenu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
                pauseMenu.SetActive(false);
            }
            else
            {
                pauseMenu.SetActive(true);
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        // Optionally, you can also show a pause menu here.
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        // Optionally, you can also hide the pause menu here.
    }
}