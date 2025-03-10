using UnityEngine;
using static UnityEditor.Progress;

public class HitHero : MonoBehaviour 
{
   public int damage;
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Ударили героя");

        if (other.gameObject.CompareTag("Player")) // Проверяем, что столкновение с объектом игрока
        {
            Debug.Log("Ударили героя");
            other.gameObject.GetComponent<HeroStotistic>().damageHero(damage);
        }
        
    }
}
