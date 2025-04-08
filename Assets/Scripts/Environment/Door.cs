using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen;
    public Animator animator;
    private bool isAllDied;
    float delay = 2f; // задержка в секундах
    float timer = 0f;
    void Start()
    {
        isOpen = false;
        isAllDied = false;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= delay)
        {
            isAllEnemiesDied();
            if (isAllDied == true)
            {
                animator.SetTrigger("openDoor");
                // Debug.Log("Дверь открыта");
                isOpen = true;
            }
        }
    }

    List<GameObject> findAllEnemies()
    {
        // Получаем все объекты с тегом "Enemy"
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> enemyObjects = new List<GameObject>();

        foreach (GameObject obj in allEnemies)
        {
            enemyObjects.Add(obj);
        }

        return enemyObjects;
    }
    void isAllEnemiesDied()
    {
        List<GameObject> enemyObjects = findAllEnemies();
        foreach (GameObject obj in enemyObjects)
        {
            if (obj.name.Contains("enemy_prefa"))
            {
                if (obj.GetComponent<EnemyAI>().died != true)
                {
                    isAllDied = false;
                    return;
                }
            }
            else if (obj.name.Contains("Archer"))
            {
                if (obj.GetComponent<EnemyAI_Archer>().died != true)
                {
                    isAllDied = false;
                    return;
                }

            }
        }
        isAllDied = true;
    }
}
