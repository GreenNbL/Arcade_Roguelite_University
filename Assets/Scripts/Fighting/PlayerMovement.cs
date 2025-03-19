using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;

    public Animator animator;

    public float radiusAttack;

    public LayerMask mask;

    private Collider2D heroCollider; // Ссылка на коллайдер героя
    private Vector3 lastPosition; // Хранит последнее положение героя
    public bool isInvisible = false; // Статус невидимости
    private SpriteRenderer spriteRenderer; // Ссылка на SpriteRenderer
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        heroCollider = GetComponent<Collider2D>(); // Получаем коллайдер
        spriteRenderer = GetComponent<SpriteRenderer>(); // Получаем SpriteRenderer
    }

    void Update()
    {
        if (!isInvisible)
        {
           // Debug.Log("Идем");
            MovePlayer();
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("Вылазим");
            if (isInvisible)
            {
               // Debug.Log("Вылазим");
                // Если герой уже невидимый, возвращаемся на место
                ReturnToLastPosition();
            }
            else
            {
               // Debug.Log("Вылазим3");
                // Проверяем наличие предмета в радиусе 1 с тегом "Bushe"
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);

                foreach (Collider2D hit in hits)
                {
                    if (hit.CompareTag("Bushe"))
                    {
                        MoveToBushe(hit.transform.position); // Перемещаем героя к Bushe
                        break; // Выход из цикла после перемещения
                    }
                } 
            }
        }
    }
    private void MoveToBushe(Vector3 bushePosition)
    {
        //Debug.Log("Вылазим4");
        rb.linearVelocity = Vector2.zero; 
        isInvisible = true; // Обновляем статус
        lastPosition = transform.position; // Сохраняем текущее положение
        transform.position = bushePosition; // Перемещаем героя к Bushe
        heroCollider.enabled = false; // Коллайдер остается включенным
        spriteRenderer.enabled = false; // Делаем героя невидимым (отключаем визуализацию)
       
    }

    private void ReturnToLastPosition()
    {
        transform.position = lastPosition; // Возвращаем героя на последнее положение
        heroCollider.enabled = true; // Включаем коллайдер
        spriteRenderer.enabled = true; // Делаем героя видимым
        isInvisible = false; // Обновляем статус
    }
    private void MovePlayer()
    {
       // Debug.Log("Идем 2");
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

