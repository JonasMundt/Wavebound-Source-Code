using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("Standard Game Over")]
    public GameObject gameOverUI;

    [Header("Endscreen (nach Boss)")]
    public GameObject endScreenUI;
    public TMP_Text endScreenText;

    public void ShowGameOver()
    {
        gameOverUI.SetActive(true);
    }

public void ShowEndScreen()
{
    Debug.Log("ShowEndScreen() wird aufgerufen.");
    if (endScreenUI != null)
    {
        endScreenUI.SetActive(true);
    }

    if (endScreenText != null)
    {
        endScreenText.text = "Das war die Demo von Wavebound! Danke fürs Spielen.";
    }

    Time.timeScale = 0f;
}



    public void RestartGame()
    {
        Debug.Log("🔁 Restarting...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
