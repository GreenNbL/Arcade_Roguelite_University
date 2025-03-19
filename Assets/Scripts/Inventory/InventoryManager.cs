using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.Progress;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;
public class InventoryManager : MonoBehaviour
{
    public Transform inventoryPanel;

    public static List<InventorySlot> slots =new List<InventorySlot>();

    private bool isOpen = false;

    private static bool isFirstArmor = false;

    private static bool isFirstWeapon = false;

    public GameObject UIPanel;

    public static GameObject description;
    void Start()
    {
        description=GameObject.Find("Canvas/UIPanel/DescriptionPanel/Description");
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
                CalculateArmor();
                CalculateWeaponDamage();
                сalculateRecipe();
            }   
        }
        if(isOpen)
        {
            CalculateArmor();
            CalculateWeaponDamage();
            сalculateRecipe();
        }
    }
    private void improveItem( InventorySlot slot)
    {

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
                    сalculateCharacteristics(slot);
                    updateLevel(slot);
                    return;
                }
                break;
            }
        }
        int i = 0;
        foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty == true)
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                slot.textAmount.text = _amount.ToString();
                if (!isFirstArmor && slot.item.typeItem==ItemType.Armor)
                {
                    //slot.item.armor
                    slot.outline.SetActive(true);
                    isFirstArmor = true;
                }
                if (!isFirstWeapon && slot.item.typeItem == ItemType.Weapon)
                {
                    //slot.item.armor
                    slot.outline.SetActive(true);
                    isFirstWeapon = true;
                }
                if (slot.item is Recipe)
                {
                    slot.item.calculateRecipe(slots, i);
                }
                сalculateCharacteristics(slot);
                return;
            }
            i++;
        } 
   
    }
    public static void сalculateRecipe()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<HeroStotistic>().setStartChanceSciticalDamage();
        GameObject.FindGameObjectWithTag("Player").GetComponent<HeroStotistic>().resetAtackSpeed();
        GameObject.FindGameObjectWithTag("Player").GetComponent<HeroStotistic>().setStarthealScale();
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];

            // Проверяем, активен ли outline и есть ли предмет в слоте
            if (!slot.isEmpty && slot.item is Recipe)
            {
                slot.item.calculateRecipe(slots, i);
            }
        }
    }
    public static void CalculateArmor()
    {
        // Создаем переменные для хранения информации об экипировке
        InventorySlot helmetSlot = null;
        InventorySlot chestSlot = null;
        InventorySlot pantsSlot = null;
        GameObject.FindGameObjectWithTag("Player").GetComponent<HeroStotistic>().setStartArmor();
        // Итерируемся по слотам и проверяем их положение
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];

            // Проверяем, активен ли outline и есть ли предмет в слоте
            if (!slot.isEmpty  && slot.outline.activeSelf)
            {
                // Проверяем тип предмета и сохраняем в соответствующие переменные
                if (slot.item.typeItem == ItemType.Armor)
                {
                    ArmorType armorType = slot.item.getArmorType();

                    // Устанавливаем позиции в зависимости от значения индекса i
                    switch (armorType)
                    {
                        case ArmorType.Helmet:
                            // Шлем должен быть в первой строке (индексы 0-4)
                            /*if (i < 5 && helmetSlot == null) // Обеспечиваем, что только один шлем
                            {
                                helmetSlot = slot;
                            }*/
                            helmetSlot = slot;
                            if ((i+5)<15 && slots[i + 5].item!= null && slots[i + 5].item.getArmorType() == ArmorType.Armor)
                                chestSlot = slots[i + 5];
                            if ((i + 10) < 15 && slots[i + 10].item != null && slots[i + 10].item.getArmorType() == ArmorType.Pants)
                                pantsSlot = slots[i + 10];
                            break;
                        case ArmorType.Armor:
                            // Броня должна быть во второй строке (индексы 5-9)
                            /*if (i >= 5 && i < 10 && chestSlot == null)
                            {
                                chestSlot = slot;
                            }*/
                            chestSlot = slot;
                            if ((i - 5) >=0 && slots[i - 5].item != null && slots[i - 5].item.getArmorType() == ArmorType.Helmet)
                                helmetSlot = slots[i - 5];
                            if ((i + 5) < 15 && slots[i + 5].item != null && slots[i + 5].item.getArmorType() == ArmorType.Pants)
                                pantsSlot = slots[i + 5];
                            break;
                        case ArmorType.Pants:
                            // Штаны должны быть в третьей строке (индексы 10-14)
                            /*if (i >= 10 && i < 15 && pantsSlot == null)
                            {
                                pantsSlot = slot;
                            }*/
                            pantsSlot = slot;
                            if ((i - 10) >=0 && slots[i - 10].item != null && slots[i - 10].item.getArmorType() == ArmorType.Helmet)
                                helmetSlot = slots[i - 5];
                            if ((i - 5) >= 0 && slots[i - 5].item != null && slots[i - 5].item.getArmorType() == ArmorType.Armor)
                                chestSlot = slots[i - 5];
                            break;
                    }
                    break ;
                }
            }
        }

        // Начинаем подсчет брони
       // int totalArmorValue = 0;

        // Смотрим на состояние шлема
        if (helmetSlot != null)
        {
           // Debug.Log("Есть шлем ");
            helmetSlot.item.increaseArmorPlayer(); // Предполагаем, что есть метод получения значения брони
        }

        // Смотрим на состояние бронь и штанов
        if (chestSlot != null)
        {
            //Debug.Log("Есть броня ");
            chestSlot.item.increaseArmorPlayer();
        }
        if (pantsSlot != null)
        {
            //Debug.Log("Есть штаны ");
            pantsSlot.item.increaseArmorPlayer();
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<HeroStotistic>().printArmor();
    }
    public static void CalculateWeaponDamage()
    {
        // Создаем переменные для хранения информации об экипировке
        InventorySlot weaponSlot = null;
      
        GameObject.FindGameObjectWithTag("Player").GetComponent<HeroStotistic>().setStartDamage();
        // Итерируемся по слотам и проверяем их положение
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];

            // Проверяем, активен ли outline и есть ли предмет в слоте
            if (!slot.isEmpty && slot.outline.activeSelf)
            {
                // Проверяем тип предмета и сохраняем в соответствующие переменные
                if (slot.item.typeItem == ItemType.Weapon)
                {
                    weaponSlot = slot;
                    break;
                }
            }
        }
        if (weaponSlot != null)
        {
           // Debug.Log("Есть оружие ");
            weaponSlot.item.increaseDamagePlayer(); // Предполагаем, что есть метод получения значения брони
        }
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<HeroStotistic>().damage == 0)
            GameObject.FindGameObjectWithTag("Player").GetComponent<HeroStotistic>().damage = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroStotistic>().startDamage;
        GameObject.FindGameObjectWithTag("Player").GetComponent<HeroStotistic>().printDamage();
    }
    private static void сalculateCharacteristics(InventorySlot slot)
    {

        if (slot.item.typeItem == ItemType.Armor)
        {
            CalculateArmor();
        }
        else if (slot.item.typeItem == ItemType.Weapon)
        {
            CalculateWeaponDamage();
        }
            
    }
    public static void setInactiveOutline(ItemType type)
    {
        foreach (InventorySlot slot in slots)
        {
            if (!slot.isEmpty)
            {
                if (slot.item.typeItem == type)
                {
                    slot.outline.SetActive(false);
                }
            }
        }
    }
    public static void setDescription(string _description)
    {
        description.GetComponent<TMP_Text>().text= _description; 
    }
    public static void updateLevel(InventorySlot slot)
    {
        if (slot.item.improveable && slot.item.maxAmount==slot.amount && slot.item.maxLevel>slot.item.level)
        {
            foreach (Gain gain in slot.item.gains)
            {
                if(gain.levelIncrease==slot.item.level)
                {
                    slot.item.characteristic += slot.item.characteristic * gain.amountChange / 100;
                    slot.amount = 1;
                    slot.textAmount.text= 1.ToString();
                    slot.item.level++;
                    return;
                }
            }
        }
    }
}
