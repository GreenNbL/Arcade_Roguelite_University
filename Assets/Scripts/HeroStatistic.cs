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

    public int score = 0;

    public int damage = 10;

    public int healScale = 0;

    public int startHealScale = 0;

    public int startDamage = 10;

    public TMP_Text healthPoint;

    public TMP_Text scrorePoint;

    public TMP_Text scrorePoint2;

    public TMP_Text armorPoint;

    public TMP_Text damagePoint;

    public Image firstLife;

    public Image secondLife;

    public GameObject diedMenu;

    private void Start()
    {
        healthPoint.text= health.ToString();
        scrorePoint.text = score.ToString();
        scrorePoint2.text = score.ToString();
        armorPoint.text= armor.ToString();
        damagePoint.text= damage.ToString();
        diedMenu.SetActive(false);
    }
    public void increadeScore(int _score)
    {
        score += _score;
        scrorePoint.text= score.ToString();
        scrorePoint2.text = score.ToString();
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
     
        if (health - Mathf.Round(_damage * (1 - (armor / 100f))) <= 0)
        {
            health = 0;
            GetComponent<PlayerMovement>().Respawn();
            health = maxHealth;
            if(secondLife.IsActive())
                secondLife.enabled = false;
            else if (firstLife.IsActive())
                firstLife.enabled = false;
            else
            {
                diedMenu.SetActive(true);
                GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
            }    
        }
        else
        {
            health -= Mathf.Round(_damage * (1 - (armor / 100f)));
        }
        healthPoint.text = health.ToString();
    }
    GameObject[] FindEnemiesByTag(string tag)
    {
        GameObject[] enemiesWithTag = GameObject.FindGameObjectsWithTag(tag);

        return enemiesWithTag; 
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
    }
    public void increaseAtackSpeed(int _speed)
    {
        atackSpeed += _speed;   
    }
    public void increaseAtackSpeedInPercents(int percent)
    {
        atackSpeed += atackSpeed * percent / 100;
    }
    public void  resetAtackSpeed()
    {
        atackSpeed = startAtackSpeed;
    }
    public void increaseArmor(int _armor)
    {
        armor += _armor;
    }
    public void increaseArmorInPercents(int percent)
    {
        armor += armor* percent/100;
    }
    public void increaseDamage(int _damage)
    {
        damage += _damage;
        damagePoint.text = damage.ToString();
    }
    public void increaseDamageInPercents(int percent)
    {
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
