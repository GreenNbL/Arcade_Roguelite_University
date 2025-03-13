using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum RecipeType { AttackBooster, PowerOfProtection, HealthPotion,
    MedicalKit, LeatherArmor, CallBook, ElixirOfRecovery, MagicAmulet, GladiatorArmor }

/*
    Gladiator Armor     ����������� ���� �� 10%         ������ �����������, ���� ��������� � ������ ����� 

    PowerOfProtection   ����������� ������ �� 15        ����������� � ������ 3-� ������ ���������

    HealthPotion        ��������������� 50 ������ ��������      ������ ��������������� 10% ������, ���� ��������� � ��������� �����

    MedicalKit      �������������� 30% �������� �� 5 ������ � ���       ����������� ����� �������� �� 2 �������, ���� � ������� �����

    LeatherArmor        ����������� ������ �� 5, ��������� ����         ������ ������������� � ����������� �� ����������� �����

    CallBook            ����������� �������� ����� �� 35%           ������ ���������, ���� ������� ���������� ��� ����� ���������� 

    ElixirOfRecovery        ��������������� 20% �� ������������� ��������       ����������� ��������������, ���� ������������ � 1-�� �����

    MagicAmulet         ���������� ������������ ����� �� 5%	        ������ ����������, ���� ������ ��������� ��� �������� �������

    GladiatorArmor      ����������� ������ �� 25%            ����������� ������, ���� ��������� ����� �����
    
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
    }
    // �������������� ����� ������������� ��������
    public override void calculateRecipe(List<InventorySlot> slots, int index)
    {
        switch (recipeType)
        {
            case RecipeType.AttackBooster:
                Debug.Log("����������� ��������� �����!");
                calculateAttackBooster(slots, index);
                break;
            case RecipeType.PowerOfProtection:
                Debug.Log("������������ ���� ������!");
                calculatePowerOfProtection(slots, index);
                break;
            case RecipeType.HealthPotion:
                Debug.Log("������������ ����� ��������!");
                calculateHealthPotion(slots, index);
                break;
            case RecipeType.MedicalKit:
                Debug.Log("����������� ����������� �����!");
                calculateMedicalKit(slots, index);
                break;
            case RecipeType.LeatherArmor:
                Debug.Log("������������ ������� �����!");
                calculateLeatherArmor(slots, index);
                break;
            case RecipeType.CallBook:
                Debug.Log("������������ ��������� �����!");
                calculateCallBook(slots, index);
                break;
            case RecipeType.ElixirOfRecovery:
                Debug.Log("����������� ������� ��������������!");
                ///////------------------------
                break;
            case RecipeType.MagicAmulet:
                Debug.Log("����������� ���������� ������!");
                calculateMagicAmulet(slots, index);
                break;
            case RecipeType.GladiatorArmor:
                Debug.Log("������������ ����� ����������!");
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
            return;
        }
        else if ((index + 10) < 15 && slots[index + 10].item != null && slots[index + 10].item.typeItem == ItemType.Armor && slots[index + 5].outline.activeSelf == true)
        {
            player.GetComponent<HeroStotistic>().increaseArmorInPercents(characteristic);
            player.GetComponent<HeroStotistic>().printArmor();
            return;
        }
        else if ((index - 5) >= 0 && slots[index - 5].item != null && slots[index - 5].item.typeItem == ItemType.Armor && slots[index + 5].outline.activeSelf == true)
        {
            player.GetComponent<HeroStotistic>().increaseArmorInPercents(characteristic);
            player.GetComponent<HeroStotistic>().printArmor();
            return;
        }
        else if ((index + 5) < 15 && slots[index + 5].item != null && slots[index + 5].item.typeItem == ItemType.Armor && slots[index + 5].outline.activeSelf == true)
        {
            player.GetComponent<HeroStotistic>().increaseArmorInPercents(characteristic);
            player.GetComponent<HeroStotistic>().printArmor();
            return;
        }
        else if ((index - 10) >= 0 && slots[index - 10].item != null && slots[index - 10].item.typeItem == ItemType.Armor && slots[index + 5].outline.activeSelf == true)
        {
            player.GetComponent<HeroStotistic>().increaseArmorInPercents(characteristic);
            player.GetComponent<HeroStotistic>().printArmor();
            return;
        }
        else if ((index - 5) >= 0 && slots[index - 5].item != null && slots[index - 5].item.typeItem == ItemType.Armor && slots[index + 5].outline.activeSelf == true)
        {
            player.GetComponent<HeroStotistic>().increaseArmorInPercents(characteristic);
            player.GetComponent<HeroStotistic>().printArmor();
            return;
        }

    }
}