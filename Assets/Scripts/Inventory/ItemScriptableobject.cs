using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Armor, Food, Weapon , Recipe}
public class ItemScriptableObject : ScriptableObject
{
    public int level = 1;

    public int maxLevel = 5;

    public bool improveable = false;

    public ItemType typeItem;

    public string itemName;

    public bool clickable;

    public int maxAmount;

    public string itemDescription;

    public int characteristic;

    public int defaultCharacteristic;

    public Sprite icon;

    public GameObject itemPrefab;

    public List<Gain> gains;

    public void setLevel(int _level)
        { this.level = _level; }
    public int getLevel()
        { return level; }
    public virtual void healPlayer() { }

    public virtual void damagePlayer() { }

    public virtual void increaseArmorPlayer() { }

    public virtual void increaseArmorInPercentsPlayer() { }

    public virtual void decreaseArmorPlayer() { }

    public virtual void increaseDamagePlayer() { }

    public virtual void increaseSpeedAtackPlayer() { }

    public virtual void increaseSpeedAtackInPercentsPlayer() { }

    public virtual void increaseDamageInPercentsPlayer() { }

    public virtual void decreaseDamagePlayer() { }

    public virtual void calculateRecipe(List<InventorySlot> slots, int index) { }
    public virtual int getDamage() { return 0; }
    public virtual int getArmor() { return 0; }
    public virtual int getHeal() { return 0; }
    public virtual ArmorType getArmorType() {  return ArmorType.Armor; }
}
