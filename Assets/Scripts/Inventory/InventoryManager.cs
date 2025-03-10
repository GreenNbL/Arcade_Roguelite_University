using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public Transform inventoryPanel;

    public static List<InventorySlot> slots =new List<InventorySlot>();

    private bool isOpen = false;

    public GameObject UIPanel;
    void Start()
    {
        UIPanel.SetActive(false);
        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            if(inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) {
            if (isOpen)
            {
                UIPanel.SetActive(false);
                isOpen = false;
            }
            else
            {
                UIPanel.SetActive(true);
                isOpen = true;
            }   
        }

    }

    public static void AddItem(ItemScriptableObject _item, int _amount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == _item)
            {
                if (slot.amount + _amount <= _item.maxAmount)
                {
                    slot.amount += _amount;
                    slot.textAmount.text = slot.amount.ToString();
                    �alculateCharacteristics(slot);
                    return;
                }
                break;
            }
        }
        foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty == true)
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                slot.textAmount.text = _amount.ToString();
                �alculateCharacteristics(slot);
                return;
            }
        } 
   
    }

    private static void �alculateCharacteristics(InventorySlot slot)
    {

        if (slot.item.typeItem == ItemType.Armor)
        {
            slot.item.increaseArmorPlayer();
        }
        else if (slot.item.typeItem == ItemType.Weapon)
        {
            slot.item.increaseDamagePlayer();
        }
            
    }

}
