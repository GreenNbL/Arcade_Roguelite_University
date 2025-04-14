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
    public GameObject fencePrefab; // ������ ������
    private int mapWidth;
    private int mapHeight;
    public List<PrefabProbability> prefabs; // ������ �������� � ������������
    public LayerMask layerMask; // ����� ���� ��� �������� �����������
    public GameObject door;

    public GameObject sandPrefab;
    public GameObject dirtPrefab;
    public GameObject grassPrefab;
    public float tileSize = 0.25f; // ������ �������

    public int width = 500;
    public int height = 500;

    public float noiseScale = 0.05f;
    void Start()
    {
        // ��������� ���������� ������� ����� �� 30 �� 100
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
        // �������� ������ �����
        int envLayer = LayerMask.NameToLayer("Env");
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        // �������� ��� ������� � �����
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // �������� �� ���� ��������
        foreach (GameObject obj in allObjects)
        {
            // ���������, �� ����� ���� ��������� ������
            if (obj.layer == envLayer || obj.layer == enemyLayer)
            {
                Destroy(obj); // ������� ������
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
        // ������������� �����
        float startX = -mapWidth / 2;
        float startY = -mapHeight / 2;

        // ����������� ����� �� ����� �����
        for (float x = startX; x < startX + mapWidth-0.5; x+=0.5f)
        {
            PlaceFence(x, startY, false); // ������ ����
            PlaceFence(x, startY + mapHeight - 1, false); // ������� ����
        }

        for (float y = startY; y < startY + mapHeight - 0.5; y += 0.5f)
        {
            PlaceFence(startX, y, true); // ����� ����
            PlaceFence(startX + mapWidth - 1, y, true); // ������ ����
        }
    }

    private void PlaceFence(float x, float y, bool isVertical)
    {
        // ������� ��� ���������� ������
        Vector3 position = new Vector3(x, y, 0);
        GameObject fence = Instantiate(fencePrefab, position, Quaternion.identity);

        // ������������ ����� �� 90 ��������, ���� ��� ������������ �����
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
                float noiseValue = Mathf.PerlinNoise((x + 1000) * noiseScale, (y + 1000) * noiseScale); // +1000 � ����� �� ������� � ������������� �������� ����
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
