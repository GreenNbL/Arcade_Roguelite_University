using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroStotistic : MonoBehaviour
{
    public float health = 50;

    public float maxHealth = 100;

    public int armor = 0;

    private int startArmor = 0;

    private int startAtackSpeed = 10;

    public int atackSpeed = 10;

    public int chanceSriticalDamage = 0;

   // public int criticalDamage = 0;

    public int score = 0;

    public int damage = 10;

    public int healScale = 0;

    public int startHealScale = 0;

    public int startDamage = 10;

    public TMP_Text healthPoint;

    public TMP_Text scrorePoint;

    public TMP_Text armorPoint;

    public TMP_Text damagePoint;

    public Image firstLife;

    public Image secondLife;

    private void Start()
    {
        healthPoint.text= health.ToString();
        scrorePoint.text = score.ToString();
        armorPoint.text= armor.ToString();
        damagePoint.text= damage.ToString();
    }
    public void increadeScore(int _score)
    {
        score += _score;
        scrorePoint.text= score.ToString();
    }
    public void setDamage(int _damage)
    {
        damage += _damage;
    }
    public void setStartDamage()
    {
        damage = startDamage;
    }
    public void printDamage()
    {
        damagePoint.text = damage.ToString();
    }
    public  void increaseChanceSciticalDamage(int chance)
    {
        chanceSriticalDamage += chance;
    }
    public void setStartChanceSciticalDamage()
    {
        chanceSriticalDamage =0;
    }
    public void damageHero(int _damage)
    {
        //Debug.Log("Ударили героя");
        //Debug.Log("Броня: " +  armor);
        //Debug.Log("Урон: " + _damage * (1 - (armor / 100)));
        if (health - Mathf.Round(_damage * (1 - (armor / 100f))) <= 0)
        {
            health = 0;
            GetComponent<PlayerMovement>().Respawn();
            health = maxHealth;
            if(secondLife.IsActive())
                secondLife.enabled = false;
            else if (firstLife.IsActive())
                firstLife.enabled = false;
        }
        else
        {
            health -= Mathf.Round(_damage * (1 - (armor / 100f)));
        }
        healthPoint.text = health.ToString();
    }
    GameObject[] FindEnemiesByTag(string tag)
    {
        // Получаем все объекты с указанным тегом
        GameObject[] enemiesWithTag = GameObject.FindGameObjectsWithTag(tag);

        return enemiesWithTag; // Возвращаем массив найденных врагов
    }
    public void setStarthealScale()
    {
        healScale = startHealScale;
    }
    public void healHero(int _health)
    {
        if (health + _health*(1+(healScale/100f)) > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += _health * (1 + (healScale / 100f));
        }
       healthPoint.text = health.ToString();
    }

    public void setStartArmor()
    {
        armor = startArmor;
       // armorPoint.text = armor.ToString();
    }
    public void increaseAtackSpeed(int _speed)
    {
        //Debug.Log("БРОНЯ");
        atackSpeed += _speed;
        //armorPoint.text = armor.ToString();
    }
    public void increaseAtackSpeedInPercents(int percent)
    {
        //Debug.Log("БРОНЯ");
        atackSpeed += atackSpeed * percent / 100;
        //armorPoint.text = armor.ToString();
    }
    public void  resetAtackSpeed()
    {
        atackSpeed = startAtackSpeed;
    }
    public void increaseArmor(int _armor)
    {
        //Debug.Log("БРОНЯ");
        armor += _armor;
        //armorPoint.text = armor.ToString();
    }
    public void increaseArmorInPercents(int percent)
    {
        //Debug.Log("БРОНЯ");
        armor += armor* percent/100;
        //armorPoint.text = armor.ToString();
    }
    public void increaseDamage(int _damage)
    {
        //Debug.Log("Урон");
        damage += _damage;
        damagePoint.text = damage.ToString();
    }
    public void increaseDamageInPercents(int percent)
    {
        //Debug.Log("БРОНЯ");
        damage += damage * percent / 100;
        damagePoint.text = damage.ToString();
    }
    public void printArmor()
    {
        armorPoint.text = armor.ToString();
    }
    public void printHealth()
    {
        healthPoint.text = health.ToString();
    }
}
