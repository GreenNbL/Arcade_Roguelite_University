using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UseItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isMouseOver = false;
    private Coroutine applyCoroutine = null;
    public InventorySlot slot;

    private void Start()
    {
       
        // ������� ������ InventorySlot � ����� � ��������
        slot = transform.GetComponentInParent<InventorySlot>();
    }

    void Update()
    {
        if (!slot.isEmpty)
        {
            // ���������, ���� ����� ��� ������� � ������� E ������
            if (isMouseOver && Input.GetKey(KeyCode.E) && slot.item.clickable == true)
            {
                if (applyCoroutine == null)
                {
                    // ��������� �������� ��� ����������
                    applyCoroutine = StartCoroutine(ApplyItemWithDelay(1f)); // �������� � 1 �������
                }
            }
            else
            {
                // ���� ������ E �������� ��� ���� �������� ������, ������������� ��������
                if (applyCoroutine != null)
                {
                    StopCoroutine(applyCoroutine);
                    applyCoroutine = null;
                }
            }
        }
    }

    // ����� ��� ����������� ��������� ���� �� ������
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    // ����� ��� ����������� ��������� ���� ������
    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }

    // ��������, �������������� ���������� �������� � ���������
    private IEnumerator ApplyItemWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // ����� ��������� �������� �� ���������� ��������
        Debug.Log("������� ��������!");
        if (slot.item.typeItem == ItemType.Food)
        {
            slot.item.healPlayer();
            slot.amount -= 1;
            slot.textAmount.text= slot.amount.ToString();
            if (slot.amount==0)
            {
                NullifySlotData();
            }
        }
        // ������� �������� ����� ����������
        applyCoroutine = null;
    }
    void NullifySlotData()
    {
        // ������� �������� InventorySlot
        slot.item = null;
        slot.amount = 0;
        slot.isEmpty = true;
        slot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        slot.iconGO.GetComponent<Image>().sprite = null;
        slot.textAmount.text = "";
    }
}
