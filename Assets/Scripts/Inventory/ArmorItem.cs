using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum ArmorType { Armor, Helmet, Pants }


[CreateAssetMenu(fileName = "Armor Item", menuName = "Inventory/Item/New Armor Item ")]
public class ArmorItem : ItemScriptableObject
{

    public GameObject player;

    public ArmorType armorType;
    public override int getArmor()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        return characteristic;
    }
    private void OnEnable()
    {
        level = 1;

        characteristic = defaultCharacteristic;

        improveable = true;

        typeItem = ItemType.Armor;

        clickable = true;

        player = GameObject.FindGameObjectWithTag("Player");
    }
    public override void increaseArmorPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<HeroStotistic>().increaseArmor(characteristic);
    }

    public override void decreaseArmorPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<HeroStotistic>().increaseArmor(-characteristic);
    }

    public override ArmorType getArmorType()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        return armorType;
    }
}
