using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    private Transform player; // Ссылка на объект игрока
    public float chaseRadius = 10f; // Радиус преследования
    public float stoppingDistance = 1f; // Расстояние, на котором враг останавливается от игрока
    public float speed = 2f; // Скорость врага
    public LayerMask obstacleMask; // Маска для определения препятствий
    public float avoidanceDistance = 2f; // Расстояние для обнаружения препятствий

    private Collider2D enemyCollider; // Коллайдер врага
    public float health; // Здоровье врага
    public float maxHealth = 100f; // Здоровье врага
    public float attackDamage = 10f; // Урон атаки врага
    public int level = 1; // Уровень врага
    public float attackSpeed = 1f; // Скорость атаки (количество ударов в секунду)

    private bool isChasing = false; // Статус преследования
    private float attackCooldown = 0f; // Задержка между атаками

    public int score;

    public Animator animator;

    private bool died = false;

    public Image healthBar;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyCollider = GetComponent<Collider2D>(); // Инициализация коллайдера
        AdjustStatsBasedOnLevel(); // Настройка параметров в зависимости от уровня
        health=maxHealth;
    }

    void Update()
    {
        if (!died)
        {
            attackCooldown -= Time.deltaTime; // Уменьшение времени до следующей атаки
            healthBar.fillAmount = (float)health / (float)maxHealth;
            float distanceToPlayer = Vector2.Distance(player.position, transform.position);
            isChasing = distanceToPlayer <= chaseRadius;

            if (isChasing)
            {
                // Проверка, есть ли преграды между врагом и игроком
                if (IsPathClear(player.position))
                {
                    ChasePlayer();
                }
                else
                {
                    AvoidObstacles();
                }

                // Дополнительно проверить на атаку
                TryAttackPlayer(distanceToPlayer);
            }
        }
        else
        {
            healthBar.fillAmount =0;
        }
    }

    /*private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // Проверка расстояния до игрока для остановки
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceToPlayer > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }*/
    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // Проверка расстояния до игрока для остановки
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceToPlayer > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // Поворот врага в сторону движения
            if (direction.x > 0 && transform.localScale.x > 0)
            {
                // Если игрок находится справа и враг смотрит влево, то поворачиваем вправо
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1); // Разворачивание вправо
            }
            else if (direction.x < 0 && transform.localScale.x < 0)
            {
                // Если игрок находится слева и враг смотрит вправо, то поворачиваем влево
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1); // Разворачивание влево
            }
        }
    }



    private void AvoidObstacles()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        Vector2 rightDirection = new Vector2(directionToPlayer.y, -directionToPlayer.x); // Направление вправо
        Vector2 leftDirection = new Vector2(-directionToPlayer.y, directionToPlayer.x); // Направление влево

        // Проверка направлений на наличие препятствий
        if (IsPathClear((Vector2)transform.position + rightDirection * avoidanceDistance))
        {
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + rightDirection, speed * Time.deltaTime);
        }
        else if (IsPathClear((Vector2)transform.position + leftDirection * avoidanceDistance))
        {
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + leftDirection, speed * Time.deltaTime);
        }
        else
        {
            // Если оба направления заблокированы, слегка отойти назад
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position - directionToPlayer, speed * Time.deltaTime);
        }
    }

    private bool IsPathClear(Vector2 targetPosition)
    {
        float radius = GetColliderRadius();
        float distance = Vector2.Distance(transform.position, targetPosition);
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, (targetPosition - (Vector2)transform.position).normalized, distance, obstacleMask);

        return hit.collider == null; // Путь свободен
    }

    private float GetColliderRadius()
    {
        float radius = 0.5f; // Значение по умолчанию
        if (enemyCollider is CircleCollider2D circleCollider)
        {
            radius = circleCollider.radius; // Радиус круга
        }
        else if (enemyCollider is BoxCollider2D boxCollider)
        {
            radius = boxCollider.size.x / 2; // Половина ширины
        }
        return radius;
    }

    // Метод для обработки атаки игрока
    private void TryAttackPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer <= stoppingDistance && attackCooldown <= 0f)
        {
            animator.SetTrigger("attack");
            attackCooldown = 1f / attackSpeed; // Время между атаками, основанное на скорости атаки
        }
    }
    // Метод для нанесения урона
    public void DealDamage()
    {
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceToPlayer <= stoppingDistance)
        {
            player.gameObject.GetComponent<HeroStotistic>().damageHero((int)attackDamage);
            Debug.Log("Урон нанесен игроку!");
        }
    }

    // Метод получения урона от героя
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
            died = true;
            enemyCollider.enabled = false;
        }
    }

    // Метод для обработки смерти врага
    private void Die()
    {
        // Здесь можно добавить логику, которая будет выполняться при смерти врага
        //Destroy(gameObject); // Уничтожить объект врага
        animator.SetTrigger("died");
    }
    public void Grave()
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 1);
    }
    // Метод для применения изменений в соответствии с уровнем
    private void AdjustStatsBasedOnLevel()
    {
        // Увеличиваем здоровье и урон в зависимости от уровня
        health += level * 10; // Пример: увеличение здоровья на 10 за уровень
        attackDamage += level * 2; // Пример: увеличение урона на 2 за уровень
        speed += level * 0.5f; // Может быть полезным для увеличения скорости
        attackSpeed += level * 0.1f; // Увеличение скорости атаки
    }
}
