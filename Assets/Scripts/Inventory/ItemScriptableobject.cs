using UnityEngine;

public enum ItemType { Default, Food, Weapon}
public class ItemScriptableObject : ScriptableObject
{
    public ItemType typeItem;

    public string itemName;

    public int maxAmount;

    public string itemDescription;

    public Sprite icon;

    public GameObject itemPrefab;
}
