using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 2f;
    public float maxDistance = 15f;
    private Vector3 startPos;
    private Vector3 targetDirection;
    private int damage;
    public void Initialize(Vector3 targetPosition, int _damage)
    {
        startPos = transform.position;
        targetDirection = (targetPosition - transform.position).normalized;
        transform.right = targetDirection; // Повернуть стрелу в нужном направлении
        damage=_damage ;
    }

    void Update()
    {
        transform.position += targetDirection * speed * Time.deltaTime;

        // Проверка максимальной дистанции
        if (Vector3.Distance(startPos, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
        
            other.gameObject.GetComponent<HeroStotistic>().damageHero(damage);
      
        }

        Destroy(gameObject); // Стрела всё равно исчезает
    }
}
