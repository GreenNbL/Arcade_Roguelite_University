using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;

    public Animator animator;

    public float radiusAttack;

    public LayerMask mask;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MovePlayer();
        Attack();
    }

    private void MovePlayer()
    {
        // Получаем ввод от игрока
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        if (moveHorizontal > 0) // Двигаемся вправо
        {
            transform.localScale = new Vector3(1, 1, 1); // Лицом вправо
        }
        else if (moveHorizontal < 0) // Двигаемся влево
        {
            transform.localScale = new Vector3(-1, 1, 1); // Лицом влево
        }
        // Создаем вектор движения
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized; // Нормализация для равномерной скорости

        if (movement.magnitude > 0)
        {
            animator.SetFloat("speed", 1);
        }
        else
            animator.SetFloat("speed", 0);
        // Перемещаем игрока
        rb.linearVelocity = movement * moveSpeed;
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("attack");
           
        }
    }
    public void AttackTimeAnimation()
    {
        Collider2D[] hittedEnemies = Physics2D.OverlapCircleAll(transform.position, radiusAttack, mask);

        foreach (Collider2D collider in hittedEnemies)
        {
            EnemyAI enemyScript = collider.GetComponent<EnemyAI>(); // Измените на ваш класс
            if (enemyScript != null)
            {
                // Теперь вы можете взаимодействовать со скриптом
                //Debug.Log("Найден скрипт Enemy!");
                enemyScript.TakeDamage(GetComponent<HeroStotistic>().damage); // Пример вызова метода TakeDamage
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
      
        // Остановить игрока при столкновении с объектом, который имеет тег "Wall"
        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.linearVelocity = Vector2.zero; // Останавливаем движение
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radiusAttack);
    }
}

