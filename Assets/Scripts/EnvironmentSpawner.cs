using UnityEngine;
using System.Collections.Generic; 
public class Envirenment : MonoBehaviour
{
    public GameObject prefab; // Ваш префаб
    public float spawnRadius = 10f; // Радиус, в котором будут появляться префабы
    public int initialSpawnCount = 20; // Начальное количество префабов
    public float distanceThreshold = 5f; // Расстояние от игрока для спавна новых объектов
    public Camera mainCamera; // Ссылка на основную камеру
    private Vector2 bottomLeft; // Левая нижняя точка
    private Vector2 topRight; // Правая верхняя точка 
    private Transform player; 
    private Vector3 lastPlayerPosition;
    public List<PrefabProbability> prefabs; // Список префабов с вероятностью

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform; // Тег игрока
        lastPlayerPosition = player.position;
        GetCoordinatesOfStartingArea();
        SpawnPrefabsInGrid();
    }

    void Update()
    {
        if (Vector3.Distance(player.position, lastPlayerPosition) > distanceThreshold)
        {
            lastPlayerPosition = player.position;
            SpawnPrefab(); // Генерируем новый префаб
        }
    }

    private void SpawnPrefab()
    {
        // Генерация случайной позиции за пределами видимости камеры
        Vector3 spawnPosition = GetRandomOffScreenPosition();
        Instantiate(prefab, spawnPosition, Quaternion.identity); // Создание префаба
    }

    private void GetCoordinatesOfStartingArea()
    {
        // Получение границ камеры
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        Vector3 lastPlayerPosition = player.position;

        this.bottomLeft.x = lastPlayerPosition.x-(cameraWidth/2);
        this.bottomLeft.y = lastPlayerPosition.y-(cameraHeight/2);

        this.topRight.x = lastPlayerPosition.x+(cameraWidth/2);
        this.topRight.y = lastPlayerPosition.y+(cameraHeight/2);
    }

    void SpawnPrefabsInGrid()
    {
        for (float x = this.bottomLeft.x; x <= this.topRight.x; x++)
        {
            for (float y = this.bottomLeft.y ; y <= this.topRight.y; y++)
            {
                Vector2 spawnPosition = new Vector2(x, y); // Позиция клетки
                SpawnPrefabWithProbability(spawnPosition);
            }
        }
    }

    void SpawnPrefabWithProbability(Vector2 position)
    {
        float rand = Random.value; // Случайное число от 0 до 1

        foreach (var prefabProb in prefabs)
        {
            if (rand <= prefabProb.GetProbability())
            {
                Debug.Log("Random.value =" + rand);
                Debug.Log("CumulativeProbability ="+  prefabProb.GetProbability());
                Instantiate(prefabProb.GetPrefab(), position, Quaternion.identity); // Создание префаба
                break; // Выход из цикла после успешного спавна
            }
        }
    }

    private Vector3 GetRandomOffScreenPosition()
    {
        // Получение границ камеры
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Определяем границы по X и Y
        float spawnX = Random.Range(-cameraWidth / 2, cameraWidth / 2);
        float spawnY;

        // Задаем Y в зависимости от того, нужно ли генерировать выше или ниже экрана
        if (Random.value > 0.5f)
        {
            spawnY = Random.Range(mainCamera.transform.position.y + mainCamera.orthographicSize, 
                                   mainCamera.transform.position.y + mainCamera.orthographicSize + spawnRadius); // Выше
        }
        else
        {
            spawnY = Random.Range(mainCamera.transform.position.y - mainCamera.orthographicSize - spawnRadius,
                                   mainCamera.transform.position.y - mainCamera.orthographicSize); // Ниже
        }

        return new Vector3(spawnX, spawnY, 0); // Возвращаем новую позицию
    }
}
