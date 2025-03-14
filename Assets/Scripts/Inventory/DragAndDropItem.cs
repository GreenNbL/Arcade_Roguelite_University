using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;
using System.Text;
/// IPointerDownHandler - ������ �� ��������� ����� �� ������� �� ������� ����� ���� ������
/// IPointerUpHandler - ������ �� ����������� ����� �� ������� �� ������� ����� ���� ������
/// IDragHandler - ������ �� ��� �� ����� �� �� ������� ����� �� �������
public class DragAndDropItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler
{
    public InventorySlot oldSlot;
    private Transform player;

    private void Start()
    {
        //��������� ��� "PLAYER" �� ������� ���������!
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // ������� ������ InventorySlot � ����� � ��������
        oldSlot = transform.GetComponentInParent<InventorySlot>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("����� �� ������");
        // ���������� InventoryManager � ���������
        if (oldSlot != null)
        {
            Debug.Log(oldSlot.item.itemDescription);
            StringBuilder description = new StringBuilder();
            if (!(oldSlot.item is Recipe))
            {
                description.Append(oldSlot.item.itemDescription);
                if (oldSlot.item.typeItem == ItemType.Weapon)
                    description.AppendFormat("\n����: {0}", oldSlot.item.getDamage());
                if (oldSlot.item.typeItem == ItemType.Armor)
                    description.AppendFormat("\n�����: {0}", oldSlot.item.getArmor());
                if (oldSlot.item.typeItem == ItemType.Food)
                    description.AppendFormat("\n�������������� ��������: {0}", oldSlot.item.getHeal());
                if (oldSlot.item.improveable)
                {
                    description.AppendFormat("\n�������: {0}", oldSlot.item.level);
                    if (oldSlot.item.maxLevel > oldSlot.item.level)
                        description.AppendFormat("\n���� �� ���������� ������: {0}", oldSlot.item.maxAmount - oldSlot.amount);
                    foreach (Gain gain in oldSlot.item.gains)
                    {
                        if (oldSlot.item.typeItem == ItemType.Weapon)
                            description.AppendFormat("\n���������� ����� {0}% �� {1} ������", gain.amountChange, gain.levelIncrease + 1);
                        if (oldSlot.item.typeItem == ItemType.Armor)
                            description.AppendFormat("\n���������� ����� {0}% �� {1} ������", gain.amountChange, gain.levelIncrease + 1);
                    }
                }
            }
            else
            {
                description.Append(oldSlot.item.itemDescription);
            }
            InventoryManager.setDescription(description.ToString());
        }
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        // ���� ���� ������, �� �� �� ��������� �� ��� ���� return;
        if (oldSlot.isEmpty)
            return;
       // Debug.Log("������� " + GetComponent<RectTransform>().position);
        //Debug.Log("������� eventData.delta.x " + eventData.delta.x);
       // Debug.Log("������� eventData.delta.y " + eventData.delta.y);
        GetComponent<RectTransform>().position += new Vector3(eventData.delta.x, eventData.delta.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;
        //������ �������� ����������
        GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.75f);
        // ������ ��� ����� ������� ������ �� ������������ ��� ��������
        GetComponentInChildren<Image>().raycastTarget = false;
        // ������ ��� DraggableObject �������� InventoryPanel ����� DraggableObject ��� ��� ������� ������� ���������
        transform.SetParent(transform.parent.parent);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;
        // ������ �������� ����� �� ����������
        GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1f);
        // � ����� ����� ����� ����� �� ������
        GetComponentInChildren<Image>().raycastTarget = true;

        //��������� DraggableObject ������� � ���� ������ ����
        transform.SetParent(oldSlot.transform);
        transform.position = oldSlot.transform.position;
        //���� ����� �������� ��� �������� �� ����� UIPanel, ��...
        if (eventData.pointerCurrentRaycast.gameObject.name == "UIPanel")
        {
            // ������ �������� �� ��������� - ������� ������ ������ ����� ����������
            GameObject itemObject = Instantiate(oldSlot.item.itemPrefab, player.position + Vector3.up + player.forward, Quaternion.identity);
            // ������������� ���������� �������� ����� ����� ���� � �����
            itemObject.GetComponent<Item>().amount = oldSlot.amount;
            // ������� �������� InventorySlot
            NullifySlotData();
        }
        else if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>() != null)
        {
            //���������� ������ �� ������ ����� � ������
            ExchangeSlotData(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>());
        }

    }
    void NullifySlotData()
    {
        Debug.Log("�������� �������� ������");
        // ������� �������� InventorySlot
       /* if (oldSlot.item.typeItem == ItemType.Armor)
        {
            oldSlot.item.decreaseArmorPlayer();
           // InventoryManager.findFirstArmor();
        }*/
        if (oldSlot.item.typeItem == ItemType.Weapon)
        {
            oldSlot.item.decreaseDamagePlayer();
        }
        oldSlot.item = null;
        oldSlot.amount = 0;
        oldSlot.isEmpty = true;
        oldSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        oldSlot.iconGO.GetComponent<Image>().sprite = null;
        oldSlot.textAmount.text = "";
        //oldSlot.outline.SetActive(false);
       
    }
    void ExchangeSlotData(InventorySlot newSlot)
    {
        Debug.Log("�������� ����������� � ������ ����");

        // �������� ������ ������ newSlot � ��������� ����������
        ItemScriptableObject tempItem = newSlot.item;
        int tempAmount = newSlot.amount;
        bool tempIsEmpty = newSlot.isEmpty;
        GameObject tempIconGO = Instantiate(newSlot.iconGO);
        // �������� �������� newSlot �� �������� oldSlot
        newSlot.item = oldSlot.item;
        newSlot.amount = oldSlot.amount;
        newSlot.isEmpty = oldSlot.isEmpty;

        // ��������� ������ � ����� ��� ������ �����
        if (!oldSlot.isEmpty)
        {
            newSlot.SetIcon(oldSlot.iconGO.GetComponent<Image>().sprite);
            newSlot.textAmount.text = oldSlot.amount.ToString();
            newSlot.iconGO.GetComponent<Image>().color = Color.white; // ��������, ��� ������ ������
        }
        else
        {
            // ������� ����� ����
            newSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0); // �������� ������
            newSlot.iconGO.GetComponent<Image>().sprite = null;
            newSlot.textAmount.text = ""; // ������� �����
        }

        // �������� �������� oldSlot �� �������� newSlot ����������� � ����������
        oldSlot.item = tempItem;
        oldSlot.amount = tempAmount;
        oldSlot.isEmpty = tempIsEmpty;

        // ��������� ������ � ����� ��� ������� �����
        if (!tempIsEmpty)
        {
            oldSlot.SetIcon(tempIconGO.GetComponent<Image>().sprite);
            oldSlot.textAmount.text = tempAmount.ToString();
            oldSlot.iconGO.GetComponent<Image>().color = Color.white; // ��������, ��� ������ ������
        }
        else
        {
            // ������� ������ ����
            oldSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0); // �������� ������
            oldSlot.iconGO.GetComponent<Image>().sprite = null;
            oldSlot.textAmount.text = ""; // ������� �����
        }
    }




}
