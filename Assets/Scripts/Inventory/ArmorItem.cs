using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Armor Item", menuName = "Inventory/Item/New Armor Item ")]
public class ArmorItem : ItemScriptableObject
{
    public int armorScore;

    public GameObject player;

    private void OnEnable()
    {
        typeItem = ItemType.Armor;

        clickable = true;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void increaseArmorPlayer()
    {
        player.GetComponent<HeroStotistic>().increaseArmor(armorScore);
    }

    public override void decreaseArmorPlayer()
    {
        player.GetComponent<HeroStotistic>().increaseArmor(-armorScore);
    }

}
