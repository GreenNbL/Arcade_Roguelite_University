using UnityEngine;


[CreateAssetMenu(fileName ="Food Item", menuName = "Inventory/Item/New Food Item ")]
public class FoodItem : ItemScriptableObject
{
    public int healAmount;

    public GameObject player;

    public override int getHeal()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        return healAmount;
    }
    private void OnEnable()
    {
        typeItem=ItemType.Food;

        clickable = true;

        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    public override void healPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //player.GetComponent<HeroStotistic>().health += healAmount;
        player.GetComponent<HeroStotistic>().healHero(healAmount);
       // player.GetComponent<HeroStotistic>().healthPoint.text = player.GetComponent<HeroStotistic>().health.ToString();
    }
}
