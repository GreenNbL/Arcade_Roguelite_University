using UnityEngine;
using static UnityEditor.Progress;

public class HitHero : MonoBehaviour 
{
   public int damage;
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("������� �����");

        if (other.gameObject.CompareTag("Player")) // ���������, ��� ������������ � �������� ������
        {
            Debug.Log("������� �����");
            other.gameObject.GetComponent<HeroStotistic>().damageHero(damage);
        }
        
    }
}
