using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Item", menuName = "Inventory/Item/New Weapon Item ")]
public class WeaponItem : ItemScriptableObject
{
    public int damageScore;

    public GameObject player;

    private void OnEnable()
    {
        typeItem = ItemType.Weapon;

        clickable = true;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void increaseDamagePlayer() {
        player.GetComponent<HeroStotistic>().increaseDamage(damageScore);
    }

    public override void decreaseDamagePlayer() {
        player.GetComponent<HeroStotistic>().increaseDamage(-damageScore);
    }
}
