using System.Collections.Generic;
using UnityEngine;
public enum BiomeType
{
    Sand,
    Dirt,
    Grass
}
public class MapGenerator : MonoBehaviour
{
    public GameObject fencePrefab; // Префаб забора
    private int mapWidth;
    private int mapHeight;
    public List<PrefabProbability> prefabs; // Список префабов с вероятностью
    public LayerMask layerMask; // Маска слоя для проверки пересечения
    public GameObject door;

    public GameObject sandPrefab;
    public GameObject dirtPrefab;
    public GameObject grassPrefab;
    public float tileSize = 0.25f; // размер спрайта

    public int width = 500;
    public int height = 500;

    public float noiseScale = 0.05f;
    void Start()
    {
        // Генерация случайного размера карты от 30 до 100
        mapWidth = Random.Range(30, 101);
        mapHeight = Random.Range(30, 101);
        Vector3 pos = new Vector3(1.5f, 0.4f, 0f);
        Instantiate(door, pos, Quaternion.identity);
        GenerateMap();
        Envirenment envSpawner =new Envirenment(prefabs, mapWidth, mapHeight, layerMask);
        envSpawner.SpawnPrefabsInGrid();
        GenerateBiomes();
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
        Vector3 pos = new Vector3(1.5f, 0.4f, 0f);
        Instantiate(door, pos, Quaternion.identity);
        GenerateMap();
        Envirenment envSpawner = new Envirenment(prefabs, mapWidth, mapHeight, layerMask);
        envSpawner.SpawnPrefabsInGrid();
    }
    private void GenerateMap()
    {
        // Центрирование карты
        float startX = -mapWidth / 2;
        float startY = -mapHeight / 2;

        // Расставляем забор по краям карты
        for (float x = startX; x < startX + mapWidth-0.5; x+=0.5f)
        {
            PlaceFence(x, startY, false); // Нижний край
            PlaceFence(x, startY + mapHeight - 1, false); // Верхний край
        }

        for (float y = startY; y < startY + mapHeight - 0.5; y += 0.5f)
        {
            PlaceFence(startX, y, true); // Левый край
            PlaceFence(startX + mapWidth - 1, y, true); // Правый край
        }
    }

    private void PlaceFence(float x, float y, bool isVertical)
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
    void GenerateBiomes()
    {
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                float noiseValue = Mathf.PerlinNoise((x + 1000) * noiseScale, (y + 1000) * noiseScale); // +1000 — чтобы не уходить в отрицательные значения шума
                BiomeType biome = GetBiomeFromNoise(noiseValue);

                GameObject prefab = GetPrefabFromBiome(biome);
                Vector3 position = new Vector3(x * tileSize, y * tileSize, 10f);
                Instantiate(prefab, position, Quaternion.identity, transform);
            }
        }
    }

    BiomeType GetBiomeFromNoise(float value)
    {
        if (value < 0.33f)
            return BiomeType.Sand;
        else if (value < 0.66f)
            return BiomeType.Dirt;
        else
            return BiomeType.Grass;
    }

    GameObject GetPrefabFromBiome(BiomeType biome)
    {
        switch (biome)
        {
            case BiomeType.Sand: return sandPrefab;
            case BiomeType.Dirt: return dirtPrefab;
            case BiomeType.Grass: return grassPrefab;
            default: return null;
        }
    }
}
