using UnityEngine;

public enum ItemType { Default, Food, Weapon}
public abstract class ItemScriptableObject : ScriptableObject
{
    public ItemType typeItem;

    public string itemName;

    public bool clickable;

    public int maxAmount;

    public string itemDescription;

    public Sprite icon;

    public GameObject itemPrefab;

    public abstract void healPlayer();
}
