using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum RecipeType { AttackBooster, PowerOfProtection, HealthPotion,
    MedicalKit, LeatherArmor, CallBook, ElixirOfRecovery, MagicAmulet, GladiatorArmor }

/*
    AttackBooster     Увеличивает урон на 10%         Эффект усиливается, если находится в первой слоте 

    PowerOfProtection   Увеличивает защиту на 15        Срабатывает в первых 3-х слотах инвентаря

    HealthPotion        Восстанавливает 50 единиц здоровья      Эффект восстанавливает 10% больше, если находится в последнем слоте

    MedicalKit      Восстановление 30% здоровья за 5 секунд в бою       Увеличивает время действия на 2 секунды, если в среднем слоте

    LeatherArmor        Увеличивает защиту на 5, уменьшает урон         Эффект увеличивается в зависимости от экипируемой брони

    CallBook            Увеличивает скорость атаки на 35%           Эффект сработает, если предмет находиться под двумя предметами 

    ElixirOfRecovery        Восстанавливает 20% от максимального здоровья       Увеличивает восстановление, если используется в 1-ом слоте

    MagicAmulet         Увеличение критического удара на 5%	        Эффект возрастает, если амулет находится под активным оружием

    GladiatorArmor      Увеличивает защиту на 25%            Увеличивает защиту, если находится среди брони
    
*/
[CreateAssetMenu(fileName = "Recipe Item", menuName = "Inventory/Item/New Recipe Item ")]
public class Recipe : ItemScriptableObject
{
    public RecipeType recipeType;

