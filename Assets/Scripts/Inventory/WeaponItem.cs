using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Item", menuName = "Inventory/Item/New Weapon Item ")]
public class WeaponItem : ItemScriptableObject
{

    public GameObject player;

    public override int getDamage()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        return characteristic;
    }
    private void OnEnable()
    {
        characteristic = defaultCharacteristic;

        level = 1;

        improveable = true;

        typeItem = ItemType.Weapon;

        clickable = true;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void increaseDamagePlayer() {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<HeroStotistic>().increaseDamage(characteristic);
    }

    public override void decreaseDamagePlayer() {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<HeroStotistic>().increaseDamage(-characteristic);
    }
}
