using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class Envirenment : MonoBehaviour
{
    private  List<PrefabProbability> prefabs; // Список префабов с вероятностью
    private  int[,] mapEnv;
    private LayerMask layerMask; // Маска слоя для проверки пересечения

    public Envirenment(List<PrefabProbability> prefabs, int x, int y, LayerMask layerMask)
    {
        mapEnv = new int[x-2 , y-2 ];
        this.layerMask = layerMask;
        this.prefabs = prefabs.OrderBy(p => p.GetProbability()).ToList();
    }

    public void SpawnPrefabsInGrid()
    {
        for (float x = (-this.mapEnv.GetLength(0)/2) ; x < (this.mapEnv.GetLength(0) / 2)-2; x++)
        {
            for (float y = (-this.mapEnv.GetLength(1) / 2) ;  y < (this.mapEnv.GetLength(1) / 2)-2 ; y++)
            {
                Vector2 spawnPosition = new Vector2(x, y); // Позиция клетки
                SpawnPrefabWithProbability(spawnPosition);
            }
        }
    }

    private void SpawnPrefabWithProbability(Vector2 position)
    {
       
        foreach (var prefabProb in prefabs)
        {
            float rand = Random.value; // Случайное число от 0 до 1

            BoxCollider2D collider=null;

            if (prefabProb.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (prefabProb.transform.Find("enemy_prefab") != null)
                    collider = prefabProb.transform.Find("enemy_prefab").GetComponent<BoxCollider2D>();
                else if (prefabProb.transform.Find("Archer") != null)
                    collider = prefabProb.transform.Find("Archer").GetComponent<BoxCollider2D>();
            }
            else
            {
                collider = prefabProb.GetComponent<BoxCollider2D>();
            }
            bool isOverlapping = Physics2D.OverlapBox(position, collider.size, 0f, layerMask);

            if (!isOverlapping)
            {
                if (rand <= prefabProb.GetProbability())
                {
                    //Debug.Log("Random.value =" + rand);
                    //Debug.Log("CumulativeProbability =" + prefabProb.GetProbability());
                    Instantiate(prefabProb.GetPrefab(), position, Quaternion.identity); // Создание префаба
                    break; // Выход из цикла после успешного спавна
                }
            }
        
           
        }
    }
}