    public GameObject player;
    private void OnEnable()
    {
        //level = 1;

        improveable = false;

        //typeItem = ItemType.Armor;

        clickable = false;

        player = GameObject.FindGameObjectWithTag("Player");

        switch (recipeType)
        {
            case RecipeType.AttackBooster:
                itemDescription = "Усилитель атаки\nУвеличивает урон на 10%, если находится в первой слоте";
                break;
            case RecipeType.PowerOfProtection:
                itemDescription = "Сила защиты\nУвеличивает защиту на 15, если находится в первых 3-х слотах";
                break;
            case RecipeType.HealthPotion:
                itemDescription = "Зелье здоровья\nУвеличивает восстанавление здоровья на 10%, если находится в последнем слоте";
                break;
            case RecipeType.MedicalKit:
                itemDescription = "Медицинский набор\nУвеличивает восстанавление здоровья на 30%, если находится в среднем слоте";
                break;
            case RecipeType.LeatherArmor:
                itemDescription = "Кожаная броня\nУвеличивает защиту на 25, если броня героя ниже 30\nУвеличивает защиту на 25%, если броня героя выше 30 и ниже 50";
                break;
            case RecipeType.CallBook:
                itemDescription = "Призывная книга\nУвеличивает скорость атаки на 35%, если предмет находиться под двумя предметами";
                break;
            case RecipeType.ElixirOfRecovery:
             
                ///////------------------------
                break;
            case RecipeType.MagicAmulet:
                itemDescription = "Магический амулет\nУвеличивает шанс критического удара на 5%, если амулет находится под активным оружием";
                break;
            case RecipeType.GladiatorArmor:
                itemDescription = "Броня гладиатора\nУвеличивает защиту на 25%, если находится среди брони";
                break;
        }
    }
    // Переопределяем метод использования предмета
    public override void calculateRecipe(List<InventorySlot> slots, int index)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        switch (recipeType)
        {
            case RecipeType.AttackBooster:
                //Debug.Log("Использован Усилитель атаки!");
                calculateAttackBooster(slots, index);
                break;
            case RecipeType.PowerOfProtection:
                //Debug.Log("Использована Сила защиты!");
                calculatePowerOfProtection(slots, index);
                break;
            case RecipeType.HealthPotion:
                //Debug.Log("Использовано Зелье здоровья!");
                calculateHealthPotion(slots, index);
                break;
            case RecipeType.MedicalKit:
                //Debug.Log("Использован Медицинский набор!");
                calculateMedicalKit(slots, index);
                break;
            case RecipeType.LeatherArmor:
                //Debug.Log("Использована Кожаная броня!");
                calculateLeatherArmor(slots, index);
                break;
            case RecipeType.CallBook:
                //Debug.Log("Использована Призывная книга!");
                calculateCallBook(slots, index);
                break;
            case RecipeType.ElixirOfRecovery:
                //Debug.Log("Использован Эликсир восстановления!");
                ///////------------------------
                break;
            case RecipeType.MagicAmulet:
                //Debug.Log("Использован Магический амулет!");
                calculateMagicAmulet(slots, index);
                break;
            case RecipeType.GladiatorArmor:
                //Debug.Log("Использована Броня гладиатора!");
                calculateGladiatorArmor(slots, index);
                break;
        }


         
    }
    
    private void calculateAttackBooster(List<InventorySlot> slots, int index)
    {
        if (index == 0)
        {
            player.GetComponent<HeroStotistic>().increaseDamageInPercents(characteristic);
        }
    }
    private void calculatePowerOfProtection(List<InventorySlot> slots, int index)
    {
        if (index >= 0 && index <= 3)
        {
            player.GetComponent<HeroStotistic>().increaseArmor(characteristic);
            player.GetComponent<HeroStotistic>().printArmor();
        }
    }
    private void calculateHealthPotion(List<InventorySlot> slots, int index)
    {
        if (index == 14)
        {
            player.GetComponent<HeroStotistic>().healScale+= characteristic;
        }
    }
    private void calculateMedicalKit(List<InventorySlot> slots, int index)
    {
        if (index == 7)
        {
            player.GetComponent<HeroStotistic>().healScale += characteristic;
        }
    }
    private void calculateLeatherArmor(List<InventorySlot> slots, int index)
    {
        if (player.GetComponent<HeroStotistic>().armor<30)
        {
            player.GetComponent<HeroStotistic>().increaseArmor(25);
        }
        if (player.GetComponent<HeroStotistic>().armor > 30 && player.GetComponent<HeroStotistic>().armor <50)
        {
            player.GetComponent<HeroStotistic>().increaseArmorInPercents(25);
        }
        player.GetComponent<HeroStotistic>().printArmor();
    }
    private void calculateCallBook(List<InventorySlot> slots, int index)
    {
        if (index-5>=0 && slots[index - 5].item!=null && index - 10 >= 0 && slots[index - 10].item != null)
        {
            player.GetComponent<HeroStotistic>().increaseAtackSpeedInPercents(characteristic);
        }
    }

    private void calculateMagicAmulet(List<InventorySlot> slots, int index)
    {
        if (index - 5 >= 0 && slots[index - 5].item != null &&  slots[index - 5].item.typeItem == ItemType.Weapon && slots[index - 5].outline.activeSelf==true)
        {
            player.GetComponent<HeroStotistic>().increaseChanceSciticalDamage(characteristic);
        }
    }
    private void calculateGladiatorArmor(List<InventorySlot> slots, int index)
    {
        if ((index + 5) < 15 && slots[index + 5].item != null && slots[index + 5].item.typeItem == ItemType.Armor && slots[index + 5].outline.activeSelf == true)
        {
            player.GetComponent<HeroStotistic>().increaseArmorInPercents(characteristic);
            player.GetComponent<HeroStotistic>().printArmor();
            Debug.Log("Среди брони!");
            return;
        }
        else if ((index + 10) < 15 && slots[index + 10].item != null && slots[index + 10].item.typeItem == ItemType.Armor && slots[index + 10].outline.activeSelf == true)
        {
            player.GetComponent<HeroStotistic>().increaseArmorInPercents(characteristic);
            player.GetComponent<HeroStotistic>().printArmor();
            Debug.Log("Среди брони!");
            return;
        }
        else if ((index - 5) >= 0 && slots[index - 5].item != null && slots[index - 5].item.typeItem == ItemType.Armor && slots[index - 5].outline.activeSelf == true)
        {
            player.GetComponent<HeroStotistic>().increaseArmorInPercents(characteristic);
            player.GetComponent<HeroStotistic>().printArmor();
            Debug.Log("Среди брони!");
            return;
        }
        else if ((index - 10) >= 0 && slots[index - 10].item != null && slots[index - 10].item.typeItem == ItemType.Armor && slots[index - 10].outline.activeSelf == true)
        {
            player.GetComponent<HeroStotistic>().increaseArmorInPercents(characteristic);
            player.GetComponent<HeroStotistic>().printArmor();
            Debug.Log("Среди брони!");
            return;
        }

    }
}