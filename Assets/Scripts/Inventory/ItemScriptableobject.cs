using UnityEngine;

public enum ItemType { Armor, Food, Weapon}
public class ItemScriptableObject : ScriptableObject
{
    public ItemType typeItem;

    public string itemName;

    public bool clickable;

    public int maxAmount;

    public string itemDescription;

    public Sprite icon;

    public GameObject itemPrefab;

    public virtual void healPlayer() { }

    public virtual void damagePlayer() { }

    public virtual void increaseArmorPlayer() { }

    public virtual void decreaseArmorPlayer() { }

    public virtual void increaseDamagePlayer() { }

    public virtual void decreaseDamagePlayer() { }

    public virtual ArmorType getArmorType() {  return ArmorType.Armor; }
}
