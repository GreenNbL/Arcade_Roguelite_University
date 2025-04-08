using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class UIManager : MonoBehaviour
{
    public GameObject startGamePanel;
    public GameObject settingsPanel;
    public GameObject scorePanel;

    private void Start()
    {
        startGamePanel.SetActive(true);
        settingsPanel.SetActive(false);
        scorePanel.SetActive(false);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("GamingScene");
    }
    public void openStartPanel()
    {
        startGamePanel.SetActive(true);
        settingsPanel.SetActive(false);
        scorePanel.SetActive(false);
    }
    public void openScorePanel()
    {
        startGamePanel.SetActive(false);
        settingsPanel.SetActive(false);
        scorePanel.SetActive(true);
    }
    public void openSettingsPanel()
    {
        startGamePanel.SetActive(false);
        settingsPanel.SetActive(true);
        scorePanel.SetActive(false);
    }

}
