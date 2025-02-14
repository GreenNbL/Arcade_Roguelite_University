using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // скорость движения персонажа
    void Update()
    {
        // Получаем ввод с клавиатуры
        float moveHorizontal = Input.GetAxisRaw("Horizontal"); // A/D
        float moveVertical = Input.GetAxisRaw("Vertical");     // W/S

        // Вектор движения
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;

        // Двигаем персонажа
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}
