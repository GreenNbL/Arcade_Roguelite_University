using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Получаем ввод от игрока
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Создаем вектор движения
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized; // Нормализация для равномерной скорости

        // Перемещаем игрока
        rb.linearVelocity = movement * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      
        // Остановить игрока при столкновении с объектом, который имеет тег "Wall"
        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.linearVelocity = Vector2.zero; // Останавливаем движение
        }
    }
}

