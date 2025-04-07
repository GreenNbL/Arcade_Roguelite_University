using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject fencePrefab; // Префаб забора
    private int mapWidth;
    private int mapHeight;
    public List<PrefabProbability> prefabs; // Список префабов с вероятностью
    public LayerMask layerMask; // Маска слоя для проверки пересечения

    void Start()
    {
        // Генерация случайного размера карты от 30 до 100
        mapWidth = Random.Range(30, 101);
        mapHeight = Random.Range(30, 101);
        GenerateMap();
        Envirenment envSpawner =new Envirenment(prefabs, mapWidth, mapHeight, layerMask);
        envSpawner.SpawnPrefabsInGrid();
    }
    void RemoveObjectsOnLayers()
    {
        // Получаем индекс слоев
        int envLayer = LayerMask.NameToLayer("Env");
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        // Получаем все объекты в сцене
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Проходим по всем объектам
        foreach (GameObject obj in allObjects)
        {
            // Проверяем, на каком слое находится объект
            if (obj.layer == envLayer || obj.layer == enemyLayer)
            {
                Destroy(obj); // Удаляем объект
            }
        }
    }


    public void RegerateMap()
    {
        RemoveObjectsOnLayers();
        mapWidth = Random.Range(30, 101);
        mapHeight = Random.Range(30, 101);
        GenerateMap();
        Envirenment envSpawner = new Envirenment(prefabs, mapWidth, mapHeight, layerMask);
        envSpawner.SpawnPrefabsInGrid();
    }
    private void GenerateMap()
    {
        // Центрирование карты
        int startX = -mapWidth / 2;
        int startY = -mapHeight / 2;

        // Расставляем забор по краям карты
        for (int x = startX; x < startX + mapWidth; x++)
        {
            PlaceFence(x, startY, false); // Нижний край
            PlaceFence(x, startY + mapHeight - 1, false); // Верхний край
        }

        for (int y = startY; y < startY + mapHeight; y++)
        {
            PlaceFence(startX, y, true); // Левый край
            PlaceFence(startX + mapWidth - 1, y, true); // Правый край
        }
    }

    private void PlaceFence(int x, int y, bool isVertical)
    {
        // Позиция для размещения забора
        Vector3 position = new Vector3(x, y, 0);
        GameObject fence = Instantiate(fencePrefab, position, Quaternion.identity);

        // Поворачиваем забор на 90 градусов, если это вертикальная часть
        if (isVertical)
        {
            fence.transform.Rotate(0, 0, 90);
        }
    }
}
