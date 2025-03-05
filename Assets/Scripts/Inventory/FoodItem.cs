using UnityEngine;


[CreateAssetMenu(fileName ="Food Item", menuName = "Inventory/Item/New Food Item ")]
public class FoodItem : ItemScriptableObject
{
    public float healAmount;

    public GameObject player;

    private void OnEnable()
    {
        typeItem=ItemType.Food;

        clickable = true;

        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    public override void healPlayer()
{
        player.GetComponent<HeroStotistic>().health += healAmount;
        player.GetComponent<HeroStotistic>().healthPoint.text = player.GetComponent<HeroStotistic>().health.ToString();
    }
}
