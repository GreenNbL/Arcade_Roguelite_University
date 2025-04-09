using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public GameObject pausePanel;
    private string filePath ;

    private void Start()
    {
        filePath = Path.Combine(Application.dataPath, "../gamedata.txt");
        filePath = Path.GetFullPath(filePath); // ����������� ����
        pausePanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel.activeSelf == true)
                pausePanel.SetActive(false);
            else 
                pausePanel.SetActive(true);
        }
    }
    public void StopGame()
    {
        AddGameData();
        SceneManager.LoadScene("Menu");
    }
    public void closePausePanel()
    {
        pausePanel.SetActive(false);
    }
    public void AddGameData()
    {
        // ���������� ������ ������ ������
        GameData newData = new GameData
        {
            playerName = StaticGameData.playerName,
            playerScore = int.Parse(GameObject.Find("Player").GetComponent<HeroStotistic>().scrorePoint.text)
        };
        SaveData(newData);
    }

    public void SaveData(GameData newData)
    {
        // ������������ ������� � JSON
        string json = JsonUtility.ToJson(newData);
        File.AppendAllText(filePath, json + "\n"); // ���� ������ �� ������
        Debug.Log("���� ��������� � ����");
        Debug.Log(filePath);
    }
}
