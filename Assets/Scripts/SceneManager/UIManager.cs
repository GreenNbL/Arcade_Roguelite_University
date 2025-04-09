using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public GameObject startGamePanel;
    public GameObject settingsPanel;
    public GameObject scorePanel;
    public TMP_InputField namefield;

    private string name;
    private string filePath;

    private List<GameData> dataList = new List<GameData>();

    public GameObject itemPrefab; // Префаб с двумя текстовыми полями
    public Transform contentPanel; // Объект с GridLayoutGroup

    private void Start()
    {
        filePath = Path.Combine(Application.dataPath, "../gamedata.txt");
        filePath = Path.GetFullPath(filePath); // нормализуем путь
        startGamePanel.SetActive(true);
        settingsPanel.SetActive(false);
        scorePanel.SetActive(false);
    }
    public void PopulateUI()
    {
        // Удаляем старые элементы, если есть
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
        int i = 1;
        dataList.Sort((a, b) => b.playerScore.CompareTo(a.playerScore));
        // Добавляем новые элементы
        foreach (var data in dataList)
        {
            GameObject item = Instantiate(itemPrefab, contentPanel);

            // Используем TMP
            TextMeshProUGUI[] texts = item.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length >= 2)
            {
                texts[0].text=i.ToString();
                texts[1].text = data.playerName;
                texts[2].text = data.playerScore.ToString();
            }
            i++;
            // Если используешь обычный Text:
            // Text[] texts = item.GetComponentsInChildren<Text>();
            // texts[0].text = data.playerName;
            // texts[1].text = data.playerScore.ToString();
        }
    }
    public void LoadAllGameData()
    {
        dataList.Clear();
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line) && line.TrimStart().StartsWith("{"))
                {
                    try
                    {
                        GameData data = JsonUtility.FromJson<GameData>(line);
                        if (data != null)
                            dataList.Add(data);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogWarning($"Ошибка при чтении строки: {line}\n{e.Message}");
                    }
                }
            }
        }
    }
    public void StartGame()
    {
        TextMeshProUGUI placeholderText = namefield.placeholder as TextMeshProUGUI;
        StaticGameData.playerName = placeholderText.text;
        if (namefield.text != "")
            StaticGameData.playerName = namefield.text;
        SceneManager.LoadScene("GamingScene");
    }

    public void StopGame()
    {
        SceneManager.LoadScene("Menu");
    }
    public void openStartPanel()
    {
        startGamePanel.SetActive(true);
        settingsPanel.SetActive(false);
        scorePanel.SetActive(false);
    }
    public void openScorePanel()
    {
        LoadAllGameData();
        PopulateUI();
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
