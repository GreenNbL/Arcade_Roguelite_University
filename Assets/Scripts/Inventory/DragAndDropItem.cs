using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;
/// IPointerDownHandler - Следит за нажатиями мышки по объекту на котором висит этот скрипт
/// IPointerUpHandler - Следит за отпусканием мышки по объекту на котором висит этот скрипт
/// IDragHandler - Следит за тем не водим ли мы нажатую мышку по объекту
public class DragAndDropItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public InventorySlot oldSlot;
    private Transform player;

    private void Start()
    {
        //ПОСТАВЬТЕ ТЭГ "PLAYER" НА ОБЪЕКТЕ ПЕРСОНАЖА!
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Находим скрипт InventorySlot в слоте в иерархии
        oldSlot = transform.GetComponentInParent<InventorySlot>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        // Если слот пустой, то мы не выполняем то что ниже return;
        if (oldSlot.isEmpty)
            return;
       // Debug.Log("Позиция " + GetComponent<RectTransform>().position);
        //Debug.Log("Позиция eventData.delta.x " + eventData.delta.x);
       // Debug.Log("Позиция eventData.delta.y " + eventData.delta.y);
        GetComponent<RectTransform>().position += new Vector3(eventData.delta.x, eventData.delta.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;
        //Делаем картинку прозрачнее
        GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.75f);
        // Делаем так чтобы нажатия мышкой не игнорировали эту картинку
        GetComponentInChildren<Image>().raycastTarget = false;
        // Делаем наш DraggableObject ребенком InventoryPanel чтобы DraggableObject был над другими слотами инвенторя
        transform.SetParent(transform.parent.parent);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;
        // Делаем картинку опять не прозрачной
        GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1f);
        // И чтобы мышка опять могла ее засечь
        GetComponentInChildren<Image>().raycastTarget = true;

        //Поставить DraggableObject обратно в свой старый слот
        transform.SetParent(oldSlot.transform);
        transform.position = oldSlot.transform.position;
        //Если мышка отпущена над объектом по имени UIPanel, то...
        if (eventData.pointerCurrentRaycast.gameObject.name == "UIPanel")
        {
            // Выброс объектов из инвентаря - Спавним префаб обекта перед персонажем
            GameObject itemObject = Instantiate(oldSlot.item.itemPrefab, player.position + Vector3.up + player.forward, Quaternion.identity);
            // Устанавливаем количество объектов такое какое было в слоте
            itemObject.GetComponent<Item>().amount = oldSlot.amount;
            // убираем значения InventorySlot
            NullifySlotData();
        }
        else if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>() != null)
        {
            //Перемещаем данные из одного слота в другой
            ExchangeSlotData(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>());
        }

    }
    void NullifySlotData()
    {
        Debug.Log("Пытаемся выкинуть объект");
        // убираем значения InventorySlot
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
        Debug.Log("Пытаемся переместить в другой слот");

        // Временно храним данные newSlot в отдельных переменных
        ItemScriptableObject tempItem = newSlot.item;
        int tempAmount = newSlot.amount;
        bool tempIsEmpty = newSlot.isEmpty;
        GameObject tempIconGO = Instantiate(newSlot.iconGO);
        // Заменяем значения newSlot на значения oldSlot
        newSlot.item = oldSlot.item;
        newSlot.amount = oldSlot.amount;
        newSlot.isEmpty = oldSlot.isEmpty;

        // Обновляем иконку и текст для нового слота
        if (!oldSlot.isEmpty)
        {
            newSlot.SetIcon(oldSlot.iconGO.GetComponent<Image>().sprite);
            newSlot.textAmount.text = oldSlot.amount.ToString();
            newSlot.iconGO.GetComponent<Image>().color = Color.white; // Убедимся, что иконка видима
        }
        else
        {
            // Очищаем новый слот
            newSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0); // Скрываем иконку
            newSlot.iconGO.GetComponent<Image>().sprite = null;
            newSlot.textAmount.text = ""; // Очищаем текст
        }

        // Заменяем значения oldSlot на значения newSlot сохраненные в переменных
        oldSlot.item = tempItem;
        oldSlot.amount = tempAmount;
        oldSlot.isEmpty = tempIsEmpty;

        // Обновляем иконку и текст для старого слота
        if (!tempIsEmpty)
        {
            oldSlot.SetIcon(tempIconGO.GetComponent<Image>().sprite);
            oldSlot.textAmount.text = tempAmount.ToString();
            oldSlot.iconGO.GetComponent<Image>().color = Color.white; // Убедимся, что иконка видима
        }
        else
        {
            // Очищаем старый слот
            oldSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0); // Скрываем иконку
            oldSlot.iconGO.GetComponent<Image>().sprite = null;
            oldSlot.textAmount.text = ""; // Очищаем текст
        }
    }




}
