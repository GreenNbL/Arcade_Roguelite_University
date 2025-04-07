using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject fencePrefab; // ������ ������
    private int mapWidth;
    private int mapHeight;
    public List<PrefabProbability> prefabs; // ������ �������� � ������������
    public LayerMask layerMask; // ����� ���� ��� �������� �����������

    void Start()
    {
        // ��������� ���������� ������� ����� �� 30 �� 100
        mapWidth = Random.Range(30, 101);
        mapHeight = Random.Range(30, 101);
        GenerateMap();
        Envirenment envSpawner =new Envirenment(prefabs, mapWidth, mapHeight, layerMask);
        envSpawner.SpawnPrefabsInGrid();
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
        GenerateMap();
        Envirenment envSpawner = new Envirenment(prefabs, mapWidth, mapHeight, layerMask);
        envSpawner.SpawnPrefabsInGrid();
    }
    private void GenerateMap()
    {
        // ������������� �����
        int startX = -mapWidth / 2;
        int startY = -mapHeight / 2;

        // ����������� ����� �� ����� �����
        for (int x = startX; x < startX + mapWidth; x++)
        {
            PlaceFence(x, startY, false); // ������ ����
            PlaceFence(x, startY + mapHeight - 1, false); // ������� ����
        }

        for (int y = startY; y < startY + mapHeight; y++)
        {
            PlaceFence(startX, y, true); // ����� ����
            PlaceFence(startX + mapWidth - 1, y, true); // ������ ����
        }
    }

    private void PlaceFence(int x, int y, bool isVertical)
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
}
