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
        filePath = Path.GetFullPath(filePath); // нормализуем путь
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
        // Добавление нового набора данных
        GameData newData = new GameData
        {
            playerName = StaticGameData.playerName,
            playerScore = int.Parse(GameObject.Find("Player").GetComponent<HeroStotistic>().scrorePoint.text)
        };
        SaveData(newData);
    }

    public void SaveData(GameData newData)
    {
        // Сериализация объекта в JSON
        string json = JsonUtility.ToJson(newData);
        File.AppendAllText(filePath, json + "\n"); // ОДНА строка на объект
        Debug.Log("инфа добавлена в файл");
        Debug.Log(filePath);
    }
}
