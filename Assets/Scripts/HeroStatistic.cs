using TMPro;
using UnityEngine;

public class HeroStotistic : MonoBehaviour
{
    public float health = 50;

    public float maxHealth = 100;

    public int armor = 0;

    private int startArmor = 0;

    public int score = 0;

    public int damage = 10;

    public TMP_Text healthPoint;

    public TMP_Text scrorePoint;

    public TMP_Text armorPoint;

    public TMP_Text damagePoint;

    private void Start()
    {
        healthPoint.text= health.ToString();
        scrorePoint.text = score.ToString();
        armorPoint.text= armor.ToString();
        damagePoint.text= damage.ToString();
    }

    public void setDamage(int _damage)
    {
        damage = _damage;
        damagePoint.text = damage.ToString();
    }
    public void damageHero(int _damage)
    {
        //Debug.Log("Ударили героя");
        //Debug.Log("Броня: " +  armor);
        //Debug.Log("Урон: " + _damage * (1 - (armor / 100)));
        if (health - Mathf.Round(_damage * (1 - (armor / 100))) <= 0)
        {
            health = 0;
        }
        else
        {
            health -= Mathf.Round(_damage * (1 - (armor / 100)));
        }
        healthPoint.text = health.ToString();
    }

    public void healHero(int _health)
    {
        if (health + _health > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += _health;
        }
       healthPoint.text = health.ToString();
    }

    public void setStartArmor()
    {
        armor = startArmor;
       // armorPoint.text = armor.ToString();
    }
    public void increaseArmor(int _armor)
    {
        //Debug.Log("БРОНЯ");
        armor += _armor;
        //armorPoint.text = armor.ToString();
    }

    public void increaseDamage(int _damage)
    {
        Debug.Log("Урон");
        damage += _damage;
        damagePoint.text = damage.ToString();
    }
    public void printArmor()
    {
        armorPoint.text = armor.ToString();
    }
}
